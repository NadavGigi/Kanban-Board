using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class User
    {
        private string userEmail;
        private Validator validator = new Validator();
        private string userPassword;
        private List<string> passList = new List<string>();
        private bool loggedIn;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BusniessLayer.User");

        //Constructor of User
        public User(string email, string pass)
        {
            this.userEmail = email.ToLower();
            this.userPassword = pass;
            loggedIn = false;
            DataAccessLayer.DALUserController duc = new DataAccessLayer.DALUserController();
            duc.Insert(ToDalObject());
        }
        public User(DataAccessLayer.DUser duser)
        {
            this.userEmail = duser.email;
            this.userPassword = duser.password;
            loggedIn = false;
        }
        /// <summary>
        /// This method checks if the user is logged in
        /// </summary>
        /// <returns></returns> true if the user is logged in or flase if not.
        public bool getLoggedIn()
        {
            return loggedIn;
        }
        /// <summary>
        /// This method show the user Email.
        /// </summary>
        /// <returns></returns> the user email
        public string getEmail()
        {
            return userEmail;
        }
        /// <summary>
        /// This function change the current password of a user to a new one
        /// </summary>
        /// <param name="oldPass"></param>The current passwword of the user
        /// <param name="newPass"></param>The new password of the user.
        public void changePassword(string oldPass, string newPass)
        {
            //check if the user is loggedIn
            if (loggedIn == false)
            {
                log.Warn("User is not logged-in");
                throw new Exception("User is not logged-in");
            }
            //check if oldPass match to the current password 
            if (oldPass == userPassword)
                if (validator.validatePassword(newPass)) //check that the new pasword is valid
                    userPassword = newPass; //replace current pssword with the new password
                else
                {
                    log.Warn("Invalid password");
                    throw new Exception("Invalid password");
                }
            else
            {
                log.Warn("Wrong password inserted");
                throw new Exception("Wrong password inserted");
            }
            //saving the old password 
            DataAccessLayer.DALUserController duc = new DataAccessLayer.DALUserController();
            duc.Update(userEmail, oldPass, newPass);
            passList.Add(oldPass);
        }
        /// <summary>
        /// CheckMatchingPassword checks if the password it get as a parameter is matching with
        /// the current password of the user.
        /// </summary>
        /// <param name="pass"></param>The password that the user inserted.
        /// <returns></returns>returns true of it identical to the user password or false if not.
        public bool CheckMatchingPassword(string pass)
        {
            //check if the password that inserted is matching the current user password
            if (pass == userPassword)
                return true;
            else return false;
        }
        /// <summary>
        ///changes LoggedIn field of the user from false to true
        /// </summary>
        public void login()
        {
            //checks the loogeIn status
            if (loggedIn == true)
            {
                log.Warn("User is already loggedIn");
                throw new Exception("User is already loggedIn");
            }
            //changes the status of logged in from true to false
            else
            {
                loggedIn = true;
                log.Debug("User loggedIn");
            }
        }
        /// <summary>
        ///changes LoggedIn field of the user from true to false
        /// </summary>
        public void logout()
        {
            //changes the status of logged in from true to false
            loggedIn = false;
        }

        public DataAccessLayer.DUser ToDalObject() //Converts BusinessUser to 
        {
            DataAccessLayer.DUser toReturn = new DataAccessLayer.DUser(userEmail, userPassword);
            log.Debug("Converted dataUser to DAL Object succesfully");
            return toReturn;
        }
    }
}