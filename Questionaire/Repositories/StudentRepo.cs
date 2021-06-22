using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Questionaire.Models;

namespace Questionaire.Repositories
{
    class StudentRepo
    {
        private string connectionString = $"Data Source={Path.Combine(Environment.CurrentDirectory, "database.db;Version=3;")}";

        public void AddStudentWithTicket(string name, int number)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    // 1. Create student
                    conn.Execute("INSERT INTO Students (Name) VALUES (@Name);", new { Name = name }, transaction: transaction);

                    // 2. Retrieve created student and ticket ids
                    int studentId = conn.QuerySingleOrDefault<int>("SELECT Id FROM Students WHERE Name=@Name;", 
                        new { Name = name }, transaction: transaction);
                    int ticketId = conn.QuerySingleOrDefault<int>("SELECT Id FROM Tickets WHERE Number=@Number AND Exam=@Exam;",
                        new { Number = number, Exam = 1 }, transaction: transaction);
                    int ticketId2 = conn.QuerySingleOrDefault<int>("SELECT Id FROM Tickets WHERE Number=@Number AND Exam=@Exam;",
                        new { Number = number, Exam = 2 }, transaction: transaction);

                    // 3. Add student-ticket relationship
                    conn.Execute("INSERT INTO StudentTicket (StudentId, TicketId) VALUES (@StudentId, @TicketId)",
                        new { StudentId = studentId, TicketId = ticketId }, transaction: transaction);
                    conn.Execute("INSERT INTO StudentTicket (StudentId, TicketId) VALUES (@StudentId, @TicketId)",
                        new { StudentId = studentId, TicketId = ticketId2 }, transaction: transaction);
                    transaction.Commit();
                }
            }
        }
        public List<Student> GetStudentsWithQuestions()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"
SELECT s.Name, group_concat(q.Question, char(10)) AS 'Questions' FROM Students AS s
JOIN StudentTicket AS st ON s.Id = st.StudentId
JOIN Tickets AS t ON st.TicketId = t.Id
JOIN Questions AS q ON t.Id = q.TicketId
GROUP BY s.Name;
";
                return conn.Query<Student>(query).ToList();
            }
        }
    }
}
