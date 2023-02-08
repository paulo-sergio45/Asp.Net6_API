using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Model;
public class UserRegister
{
    [Required]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string NickName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get;  set; }
}

