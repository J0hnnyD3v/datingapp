using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class LoginDto
{
  [Required]
  [EmailAddress]
  public string email { get; set; }
  
  [Required]
  public string password { get; set; }
}
