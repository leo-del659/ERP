using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Demo.Api.Admin.Conventions;

/// <summary>
/// 路由版本约定：将 "api/..." 全局改写为 "api/v1/..."，
/// 所有控制器路由统一带版本前缀，无需逐个修改控制器。
/// 仅处理以 "api/" 开头的属性路由。
/// </summary>
public class ApiVersionPrefixConvention : IControllerModelConvention
{
    private const string Prefix = "api/";
    private const string Versioned = "api/v1/";

    private static void Rewrite(SelectorModel? selector)
    {
        var tpl = selector?.AttributeRouteModel?.Template;
        if (string.IsNullOrEmpty(tpl) || !tpl!.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
            return;
        selector!.AttributeRouteModel = new AttributeRouteModel
        {
            Template = Versioned + tpl[Prefix.Length..]
        };
    }

    public void Apply(ControllerModel controller)
    {
        foreach (var s in controller.Selectors) Rewrite(s);
        foreach (var action in controller.Actions)
            foreach (var s in action.Selectors) Rewrite(s);
    }
}
