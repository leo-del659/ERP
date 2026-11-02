using Demo.Infrastructure.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Demo.Infrastructure.Tools
{
    public static class Service_Extension
    {
        /// <summary>
        /// 容错获取程序集类型。
        /// GetTypes() 在任一类型因依赖无法加载时会整体抛出 ReflectionTypeLoadException，
        /// 这里捕获后返回「成功加载的部分类型」，并把失败原因打到日志，避免单个坏类型拖垮整个应用启动。
        /// </summary>
        private static Type[] SafeGetTypes(Assembly asm)
        {
            try
            {
                return asm.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var le in ex.LoaderExceptions)
                {
                    if (le != null)
                        Console.Error.WriteLine($"[Service_Extension] 类型加载失败({asm.GetName().Name}): {le.Message}");
                }
                return ex.Types.Where(t => t != null).Select(t => t!).ToArray();
            }
        }

        /// <summary>
        /// 批量注入AddScoped
        /// </summary>
        /// <param name="service">原始类</param>
        /// <param name="providers">提供者程序集</param>
        /// <param name="injects">注入者程序集</param>
        public static void AddScoped_Extension(this IServiceCollection service,Assembly providers,Assembly injects)
        {
           
            var pros = SafeGetTypes(providers).Where(d => d.GetCustomAttribute(typeof(Provider_Attribute)) != null
            ).ToList();
            var injs = SafeGetTypes(injects).Where(d => d.GetCustomAttribute(typeof(Inject_Attribute)) != null
           ).ToList();
            foreach (var item in pros)
            {
               var bfs= injs.Where(d => item.IsAssignableFrom(d)).ToList();//找出injs中所有是item 的子类的类型

                bfs.ForEach(e =>
                {
                    service.AddScoped(item, e);
                });
            }
        }


        /// <summary>
        /// 批量注入AddTransient
        /// </summary>
        /// <param name="service">原始类</param>
        /// <param name="providers">提供者程序集</param>
        /// <param name="injects">注入者程序集</param>
        public static void AddTransient_Extension(this IServiceCollection service, Assembly providers, Assembly injects)
        {

            var pros = SafeGetTypes(providers).Where(d => d.GetCustomAttribute(typeof(Provider_Attribute)) != null
            ).ToList();
            var injs = SafeGetTypes(injects).Where(d => d.GetCustomAttribute(typeof(Inject_Attribute)) != null
           ).ToList();
            foreach (var item in pros)
            {
                var bfs = injs.Where(d => item.IsAssignableFrom(d)).ToList();//找出injs中所有是item 的子类的类型

                bfs.ForEach(e =>
                {
                    service.AddTransient(item, e);
                });
            }
        }



        /// <summary>
        /// 批量注入AddSingleton
        /// </summary>
        /// <param name="service">原始类</param>
        /// <param name="providers">提供者程序集</param>
        /// <param name="injects">注入者程序集</param>
        public static void AddSingleton_Extension(this IServiceCollection service, Assembly providers, Assembly injects)
        {

            var pros = SafeGetTypes(providers).Where(d => d.GetCustomAttribute(typeof(Provider_Attribute)) != null
            ).ToList();
            var injs = SafeGetTypes(injects).Where(d => d.GetCustomAttribute(typeof(Inject_Attribute)) != null
           ).ToList();
            foreach (var item in pros)
            {
                var bfs = injs.Where(d => item.IsAssignableFrom(d)).ToList();//找出injs中所有是item 的子类的类型

                bfs.ForEach(e =>
                {
                    service.AddSingleton(item, e);
                });
            }
        }

    }
}
