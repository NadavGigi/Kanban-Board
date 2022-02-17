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
    /// Interaction logic for EditTaskTitleView.xaml
    /// </summary>
    public partial class EditTaskTitleView : Window
    {
        public EditTaskViewModel etvm;
        public EditTaskTitleView(EditTaskViewModel etvm)
        {
            InitializeComponent();
            this.etvm = etvm;
            this.DataContext = etvm;
        }

        private void UpdateTitle_Click(object sender, RoutedEventArgs e)
        {
            if (etvm.UpdateTitle())
            {

                this.Close();
            }
        }
    }
}
