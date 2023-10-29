using dal.Interfaces;
using dal.repositories;
using models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public void RefreshTaskList()
        {
            List<Werkskes> updatedTaskList = TaskRepo.GetTasks();
            taskListView.ItemsSource = updatedTaskList;
        }

        private void Button_AddTask(object sender, RoutedEventArgs e)
        {
            string taskName = TaskName.Text;
            string taskDescription = TaskDescription.Text;
            Werkskes NieuwWerkske = new Werkskes() { Name = taskName, Description = taskDescription };
            var valid = NieuwWerkske.IsGeldig();
            if (valid)
            {
                bool succes = TaskRepo.InsertTask(NieuwWerkske);

                if (succes)
                {
                    MessageBox.Show("Task created!");

                    // Clear the input fields
                    TaskName.Text = "";
                    TaskDescription.Text = "";
                    List<Werkskes> updatedTaskList = TaskRepo.GetTasks();
                    taskListView.ItemsSource = updatedTaskList;
                }
                else
                {
                    MessageBox.Show("Task creation failed!");
                }
            }
            else
            {
                MessageBox.Show(NieuwWerkske.Error);
            }
        }

        private void TaskListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Werkskes SelectedWerkske = (Werkskes)((ListView)sender).SelectedItem;
            DetailsWindow detailsWindow = new DetailsWindow(SelectedWerkske, SelectedWerkske.Id);
            detailsWindow.Show();
        }

        private void TaskName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }

        private void Button_Filter(object sender, RoutedEventArgs e)
        {
            if (Filter_Grid.Visibility == Visibility.Visible)
            {
                Filter_Grid.Visibility = Visibility.Collapsed;
                RefreshTaskList();
                FilterName.Text = "";
                Status_Combobox.SelectedIndex = -1;
            }
            else
            {
                Filter_Grid.Visibility = Visibility.Visible;
            }
        }

        private void NameTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            _filters.Name = FilterName.Text;
            GoFilter();
        }

        private void GoFilter()
        {
            List<Werkskes> list = TaskRepo.Filter(_filters);
            taskListView.ItemsSource = list;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Status_Combobox.SelectedIndex == -1)
            {
                return;
            }
            var tag = ((ComboBoxItem)Status_Combobox.SelectedItem).Content;
            _filters.Status = "" + tag;
            GoFilter();
        }

        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CreatedDate_Filter(object sender, SelectionChangedEventArgs e)
        {
            string value = ((DatePicker)sender).SelectedDate.Value.ToString("yyyy-MM-dd");
            _filters.CreatedDt = value;
            GoFilter();
        }

        private void FinishedDate_Filter(object sender, SelectionChangedEventArgs e)
        {
            string value = ((DatePicker)sender).SelectedDate.Value.ToString("yyyy-MM-dd");
            _filters.FinishedDt = value;
            GoFilter();
        }

        private void TaskDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private readonly TaskFilters _filters = new TaskFilters();
        private readonly ITaskRepo TaskRepo = new TaskRepo();

        public MainWindow()
        {
            InitializeComponent();
            List<Werkskes> Werkskes = TaskRepo.GetTasks();
            taskListView.ItemsSource = Werkskes;
        }
    }
}