using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Testing.Controller;

public class BaseControllerTest : BaseTest
{
    protected ControllerContext SetupControllerContext()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, "Chrille@chrillescompany.com")
        }, "TestAuthentication"));

        var controllerContext = new ControllerContext();
        controllerContext.HttpContext = new DefaultHttpContext()
        {
            User = user
        };

        return controllerContext;
    }
}