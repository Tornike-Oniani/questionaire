using Questionaire.Classes;
using Questionaire.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Questionaire.ViewModels
{
    class StudentEntryViewModel : INotifyPropertyChanged
    {
        // Private attributes
        private string _studentName;
        private bool _isStudentEntryFocused;

        // Public properties
        public string StudentName
        {
            get { return _studentName; }
            set { _studentName = value; OnPropertyChanged("StudentName"); }
        }
        public int TicketNumber { get; set; }
        public int ExamNumber { get; set; }
        public List<int> AvailableTickets { get; set; }
        public bool IsStudentEntryFocused
        {
            get { return _isStudentEntryFocused; }
            set { _isStudentEntryFocused = value; OnPropertyChanged("IsStudentEntryFocused"); }
        }


        // Commands
        public ICommand AddStudentCommand { get; set; }

        // Constructor
        public StudentEntryViewModel()
        {
            AvailableTickets = new List<int>() { 1, 2, 3, 4, 5, 6 };

            AddStudentCommand = new RelayCommand(AddStudent);
        }

        // Command actions
        public void AddStudent(object input = null)
        {
            new StudentRepo().AddStudentWithTicket(StudentName, TicketNumber);
            StudentName = null;
            IsStudentEntryFocused = false;
            IsStudentEntryFocused = true;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
