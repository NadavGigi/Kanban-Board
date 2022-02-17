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
    /// Interaction logic for EditTaskView.xaml
    /// </summary>
    public partial class EditTaskView : Window
    {
        public EditTaskViewModel etvm;
        public EditTaskView(EditTaskViewModel etvm)
        {
            InitializeComponent();
            this.etvm = etvm;
            this.DataContext = etvm;
        }

        private void Update_DueDate_Click(object sender, RoutedEventArgs e)
        {
            EditTaskDueDateView EditDueDateWindow = new EditTaskDueDateView(etvm);
            EditDueDateWindow.ShowDialog();
        }

        private void Update_Description_Click(object sender, RoutedEventArgs e)
        {
            EditTaskDescriptionView EditDescriptionWindow = new EditTaskDescriptionView(etvm);
            EditDescriptionWindow.ShowDialog();
            
        }

        private void Update_Title_Click(object sender, RoutedEventArgs e)
        {
            EditTaskTitleView EditTitleWindow = new EditTaskTitleView(etvm);
            EditTitleWindow.ShowDialog();
        }

        private void Advance_Click(object sender, RoutedEventArgs e)
        {
            if (etvm.AdvanceTask())
            {
                Close();
            }
        }

        private void Assign_Click(object sender, RoutedEventArgs e)
        {
            AssignTaskView AssignTaskWindow = new AssignTaskView(etvm.AssignTask());
            this.Close();
            AssignTaskWindow.ShowDialog();
        }
    }
}
