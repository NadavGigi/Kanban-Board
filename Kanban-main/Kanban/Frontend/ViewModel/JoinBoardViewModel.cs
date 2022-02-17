using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class JoinBoardViewModel : NotifiableObject

    {
        public UserModel user;
        private string boardName;
        public string BoardName {
            get {
                return boardName;
            }
            set {
                boardName = value;
                RaisePropertyChanged("BoardName");
            }
        }
        private string creatorEmail;
        public string CreatorEmail {
            get {
                return creatorEmail;
            }
            set {
                creatorEmail = value;
                RaisePropertyChanged("CreatorEmail");
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
        public JoinBoardViewModel(UserModel user)
        {
            this.user = user;
        }
        public bool JoinBoard()
        {
            try
            {
                user.bc.JoinBoard(user.Email, CreatorEmail, BoardName);
                BoardModel newBoard = new BoardModel(user.bc.GetBoard(CreatorEmail, BoardName), user.bc);
                user.Boards.Add(newBoard);
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
