using System.ComponentModel.DataAnnotations;

namespace RepoWithUoW.Domain;

public class RegisterDTO
{

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? FullName { get; set; }   

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}
