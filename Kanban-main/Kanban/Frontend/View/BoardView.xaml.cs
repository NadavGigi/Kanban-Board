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
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        public BoardViewModel bvm;
        public BoardControllerView BoardControllerWindow;
        public BoardView(BoardControllerView BoardControllerWindow, string userEmail, BoardModel board)
        {
            InitializeComponent();
            bvm = new BoardViewModel(userEmail, board);
            this.DataContext = bvm;
            this.BoardControllerWindow = BoardControllerWindow;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            BoardControllerWindow.Show();
            this.Close();
        }

        private void Add_Column_Click(object sender, RoutedEventArgs e)
        {
            AddColumnView AddColumnWindow = new AddColumnView(bvm.AddColumnView());
            AddColumnWindow.ShowDialog();
        }

        private void Remove_Column_Click(object sender, RoutedEventArgs e)
        {
            bvm.RemoveColumn();
        }

        private void Add_Task_Click(object sender, RoutedEventArgs e)
        {
            AddTaskView AddTaskWindow = new AddTaskView(bvm.AddTaskView());
            AddTaskWindow.ShowDialog();
        }
        private void tasksList_DoubleClick(object sender, RoutedEventArgs e)
        {
            if( bvm.TaskView() != null)
            {
                EditTaskView EditTaskWindow = new EditTaskView(bvm.TaskView());
                EditTaskWindow.ShowDialog();
            }

        }

        private void Move_left_Click(object sender, RoutedEventArgs e)
        {
            bvm.MoveLeft();
        }

        private void Move_right_Click(object sender, RoutedEventArgs e)
        {
            bvm.MoveRight();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            SetFilterView SetFilterWindow = new SetFilterView(bvm);
            SetFilterWindow.ShowDialog();
        }

        private void Clear_Filter_Click(object sender, RoutedEventArgs e)
        {
            bvm.ClearFilter();
        }

        private void DueDateSort_Click(object sender, RoutedEventArgs e)
        {
            bvm.DueDateSort();
        }

        private void RenameCoulmn_Click(object sender, RoutedEventArgs e)
        {
            ColumnRenameView ColumnRenameWindow = new ColumnRenameView(bvm);
            ColumnRenameWindow.ShowDialog();
        }

        private void SetColumnLimit_Click(object sender, RoutedEventArgs e)
        {
            SetColumnLimitView SetLimitWindow = new SetColumnLimitView(bvm);
            SetLimitWindow.ShowDialog();
        }

        private void ShowInProgress_Click(object sender, RoutedEventArgs e)
        {
            bvm.SetIInProgress();
        }
    }
}
