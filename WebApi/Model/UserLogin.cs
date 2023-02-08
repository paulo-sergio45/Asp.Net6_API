using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Model;
public class UserLogin
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

}

