using dal.Interfaces;
using dal.repositories;
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
    public partial class CommentDialog : Window
    {
        public string CommentText { get; private set; }

        public CommentDialog()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Set DialogResult to false to cancel the comment
            DialogResult = false;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the comment text from the TextBox
            CommentText = txtComment.Text;

            // Set DialogResult to true to confirm the comment
            DialogResult = true;
        }
    }
}