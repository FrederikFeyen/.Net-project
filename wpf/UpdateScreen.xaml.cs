using dal.Interfaces;
using dal.repositories;
using models;
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

namespace wpf
{
    /// <summary>
    /// Interaction logic for UpdateScreen.xaml
    /// </summary>
    public partial class UpdateScreen : Window
    {
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve user input from TextBox controls

            string newName = txtNewTaskName.Text;
            string newDescription = txtNewTaskDescription.Text;

            // Create a new instance of the task to be updated
            Werkskes updatedTask = new Werkskes
            {
                Id = TaskIdToUpdate, // Set the ID of the task to update
                Name = newName,
                Description = newDescription
            };

            if (!updatedTask.IsGeldig())
            {
                MessageBox.Show(updatedTask.Error);
                return;
            }

            // Call the UpdateTask method to update the task
            bool updateResult = DetailRepo.UpdateTask(updatedTask);

            if (updateResult)
            {
                MessageBox.Show("Task updated successfully.");
                RefreshAndClose();
            }
            else
            {
                MessageBox.Show("Failed to update task.");
            }
        }

        private void RefreshAndClose()
        {
            (Application.Current.MainWindow as MainWindow)?.RefreshTaskList();
            Close();
        }

        private readonly ITaskRepo taskRepo = new TaskRepo();
        private IDetailRepo DetailRepo = new DetailRepo();
        private int TaskIdToUpdate;

        public UpdateScreen(int taskId)
        {
            InitializeComponent();
            // Store the task ID in a private field for later use
            TaskIdToUpdate = taskId;

            var task = taskRepo.GetTask(taskId);
            txtNewTaskName.Text = task.Name;
            txtNewTaskDescription.Text = task.Description;
        }
    }
}