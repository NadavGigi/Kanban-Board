using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using log4net;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class Validator
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BusniessLayer.Validator");

        //This function is checking if the password inserted is a valid password
        //according to the instruction of milestone1
        public bool validatePassword(string pass)
        {
            List<string> CommunPassword = new List<string>{ "123456","123456789","qwerty","password","1111111", "12345678",
            "abc123","1234567","password1","12345","1234567890","123123","000000","Iloveyou","1234","1q2w3e4r5t","Qwertyuiop",
            "123","Monkey","Dragon"};
            int minPassLength = 4;
            int maxPassLength = 20;
            int countUpper = 0;
            int countNum = 0;
            int countLower = 0;
            if (CommunPassword.Contains(pass))
            {
                return false;
            }
            //Check if the pass length is at least 4 and less than 21
            if (pass.Length < minPassLength | pass.Length > maxPassLength)
                return false;
            //check if pass contains at least one smal character, uppercase letter and a number
            foreach (char letter in pass)
            {
                if (char.IsUpper(letter))
                    countUpper++;
                if (char.IsLower(letter))
                    countLower++;
                if (char.IsDigit(letter))
                    countNum++;
            }
            if (countUpper >= 1 & countNum >= 1 & countLower >= 1)
                return true;
            else return false;
        }

        //This function is checking if the email inserted is a valid email
        public bool validateEmail(string email)
        {
            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            bool isValid = Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
            return isValid;
        }
        public bool validateTitle(string title)
        {
            int maxTitleLength = 50;
            //This function checks that the title is not empty or too long.
            if (title == null | title.Length > maxTitleLength | title == "")
            {
                log.Warn("Invalid title");
                throw new Exception("Invalid title");
            }
            return true;
        }
        public bool validateDes(string description)
        {
            int maxDesLength = 300;
            if (description == null)
                return true;
            //this function checks that the description is not empty.
            if (description.Length > maxDesLength)
            {
                log.Warn("The description is too long");
                throw new Exception("The description is too long");
            }
            return true;
        }
    }
}