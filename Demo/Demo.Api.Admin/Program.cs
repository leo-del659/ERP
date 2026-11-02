using System.Text;
using Demo.Api.Admin.Conventions;
using Demo.Api.Admin.Filters;
using Demo.Api.Admin.Hosted;
using Demo.Api.Admin.Middles;
using Demo.Infrastructure.EFCore;
using Demo.Infrastructure.Tools;
using Demo.Repository.Instance;
using Demo.Repository.Interface;
using Demo.Service.Instance;
using Demo.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(o =>
{
    o.Filters.Add<Action_Filter>();
    o.Filters.Add<Permission_Filter>();
    o.Conventions.Add(new ApiVersionPrefixConvention());
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddPolicy("web", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var connectionString = builder.Configuration.GetConnectionString("ErpDb")
    ?? throw new InvalidOperationException("未找到连接字符串 ConnectionStrings:ErpDb");
builder.Services.AddDbContext<ErpDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var jwt = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SecretKey"]!)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

var redisCs = builder.Configuration["Redis:Connection"] ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(redisCs + ",abortConnect=false")));
builder.Services.AddSingleton<Redis_Cache>();
builder.Services.AddSingleton<JWT_Helper>();
builder.Services.AddSingleton<RabbitMq_Bus>();
builder.Services.AddScoped<BillNo_Helper>();
builder.Services.AddHostedService<StockAlert_HostedService>();

builder.Services.AddScoped(typeof(I_Repository<>), typeof(Repository<>));
builder.Services.AddScoped_Extension(typeof(I_Auth_Service).Assembly, typeof(Auth_Service).Assembly);

var app = builder.Build();

    // 项目未使用 EF Migration，启动时确保审批规则表存在
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ErpDbContext>();
        db.Database.ExecuteSqlRaw(@"
        CREATE TABLE IF NOT EXISTS sys_approval_rule (
          id BIGINT NOT NULL AUTO_INCREMENT,
          biz_type VARCHAR(30) NOT NULL,
          min_amount DECIMAL(18,2) NOT NULL DEFAULT '0.00',
          max_amount DECIMAL(18,2) NULL,
          level INT NOT NULL DEFAULT 1,
          node_roles VARCHAR(200) NOT NULL DEFAULT 'admin',
          enabled TINYINT NULL DEFAULT 1,
          remark VARCHAR(200) NULL,
          create_time DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
          PRIMARY KEY (id),
          KEY idx_rule_biz (biz_type)
        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;");
        // 补充节点角色列（列已存在则忽略）
        try { db.Database.ExecuteSqlRaw("ALTER TABLE sys_approval ADD COLUMN node_roles VARCHAR(200) NULL"); }
        catch { /* 列已存在 */ }
        // 补充负责人姓名列，并从员工表回填
        try { db.Database.ExecuteSqlRaw("ALTER TABLE hr_department ADD COLUMN manager_name VARCHAR(50) NULL"); }
        catch { /* 列已存在 */ }
    try
    {
        db.Database.ExecuteSqlRaw(@"UPDATE hr_department d
            JOIN hr_employee e ON d.manager_id = e.id
            SET d.manager_name = e.name
            WHERE (d.manager_name IS NULL OR d.manager_name = '') AND d.manager_id IS NOT NULL");
    }
    catch { /* 回填失败不影响启动 */ }
    // 各业务单据通用审批状态列（列已存在则忽略）
    foreach (var tbl in new[] { "po_order", "po_receipt", "po_requisition", "so_quote", "so_customer_credit", "fin_voucher", "fin_receipt", "fin_payment", "inv_in_order", "inv_out_order", "inv_transfer", "inv_check" })
    {
        try { db.Database.ExecuteSqlRaw($"ALTER TABLE {tbl} ADD COLUMN audit_status TINYINT NOT NULL DEFAULT 0"); }
        catch { /* 列已存在 */ }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("web");
app.UseMiddleware<RequestBuffer_MiddleWare>();
app.UseMiddleware<Exception_MiddleWare>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
