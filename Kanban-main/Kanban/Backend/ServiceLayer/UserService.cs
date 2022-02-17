using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using IntroSE.Kanban.Backend.BuisnessLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("UserService");

        public UserService() { }
        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        public Response<SUser> Login(string email, string password, UserController uc)
        {
            try
            {
                uc.login(email, password);
            }
            catch (Exception e)
            {
                return Response<SUser>.FromError(e.Message);
            }
            SUser serUser = new SUser(email);
            log.Info("Logged in succesfully");
            return Response<SUser>.FromValue(serUser);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, UserController uc)
        {
            try
            {
                uc.Register(email, password);
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            log.Info("Registered successfully");
            return new Response();

        }
        /// <summary>        
        /// Log out an logged-in user. 
        /// </summary>
        /// <param name="userEmail">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response logout(string email, UserController uc)
        {
            try
            {
                uc.getUser(email).logout();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
            log.Info("Logged out successfully");
            return new Response();
        }
        ///<summary>This method loads the data from the persistance.
        ///         You should call this function when the program starts. </summary>
        public Response loadData(UserController uc, BoardController bc)
        {
            try
            {
                uc.loadata();
                bc.LoadData();
                return new Response();
            }
            catch (Exception e)
            {
                log.Warn("load Data failed");
                return new Response(e.Message);
            }
        }
        ///<summary>Removes all persistent data.</summary>
        public Response DeleteData(UserController uc, BoardController bc)
        {
            try
            {
                uc.DeleteData();
                bc.DeleteData();
                return new Response();
            }
            catch (Exception e)
            {
                log.Warn("deleting data Data failed");
                return new Response(e.Message);
            }
        }

        public Response<bool> validateLogin(string email, UserController uc)
        {
            try
            {
                return Response<bool>.FromValue(uc.validateLogin(email));
            }
            catch (Exception e)
            {
                return Response<bool>.FromError(e.Message);
            }
        }
    }
}