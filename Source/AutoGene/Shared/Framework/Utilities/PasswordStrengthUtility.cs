using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Shared.Enum;

namespace Shared.Framework.Utilities
{
    public static class PasswordStrengthUtility
    {
        public static PasswordScore CheckStrength(string password)
        {
            int score = 0;

            if (password.Length < 1)
                return PasswordScore.Blank;
            //if (password.Length < 4)
            //    return PasswordScore.VeryWeak;

            if (password.Length >= 8)
                return PasswordScore.Strong;

            //if (password.Length >= 8)
            //    score++;
            //if (password.Length >= 12)
            //    score++;
            //if (Regex.Match(password, @"/\d+/", RegexOptions.ECMAScript).Success)
            //    score++;
            //if (Regex.Match(password, @"/[a-z]/", RegexOptions.ECMAScript).Success ||
            //    Regex.Match(password, @"/[A-Z]/", RegexOptions.ECMAScript).Success)
            //    score++;
            //if (Regex.Match(password, @"/.[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]/", RegexOptions.ECMAScript).Success)
            //    score++;

            return (PasswordScore)score;
        }
    }
}
