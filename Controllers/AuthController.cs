using parser_cont.Models.LoginJMU;
using parser_cont.Services.Firebird;

namespace parser_cont.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    [HttpPost(Name = "LoginInJMU")]
    public async Task<User> Auth(string username = "1978" , string password = "root")
    {
        try
        {
            using ClientApi client = new("http://localhost:8080");

            User user = new()
            {
                UserName = username,
                Password = CustomAes256.Encrypt(password, "8UHjPgXZzXDgkhqV2QCnooyJyxUzfJrO"),
                IdModules = ModulesProject.Education,

            };
            return await client.PostAsync<User>("api/auth", user);

        }
        catch (HttpRequestException ex)
        {
            throw ex;
        }
    }
}

