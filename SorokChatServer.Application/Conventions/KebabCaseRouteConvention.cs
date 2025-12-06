using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SorokChatServer.Application.Conventions;

public class KebabCaseRouteConvention : Attribute, IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        var controllerName = controller.ControllerName;
        var kebabName = ToKebabCase(controllerName);
        controller.Selectors[0].AttributeRouteModel = new AttributeRouteModel
        {
            Template = kebabName
        };
    }

    private static string ToKebabCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        return string.Concat(input.Select((x, i) =>
            i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString()
        )).ToLower();
    }
}