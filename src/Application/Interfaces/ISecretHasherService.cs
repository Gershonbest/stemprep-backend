namespace Application.Interfaces;

public interface ISecretHasherService
{
    string Hash(string input);
    bool Verify(string input, string hashString);
}


