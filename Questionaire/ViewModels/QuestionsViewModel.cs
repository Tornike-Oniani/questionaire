using Questionaire.Classes;
using Questionaire.Models;
using Questionaire.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Questionaire.ViewModels
{
    class QuestionsViewModel : INotifyPropertyChanged
    {
        // Private attributes
        private Question _selectedQuestion;
        private string _search;
        private string _students;

        // Public properties
        public List<Question> Questions { get; set; }
        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set { _selectedQuestion = value; OnPropertyChanged("SelectedQuestion"); }
        }
        public CollectionViewSource _questionsCollection { get; set; }
        public ICollectionView QuestionsCollection { get { return _questionsCollection.View; } }
        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                _questionsCollection.View.Refresh();
                OnPropertyChanged("Search");
            }
        }
        public string Students
        {
            get { return _students; }
            set { _students = value; OnPropertyChanged("Students"); }
        }


        // Commands
        public ICommand QuestionCheckCommand { get; set; }

        // Constructor
        public QuestionsViewModel()
        {
            Questions = new QuestionRepo().GetQuestions();
            _questionsCollection = new CollectionViewSource();
            _questionsCollection.Source = Questions;
            _questionsCollection.Filter += OnSearch;

            QuestionCheckCommand = new RelayCommand(QuestionCheck);
        }

        // Command actions
        public void QuestionCheck(object input = null)
        {
            List<string> selectedQuestions = Questions.FindAll(q => q.IsChecked).Select(q => q.Text).ToList();
            Students = new QuestionRepo().GetStudentsWhoHaveNoQuestions(selectedQuestions);
        }

        // Private helpers
        private void OnSearch(object sender, FilterEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                e.Accepted = true;
                return;
            }

            e.Accepted = false;

            Question current = e.Item as Question;
            if (current.Text.Contains(Search))
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
