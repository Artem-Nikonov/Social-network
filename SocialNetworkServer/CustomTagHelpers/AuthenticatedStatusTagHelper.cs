using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SocialNetworkServer.CustomTagHelpers.Enums;
namespace SocialNetworkServer.CustomTagHelpers
{
    //Данный TagHelper скрывает или отображает элемент в зависимости от того, аутентифицирован ли пользователь
    [HtmlTargetElement(Attributes = "authentication-status")]
    public class AuthenticationStatusTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }
        public AuthenticationStatus authenticationStatus { get; set; } = AuthenticationStatus.Authenticated;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var isAuthenticated = ViewContext?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated && authenticationStatus == AuthenticationStatus.Authenticated ||
                isAuthenticated && authenticationStatus == AuthenticationStatus.NotAuthenticated)
                output.SuppressOutput();
        }

    }
}
