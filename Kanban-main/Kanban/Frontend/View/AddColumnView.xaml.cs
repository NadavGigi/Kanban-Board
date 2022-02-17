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
    /// Interaction logic for AddColumnView.xaml
    /// </summary>
    public partial class AddColumnView : Window
    {
        public AddColumnViewModel acvm;
        public AddColumnView(AddColumnViewModel acvm)
        {
            InitializeComponent();
            this.acvm = acvm;
            this.DataContext = acvm;
        }

        private void add_Column_Click(object sender, RoutedEventArgs e)
        {
            if (acvm.AddColumn())
            {
                this.Close();
            }
        }
    }
}
