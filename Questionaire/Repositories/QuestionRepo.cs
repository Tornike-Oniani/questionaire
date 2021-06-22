using Questionaire.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Windows;
using LiteDB;

namespace Questionaire.Repositories
{
    class QuestionRepo
    {
        private string connectionString;

        public QuestionRepo()
        {
            connectionString =Path.Combine(Environment.CurrentDirectory, "lite.db");
        }

        public void AddQuestion(string questionText)
        {
            using (LiteDatabase db = new LiteDatabase(connectionString))
            {
                ILiteCollection<Question> col = db.GetCollection<Question>("questions");

                Question question = new Question()
                {
                    Text = questionText
                };

                col.Insert(question);
            }
        }
        public List<string> GetQuesitons()
        {
            using (LiteDatabase db = new LiteDatabase(connectionString))
            {
                ILiteCollection<Question> col = db.GetCollection<Question>("questions");
                return col.Query().Select(q => q.Text).ToList();
            }
        }
    }
}
