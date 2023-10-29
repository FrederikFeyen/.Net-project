using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace models
{
    public class Werkskes : BasisKlasse
    {
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<string> Comments { get; set; }

        // Additional properties or methods if needed
        public string Countdown
        {
            get
            {
                TimeSpan Difference = DateTime.Now - CreatedDate;
                return Math.Floor(Difference.TotalDays).ToString();
            }
        }

        public DateTime CreatedDate { get; set; }

        public string Description { get; set; }

        public string DoneDate
        {
            get
            {
                if (FinishedDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return FinishedDate.ToString("dd/MM/yyyy");
                }
            }
        }

        public string UitstelDate
        {
            get
            {
                if (PostponedDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return PostponedDate.ToString("dd/MM/yyyy");
                }
            }
        }

        public DateTime FinishedDate { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime PostponedDate { get; set; }
        public string Reason { get; set; }

        public string StartDate
        {
            get
            {
                return CreatedDate.ToString("dd/MM/yyyy");
            }
        }

        public DateTime StartedDate { get; set; }
        public string Status { get; set; }

        // List to store comments
        public Werkskes()
        {
            Comments = new List<string>();
        }

        public Werkskes(int id, string name, string description, string status, DateTime createdDate, DateTime startedDate, DateTime finishedDate, DateTime postponedDate, List<string> comments)
        {
            Id = id;
            Name = name;
            Description = description;
            Status = status;
            CreatedDate = createdDate;
            StartedDate = startedDate;
            FinishedDate = finishedDate;
            PostponedDate = postponedDate;
            Comments = comments;
        }

        // Implement INotifyPropertyChanged if needed for data binding
        public event PropertyChangedEventHandler PropertyChanged;

        public override string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Name) && string.IsNullOrWhiteSpace(Name))
                {
                    return "Naam is een verplicht in te vullen veld!";
                }
                if (columnName == nameof(Name) && Name.Length < 3)
                {
                    return "Task name must be at least 3 characters long";
                }
                if (columnName == nameof(Name) && Name.Length > 50)
                {
                    return "Task name must be at most 50 characters long";
                }
                if (columnName == nameof(Description) && Description.Length > 120)
                {
                    return "Task description must be at most 120 characters long";
                }
                return "";
            }
        }
    }
}