namespace AuthenticationAPI.Controllers;

using static BCrypt.Net.BCrypt;
using AuthenticationAPI.Helpers;
using AuthenticationAPI.Models;
using AuthenticationAPI.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;

    public LoginController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public IActionResult Login(LoginRequest model)
    {
        // Check if the user exists in the database based on the provided login
        var login = model.Login ?? throw new AppException("Login cannot be empty");
        
        var user = _userService.GetUserByLogin(login);

        if (user == null)
        {
            // User not found, return appropriate response
            return NotFound("User not found");
        }
        else  
        {
            bool isPasswordValid = Verify(model.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                // Password is incorrect, return appropriate response
                return BadRequest("Incorrect password");
            }
        }

        return Ok(user);
    }
}