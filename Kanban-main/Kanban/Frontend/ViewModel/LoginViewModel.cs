using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class LoginViewModel : NotifiableObject
    {
        public BackendController bc { get; private set; }
        private string username;
        private string password;
        private string message;
        public string Username {
            get {
                return username;
            }
            set {
                username = value;
                RaisePropertyChanged("Username");

            }
        }
        public string Password {
            get {
                return password;
            }
            set {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        public string Message {
            get {
                return message;
            }
            set {
                message = value;
                RaisePropertyChanged("Message");
            }
        }

        public LoginViewModel(BackendController bc)
        {
            this.bc = bc;
        }

        public UserModel Login()
        {
            Message = "";
            try
            {
                UserModel user = bc.Login(Username, Password);
                return user;
            }
            catch(Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
    }
}
