using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Entities
{
    public class Questionnaire
    {
        public int Id { get; set; }
        public List<Question> questions;
        public List<Answer> answers;
        public List<Chosen> chosenOption;
        public int riskParam;

        public Questionnaire(int id, List<Question> questions, List<Answer> answers)
        {
            Id = id;
            this.questions = questions;
            this.answers = answers;
            chosenOption = new List<Chosen>();
            riskParam = 0;
        }

        public Questionnaire() { }

        private void displayOptions()
        {
            Console.WriteLine("你选的选项如下所示");
            for (int i = 0; i < chosenOption.Count; i++)
            {
                Console.Write(chosenOption[i].Content + "  ");
            }
            Console.WriteLine();
        }
        public void displayQuestion()
        {
            for (int i = 0; i < questions.Count; i++)
            {
                Console.WriteLine(questions[i].questionInfo);
                for (int j = 0; j < questions[i].options.Count; j++)
                { 
                    Console.Write(questions[i].options[j] + "  ");
                }
                Console.WriteLine();
                string? ans = Console.ReadLine();
                if (ans != null)
                {
                    Chosen c = new Chosen();
                        c.Content = ans;
                    chosenOption.Add(c);
                }
            }
            displayOptions();
        }

        public int GetScore()
        {
            int score = 0;
            for (int i = 0; i < chosenOption.Count; i++)
            {
                switch (chosenOption[i].Symbol)
                {
                    case "A":
                    case "a":
                        score += answers[i].optionsScore[0].Level; break;
                    case "B":
                    case "b":
                        score += answers[i].optionsScore[1].Level; break;
                    case "C":
                    case "c":
                        score += answers[i].optionsScore[2].Level; break;
                    case "D":
                    case "d":
                        score += answers[i].optionsScore[3].Level; break;
                    default: throw new Exception("问卷调查答案出现不符合的选项");
                }
            }
            riskParam = score;
            return score;
        }

    }
}
