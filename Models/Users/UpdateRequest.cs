namespace AuthenticationAPI.Models.Users;

using System.ComponentModel.DataAnnotations;
using AuthenticationAPI.Entities;

public class UpdateRequest
{
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [EnumDataType(typeof(Role))]
    public string? Role { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    // treat empty string? as null for password fields to 
    // make them optional in front end apps
    private string? _password;
    [MinLength(6)]
    public string? Password
    {
        get => _password;
        set => _password = ReplaceEmptyWithNull(value);
    }

    private string? _confirmPassword;
    [Compare("Password")]
    public string? ConfirmPassword 
    {
        get => _confirmPassword;
        set => _confirmPassword = ReplaceEmptyWithNull(value);
    }

    // helpers

    private static string? ReplaceEmptyWithNull(string? value)
    {
        // replace empty string? with null to make field optional
        return string.IsNullOrEmpty(value) ? null : value;
    }
}