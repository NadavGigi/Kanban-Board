using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;

namespace Presentation.ViewModel
{
    public class AddBoardViewModel : NotifiableObject
      
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
        public AddBoardViewModel(UserModel user)
        {
            this.user = user;
        }
        public bool AddBoard()
        {
            try
            {
                user.bc.AddBoard(user, BoardName);
                BoardModel newBoard = new BoardModel(user.bc.GetBoard(user.Email, BoardName),user.bc);
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
