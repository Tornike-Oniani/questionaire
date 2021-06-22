using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questionaire.Models;
using LiteDB;

namespace Questionaire.Repositories
{
    class StudentRepo
    {
        private string connectionString;

        public StudentRepo()
        {
            connectionString = Path.Combine(Environment.CurrentDirectory, "lite.db");
        }

        public void AddStudentWithQuestions(string name, List<string> questions)
        {
            using (LiteDatabase db = new LiteDatabase(connectionString))
            {
                ILiteCollection<Student> col = db.GetCollection<Student>("students");

                Student student = new Student()
                {
                    Name = name,
                    Questions = questions
                };

                col.Insert(student);
            }
        }
        public List<string> GetStudentThatHasntAnsweredQuestions(List<string> questions)
        {
            using (LiteDatabase db = new LiteDatabase(connectionString))
            {
                ILiteCollection<Student> col = db.GetCollection<Student>("students");

                //return col.Query().Where(s => !s.Questions.Any(q => questions.Contains(q))).Select(st => st.Name).ToList();
                List<Student> allStudents = col.Query().ToList();
                List<Student> resultStudents = allStudents.Where(s => !s.Questions.Any(q => questions.Contains(q))).ToList();
                return resultStudents.Select(s => s.Name).ToList();
            }
        }
    }
}
