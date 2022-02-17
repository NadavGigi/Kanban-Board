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
    /// Interaction logic for AddTaskView.xaml
    /// </summary>
    public partial class AddTaskView : Window
    {
        public AddTaskViewModel atvm;
        public AddTaskView(AddTaskViewModel atvm)
        {
            InitializeComponent();
            this.atvm = atvm;
            this.DataContext = atvm;

        }

        private void Add_task_Click(object sender, RoutedEventArgs e)
        {
            if (atvm.AddTask())
            {
                this.Close();
            }
        }
    }
}
