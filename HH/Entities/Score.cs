using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HH.Entities
{
	public class Score
    {

        public int Id { get; set; }
		 public int Level { get; set; }

        [ForeignKey("AnswerId")]
        public int AnswerId { get; set; }
		public string Symbol { get; set; }
        public Score()
		{
		}
	}
}

