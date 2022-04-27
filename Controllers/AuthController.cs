using parser_cont.Models.LoginJMU;

namespace parser_cont.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    /// <summary>
    /// Post Login to JMU
    /// </summary>
    /// <param name="username">Login</param>
    /// <param name="password">Password</param>
    /// <returns>Model - User</returns>
    [HttpPost(Name = "LoginInJMU")]
    public async Task<User> Auth(string username = "1978" , string password = "root")
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
}

