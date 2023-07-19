namespace AuthenticationAPI.Models.Desktop;

using System.ComponentModel.DataAnnotations;
using AuthenticationAPI.Entities;
using RequestMaker.UserRequests;

public class UpdateRequestApi : UpdateRequest
{
    [EnumDataType(typeof(Role))]
    public new string? Role { get; set; }
}