namespace Application.Auth
{
    public interface IUserValidator
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
    }
}
