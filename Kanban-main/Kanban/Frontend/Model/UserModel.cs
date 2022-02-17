using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class UserModel : NotifiableModelObject
    {
        private ObservableCollection<BoardModel> boards;
        public ObservableCollection<BoardModel> Boards {
            get {
                return boards;
            }
            set {
                boards = value;
                RaisePropertyChanged("Boards");
            }
        }
        private string email;
        public string Email {
            get {
                return email;
            }
            set {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        public UserModel(BackendController bc, string email) : base(bc)
        {
            this.Boards = bc.GetAllBoards(email);
            this.Email = email;
        }

    }
}
