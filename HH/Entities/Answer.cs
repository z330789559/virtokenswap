using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public   int QuestionId { get; set; }
       
        public List<Score> optionsScore;
        public Answer() { }
    }
}
