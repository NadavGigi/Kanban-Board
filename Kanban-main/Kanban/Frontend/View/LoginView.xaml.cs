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
using Presentation.ViewModel;
using Presentation.Model;


namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private LoginViewModel lvm;
        public LoginView(MainViewModel mvm)
        {
            InitializeComponent();
            lvm = new LoginViewModel(mvm.bc);
            this.DataContext = lvm;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = lvm.Login();
            if (user != null)
            {
                BoardControllerView boardListWindow = new BoardControllerView(user,lvm);
                this.Close();
                boardListWindow.Show();
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
    }
}
