using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    class BoardControllerViewModel : NotifiableObject
    {
        public BackendController bc { get; private set; }
        public string Title { get; private set; }
        private UserModel user;
        public UserModel User {
            get {
                return user;
            }
            set {
                user = value;
                RaisePropertyChanged("User");
            }
        }
        private string message;
        public string Message {
            get {
                return message;
            }
            set {
                message = value;
                RaisePropertyChanged("Message");
            }
        }
        private BoardModel selectedBoard;
        public BoardModel SelectedBoard {
            get {
                return selectedBoard;
            }
            set {
                selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
            }
        }
        private bool _enableForward = false;
        public bool EnableForward {
            get => _enableForward;
            private set {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }
        public BoardControllerViewModel(UserModel user, BackendController bc)
        {
            this.bc = bc;
            this.User = user;
            Title = "Welcome to your account " + User.Email;

        }
        public bool RemoveBoard()
        {
            try
            {
                bc.RemoveBoard(user.Email, SelectedBoard.CreatorEmail, SelectedBoard.BoardName);
                user.Boards.Remove(SelectedBoard);
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }

        }
        public bool Logout()
        {
            try
            {
                bc.Logout(user.Email);
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
                return false;
            }
        }
    }
}
