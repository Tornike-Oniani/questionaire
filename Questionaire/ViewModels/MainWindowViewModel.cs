using Questionaire.Classes;
using Questionaire.Repositories;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Questionaire.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private List<string> _students;

        public List<QuestionBox> Questions { get; set; }
        public List<string> Students
        {
            get { return _students; }
            set { _students = value; OnPropertyChanged("Students"); }
        }


        public ICommand QuestionCheckChangedCommand { get; set; }

        public MainWindowViewModel()
        {
            Questions = new List<QuestionBox>();
            foreach (string question in new QuestionRepo().GetQuesitons())
                Questions.Add(new QuestionBox() { Question = question });

            QuestionCheckChangedCommand = new RelayCommand(QuestionCheckChanged);
        }

        public void QuestionCheckChanged(object input = null)
        {
            List<string> checkedQuestions = Questions.FindAll(q => q.IsChecked).Select(q => q.Question).ToList();
            Students = new StudentRepo().GetStudentThatHasntAnsweredQuestions(checkedQuestions);
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
