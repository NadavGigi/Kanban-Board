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
    /// Interaction logic for AssignTaskView.xaml
    /// </summary>
    public partial class AssignTaskView : Window
    {
        public AssignTaskViewModel atvm;
        public AssignTaskView(AssignTaskViewModel atvm)
        {
            InitializeComponent();
            this.atvm = atvm;
            this.DataContext = atvm;
        }

        private void UpdateDes_Click(object sender, RoutedEventArgs e)
        {
            if (atvm.AssignTask())
            {

                this.Close();
            }
        }

        private void Assign_Click(object sender, RoutedEventArgs e)
        {
            if (atvm.AssignTask())
            {

                this.Close();
            }
        }
    }
}
