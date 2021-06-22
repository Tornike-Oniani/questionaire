using Questionaire.Models;
using Questionaire.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Questionaire.ViewModels
{
    class StudentsViewModel : INotifyPropertyChanged
    {
        // Private attributes
        private Student _selectedStudent;
        private string _search;

        // Public properties
        public List<Student> Students { get; set; }
        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set { _selectedStudent = value; OnPropertyChanged("SelectedStudent"); }
        }
        public CollectionViewSource _studentsCollection { get; set; }
        public ICollectionView StudentsCollection { get { return _studentsCollection.View; } }
        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                _studentsCollection.View.Refresh();
                OnPropertyChanged("Search");
            }
        }

        // Constructor
        public StudentsViewModel()
        {
            Students = new StudentRepo().GetStudentsWithQuestions();
            _studentsCollection = new CollectionViewSource();
            _studentsCollection.Source = Students;
            _studentsCollection.Filter += OnSearch;
        }

        private void OnSearch(object sender, FilterEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                e.Accepted = true;
                return;
            }

            e.Accepted = false;

            Student current = e.Item as Student;
            if (current.Name.Contains(Search))
            {
                e.Accepted = true;
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
