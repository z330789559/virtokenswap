using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string questionInfo;
        public List<string> options;
        public Question(string questionInfo, List<string> options)
        {
            this.questionInfo = questionInfo;
            this.options = options;
        }

        public Question()
        {

        }
    }
}
