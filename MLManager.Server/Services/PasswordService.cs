using System;
using System.Text.RegularExpressions;

namespace MLManager.Services
{
    public class PasswordService : IPasswordService
    {
        public bool VerifyPassword(string password)
        {
            //Probably should be a flag enum
            if(password == null)
            {
                return false;                
            }

            if(password.Length < 8)
            {
                return false;
            }

            if(!Regex.IsMatch(password,"[a-z]"))
            {
                return false;
            }
            
            if(!Regex.IsMatch(password,"[A-Z]"))
            {
                return false;
            }

            if(!Regex.IsMatch(password,"[0-9]"))
            {
                return false;
            }

            return true;
        }

        public string HashPassword(string password)
        {
            if(password == null)
                throw new ArgumentNullException("password");

            //Should verify that a password matches our expectations
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password,12);
        }

        public bool CheckPassword(string password,string passwordHash)
        {
            if(password == null)
                throw new ArgumentNullException("password");

            if(passwordHash == null)
                throw new ArgumentNullException("passwordHash");

            return BCrypt.Net.BCrypt.EnhancedVerify(password,passwordHash);
        }
    }
}