namespace Application.Auth
{
    public interface IUserValidator
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
    }
    public interface IPasswordValidator
    {
        string NewPassword { get; set; }
        string ConfirmPassword { get; set; }
    }
    public interface IChildValidator
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
