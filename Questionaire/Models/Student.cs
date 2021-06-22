using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionaire.Models
{
    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Questions { get; set; }
    }
}
