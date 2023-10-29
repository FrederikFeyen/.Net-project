using dal.Interfaces;
using dal.repositories;
using models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace wpf
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        private void Button_FinishTask(object sender, RoutedEventArgs e)
        {
            UpdateStatus("Finished", true);
        }

        private void Button_StartTask(object sender, RoutedEventArgs e)
        {
            UpdateStatus("In Progress", false);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int taskIdToUpdate = _SelectedID;

            UpdateScreen updateTaskWindow = new UpdateScreen(taskIdToUpdate);
            updateTaskWindow.ShowDialog(); // Show the "UpdateTask" window as a modal dialog
        }

        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            CommentDialog commentDialog = new CommentDialog();
            if (commentDialog.ShowDialog() == true)
            {
                string commentText = commentDialog.CommentText;

                // Insert the comment into the database
                Comment newComment = new Comment
                {
                    Text = commentText,
                    CreatedAt = DateTime.Now,
                    CommentReason = "Postponed",
                };

                IDetailRepo detailRepo = new DetailRepo();
                bool commentInserted = detailRepo.InsertComment(newComment, _SelectedID);

                if (commentInserted)
                {
                    // Refresh the PropsForDetails list and update the listView
                    List<PropsForDetails> updatedProps = new List<PropsForDetails>();

                    // Add existing properties...
                    // For example, you can get the existing properties from your current `listView.ItemsSource`
                    if (listView.ItemsSource is List<PropsForDetails> currentProps)
                    {
                        updatedProps.AddRange(currentProps);
                    }

                    // Add the new comment property
                    updatedProps.Add(new PropsForDetails
                    {
                        PropName = "Comments",
                        PropValue = commentText
                    });

                    // Update the listView
                    listView.ItemsSource = updatedProps;
                }
                else
                {
                    MessageBox.Show("Failed to insert the comment.");
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker?", "Bevestiging verwijderen", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var task = TaskRepo.GetTask(_SelectedID);
                if (task.Status == "In Progress")
                {
                    MessageBox.Show("Task is in progress, cannot be deleted");
                    return;
                }
                // Verwijder task
                if (TaskRepo.DeleteTask(_SelectedID))
                {
                    MessageBox.Show("Task verwijderd!");
                    // Call the RefreshTaskList method of MainWindow
                    RefreshAndClose();
                }
                else
                {
                    MessageBox.Show("Task verwijderen mislukt!");
                }
            }
        }

        private void RefreshAndClose()
        {
            (Application.Current.MainWindow as MainWindow)?.RefreshTaskList();
            Close();
        }

        private void PostponeButton_Click(object sender, RoutedEventArgs e)
        {
            commentPopup.IsOpen = true;
        }

        private void UpdateStatus(string status, bool AddFinishDate, string reason = "")
        {
            var task = TaskRepo.GetTask(_SelectedID);
            if (task.Status == "Finished")
            {
                MessageBox.Show("Task is done, cannot be modified");
                return;
            }
            if (!TaskRepo.UpdateStatus(_SelectedID, status, AddFinishDate, reason))
            {
                MessageBox.Show($"Status update {status} mislukt!");
            }
            else
            {
                MessageBox.Show($"Status update {status} gelukt!");
                RefreshAndClose();
            }
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetPostponed(object sender, RoutedEventArgs e)
        {
            if (commentTextBox.Text == "")
            {
                MessageBox.Show("Gelieve een reden in te vullen");
                return;
            }
            UpdateStatus("Postponed", false, commentTextBox.Text);
        }

        private int _SelectedID;

        private ITaskRepo TaskRepo = new TaskRepo();

        public DetailsWindow(Werkskes werkske, int taskId)
        {
            InitializeComponent();

            if (werkske != null)
            {
                IDetailRepo detailRepo = new DetailRepo();

                _SelectedID = werkske.Id;

                List<Comment> comments = detailRepo.GetCommentsForTask(_SelectedID);
                List<PropsForDetails> props = new List<PropsForDetails>
                {
                    new PropsForDetails() { PropName = "Id", PropValue = werkske.Id.ToString() },
                    new PropsForDetails() { PropName = "Name", PropValue = werkske.Name },
                    new PropsForDetails() { PropName = "Description", PropValue = werkske.Description },
                    new PropsForDetails() { PropName = "Status", PropValue = werkske.Status },
                    new PropsForDetails() { PropName = "Reason", PropValue = werkske.Reason },
                    new PropsForDetails() { PropName = "Date created", PropValue = werkske.StartDate },
                    new PropsForDetails() { PropName = "Date finished", PropValue = werkske.DoneDate },
                    new PropsForDetails() { PropName = "Date postponed", PropValue = werkske.UitstelDate }
                };

                // alle te tonen properties toevoegen aan props
                _SelectedID = taskId;

                foreach (var comment in comments)
                {
                    props.Add(new PropsForDetails()
                    {
                        PropName = "Comment",
                        PropValue = comment?.Text ?? ""
                    });
                }
                listView.ItemsSource = props;
            }
        }

        private class PropsForDetails
        {
            public string? PropName { get; set; }
            public string? PropValue { get; set; }
        }
    }
}