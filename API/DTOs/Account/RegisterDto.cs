using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
  [Required]
  [MinLength(4, ErrorMessage = "FirstName must be greater than or equal to 4 characters.")]
  [MaxLength(50, ErrorMessage = "FirstName cannot be longer than 40 characters.")]
  public string firstName { get; set; }
  
  [Required]
  [MinLength(4, ErrorMessage = "LastName must be greater than or equal to 4 characters.")]
  [MaxLength(50, ErrorMessage = "LastName cannot be longer than 40 characters.")]
  public string lastName { get; set; }
  
  [Required]
  [EmailAddress]
  public string email { get; set; }
  
  [Required]
  public string password { get; set; }
}
