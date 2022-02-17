using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;


namespace IntroSE.Kanban.Backend.BuisnessLayer
{
    class UserController
    {
        private Validator validate = new Validator();
        private Dictionary<string, User> usersList = new Dictionary<string, User>();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("BusniessLayer.UserController");
        public void setUserList(User user)
        {
            usersList.Add(user.getEmail(), user);
        }
        public Dictionary<string, User> getUserList() { return usersList; }

        /// <summary>
        ///This method is checking if the email and the password are valids and creating a new User
        /// </summary>
        /// <param name="email"></param>The email that the user wants to register with.
        /// <param name="password"></param>the password that the User wants to use.
        /// <returns></returns> returns a new User.
        /// <exception cref="System.Exception">throw an exeption if the Email or the password are invalid</exception>
        public User Register(string email, string password)
        {
            bool emailChecker = validate.validateEmail(email);
            bool passChecker = validate.validatePassword(password);
            //check if the email is valid and if it is already used for another user
            if (passChecker & !(usersList.Keys.Contains(email)) & emailChecker)
            {
                email = email.ToLower();
                User newUser = new User(email, password);
                usersList.Add(email, newUser);
                log.Info("Creating an User");
                return newUser;
            }
            else
            {
                if (usersList.Keys.Contains(email))
                {
                    log.Warn("Email already exist in the system");
                    throw new Exception("Email already exist in the system");
                }
                else if (!passChecker)
                {
                    log.Warn("Invalid password");
                    throw new Exception("Invalid password");
                }
                else
                {
                    log.Warn("Invalid email");
                    throw new Exception("Invalid email");
                }
            }
        }
        /// <summary>
        ///The function is asking to insert email and a password, if the email and the password inserted are matching
        ///to some User in the data, the function called the same User login() function which 
        ///changes the User loggedIn status to true;
        ///</summary>
        /// <param name="email"></param> The email of the user.
        /// <param name="pass"></param> the password of the user
        /// <returns></returns> Return the user which have the same email and password.
        /// <exception cref="System.Exception">throw an exeption if the Email or the password are invalid</exception>

        public User login(string email, string pass)
        {
            email = email.ToLower();
            //check that the email inserted already exist in the user list
            if (usersList.ContainsKey(email))
            {
                //check that the password inserted is matching to the user password
                if (usersList[email].CheckMatchingPassword(pass))
                {
                    usersList[email].login();
                    return getUser(email);
                }
                else
                {
                    log.Warn("Email or password incorrect");
                    throw new Exception("Email or password incorrect");
                }
            }
            else
            {
                log.Warn("Email or password incorrect");
                throw new Exception("user do not exist");
            }
        }
        /// <summary>
        /// gets a User 
        /// </summary>
        /// <param name="email"></param>The email of the User we want
        /// <returns></returns> The User that have the same email
        public User getUser(string email)
        {
            email = email.ToLower();
            return usersList[email];
        }
        /// <summary>
        /// This method check the logged in status of a specific user.
        /// </summary>
        /// <param name="email"></param>The email of the user
        /// <returns></returns>true if the user is logged in or false if not.
        public bool validateLogin(string email)
        {
            email = email.ToLower();
            if (!usersList.ContainsKey(email))
            {
                log.Warn("user is not registered");
                throw new Exception("user is not registered");
            }
            if (getUser(email).getLoggedIn())
                return true;
            else
            {
                log.Warn("user is not logged-in");
                throw new Exception("user is not logged-in");
            }        
        }
        public void loadata()
        {
            usersList = new Dictionary<string, User>();
            if (usersList.Count == 0)
            {
                DataAccessLayer.DALUserController UC = new DataAccessLayer.DALUserController();
                List<DataAccessLayer.DUser> users = UC.ListOfUsers();
                foreach (var Duser in users)
                {
                    if (!usersList.ContainsKey(Duser.email))
                        usersList.Add(Duser.email, new User(Duser));
                }
            }

        }
        public void DeleteData()
        {
            DataAccessLayer.DALUserController UC = new DataAccessLayer.DALUserController();
            UC.DeleteAll();
            usersList = new Dictionary<string, User>();
        }
    }
}