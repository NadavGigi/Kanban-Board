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
    /// Interaction logic for JoinBoardView.xaml
    /// </summary>
    public partial class JoinBoardView : Window
    {
        private JoinBoardViewModel jbvm;
        public JoinBoardView(UserModel user)
        {
            InitializeComponent();
            jbvm = new JoinBoardViewModel(user);
            this.DataContext = jbvm;
        }

        private void JoinBoard_Click(object sender, RoutedEventArgs e)
        {
            if (jbvm.JoinBoard())
            {
                this.Close();
            }
        }
    }
}
