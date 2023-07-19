namespace AuthenticationAPI.Controllers;

using static BCrypt.Net.BCrypt;
using AuthenticationAPI.Models.Desktop;
using AuthenticationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;

    public LoginController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("{isDesktop}")]
    public IActionResult Login(bool isDesktop, LoginRequestApi model)
    {
        // Check if the user exists in the database based on the provided login
        string s_error_msg = "Login cannot be empty";
        var exc = isDesktop ? new Win32Exception(s_error_msg) : new Exception(s_error_msg);
        if(model.Login == null)
        {
            return BadRequest(exc);        
        }

        var user = _userService.GetUserByLogin(model.Login);

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