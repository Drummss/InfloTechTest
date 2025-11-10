namespace UserManagement.UI.DTOs;

public class CreateUserRequestModel
{
    public string? Forename { get; set; }

    public string? Surname { get; set; }

    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public bool IsActive { get; set; } = true;
}
