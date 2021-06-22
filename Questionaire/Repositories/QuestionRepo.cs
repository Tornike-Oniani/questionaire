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

namespace Questionaire.Repositories
{
    class QuestionRepo
    {
        private string connectionString = $"Data Source={Path.Combine(Environment.CurrentDirectory, "database.db;Version=3;")}";

        public List<Question> GetQuestions()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"
SELECT q.Question AS 'Text', group_concat(b.Name, char(10)) AS 'Students' FROM
(SELECT s.Name, t.Id FROM Students AS s
JOIN Tickets AS t
EXCEPT
SELECT s.Name, t.Id FROM Students AS s
JOIN StudentTicket AS st ON s.Id == st.StudentId
JOIN Tickets AS t ON st.TicketId == t.Id) AS b
JOIN Questions AS q ON b.Id = q.TicketId
GROUP BY q.Question;
";
                return conn.Query<Question>(query).ToList();
            }
        }
        public string GetStudentsWhoHaveNoQuestions(List<string> questions)
        {
            if (questions.Count == 0) { return ""; }

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"
SELECT group_concat(f.Name, char(10)) AS 'Students' FROM
(SELECT b.Name, group_concat(q.Question, ', ') AS 'Questions' FROM
(SELECT s.Name, t.Id FROM Students AS s
JOIN Tickets AS t
EXCEPT
SELECT s.Name, st.TicketId FROM Students AS s
JOIN StudentTicket AS st ON s.Id = st.StudentId) AS b
JOIN Questions AS q ON b.Id = q.TicketId
GROUP BY b.Name) AS f
WHERE 
";
                for (int i = 0; i < questions.Count; i++)
                {
                    query += $"f.Questions LIKE '%{questions[i]}%'";

                    if (i == questions.Count - 1)
                        query += ";";
                    else
                        query += " AND ";
                }

                return conn.QuerySingleOrDefault<string>(query);
            }
        }
    }
}
