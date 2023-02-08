using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Model;
public class UserResponse
{
    public UserResponse(long id, string name, string nickName, string email)
    {
        Id = id;
        Name = name;
        NickName = nickName;
        Email = email;
    }

    public long Id { get; set; }
    public string Name { get; set; }
    public string NickName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

}

