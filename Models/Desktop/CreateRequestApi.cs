namespace AuthenticationAPI.Models.Desktop;

using System.ComponentModel.DataAnnotations;
using AuthenticationAPI.Entities;
using RequestMaker.UserRequests;

public class CreateRequestApi : CreateRequest
{
    [Required]
    [EnumDataType(typeof(Role))]
    public new string? Role { get; set; }
}