using System;  
using System.Text;  

namespace Application.Auth  
{  
    public static class PasswordGenerator  
    {  
        private static readonly Random Random = new Random();  

        public static string GenerateRandomPassword(int length = 10)  
        {  
            if (length < 8)  
            {  
                throw new ArgumentException("Password length should be at least 8 characters.");  
            }  

            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";  
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";  
            const string numbers = "0123456789";  
            const string specialChars = "!@#$%^&*()-_=+[]{};:,.<>?/|`~";  

            // Ensure we have at least one of each type of character  
            StringBuilder password = new StringBuilder();  
            password.Append(upperChars[Random.Next(upperChars.Length)]);  
            password.Append(lowerChars[Random.Next(lowerChars.Length)]);  
            password.Append(numbers[Random.Next(numbers.Length)]);  
            password.Append(specialChars[Random.Next(specialChars.Length)]);  

            // Fill the rest of the password length with a mix of characters  
            string allChars = upperChars + lowerChars + numbers + specialChars;  
            for (int i = password.Length; i < length; i++)  
            {  
                password.Append(allChars[Random.Next(allChars.Length)]);  
            }  

            // Shuffle the password to randomize character positions  
            return ShuffleString(password.ToString());  
        }  

        private static string ShuffleString(string str)  
        {  
            char[] array = str.ToCharArray();  
            for (int i = array.Length - 1; i > 0; i--)  
            {  
                int j = Random.Next(i + 1);  
                // Swap array[i] with the element at random index  
                char temp = array[i];  
                array[i] = array[j];  
                array[j] = temp;  
            }  
            return new string(array);  
        }  
    }  
}