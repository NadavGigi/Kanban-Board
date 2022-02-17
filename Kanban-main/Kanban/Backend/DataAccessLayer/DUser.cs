using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DUser : DAL
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("DUser");
        public const string userEmail = "email";
        public const string userPassword = "password";
        public string email { get; set; }
        public string password { get; set; }
        public DUser(string email, string password) : base(new DALUserController())
        {
            this.email = email;
            this.password = password;
        }
    }
}