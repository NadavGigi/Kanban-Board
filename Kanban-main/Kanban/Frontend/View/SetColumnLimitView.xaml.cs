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
    /// Interaction logic for SetColumnLimitView.xaml
    /// </summary>
    public partial class SetColumnLimitView : Window
    {
        public BoardViewModel bvm;
        public SetColumnLimitView(BoardViewModel bvm)
        {
            InitializeComponent();
            this.bvm = bvm;
            this.DataContext = bvm;
        }

        private void SetLimit_Click(object sender, RoutedEventArgs e)
        {
            if (bvm.SetLimit())
            {
                Close();
            }
        }
    }
}
