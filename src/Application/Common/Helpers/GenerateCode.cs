namespace Application.Common.Helpers;

public static class GenerateCode
{
    public static string GenerateRegistrationCode()
    {
        Random random = new();
        return random.Next(100000, 999999).ToString(); // Generates a 6-digit random number
    }
}