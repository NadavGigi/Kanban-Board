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
    /// Interaction logic for AddBoardView.xaml
    /// </summary>
    public partial class AddBoardView : Window
    {
        private AddBoardViewModel abvm;
        public AddBoardView(UserModel user)
        {
            InitializeComponent();
            abvm = new AddBoardViewModel(user);
            this.DataContext = abvm;
        }

        private void AddBoard_Click(object sender, RoutedEventArgs e)
        {
            if (abvm.AddBoard())
            {
                this.Close();
            }
        }
    }
}
