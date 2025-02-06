using Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services;

public class SecretHasherService : ISecretHasherService // Rename interface to represent PIN handling if needed.  
{
    public string Hash(string pin)
    {
        // Convert the integer pin to a string to hash it  
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(pin));
        StringBuilder builder = new();
        foreach (byte b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }

    public bool Verify(string enteredPin, string storedHashedPin)
    {
        string hashedPin = Hash(enteredPin);
        return hashedPin.Equals(storedHashedPin);
    }
}