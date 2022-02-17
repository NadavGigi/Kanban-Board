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

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
    {
        RegisterViewModel rvm;
        public MainViewModel mvm;
        public RegisterView(MainViewModel mvm)
        {

            InitializeComponent();
            rvm = new RegisterViewModel(mvm.bc);
            this.mvm = mvm;
            this.DataContext = rvm;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (rvm.Register())
            {
                MessageBox.Show("Registered Successfully");
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
