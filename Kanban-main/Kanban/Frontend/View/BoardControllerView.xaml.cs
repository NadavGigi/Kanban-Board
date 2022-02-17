using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Presentation.Model;
using Presentation.ViewModel;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardControllerView : Window
    {
        private BoardControllerViewModel bcvm;
        public BoardControllerView(UserModel user,LoginViewModel lvm)
        {
            InitializeComponent();
            bcvm = new BoardControllerViewModel(user,lvm.bc);
            this.DataContext = bcvm;


        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (bcvm.Logout())
            {
                MainWindow MainWindow = new MainWindow();
                MainWindow.Show();
                this.Close();
            }

        }

        private void Add_Board_Click(object sender, RoutedEventArgs e)
        {
            AddBoardView AddBoardWindow = new AddBoardView(bcvm.User);
            AddBoardWindow.ShowDialog();
        }

        private void Remove_Board_Click(object sender, RoutedEventArgs e)
        {
            bcvm.RemoveBoard();
        }
        private void BoardList_DoubleClick(object sender, RoutedEventArgs e)
        {
            BoardView BoardWindow = new BoardView(this, bcvm.User.Email, bcvm.SelectedBoard);
            BoardWindow.Show();
            this.Hide();

        }

        private void JoinBoard_Click(object sender, RoutedEventArgs e)
        {
            JoinBoardView JoinBoardWindow = new JoinBoardView(bcvm.User);
            JoinBoardWindow.ShowDialog();
        }

    }
}
