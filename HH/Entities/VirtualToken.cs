using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Entities
{
    [Table("VirtualToken")]
    public class VirtualToken
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string DayReturn { get; set; }
        public string Price   { get; set; }

        public double DayAverageReturn
        {

            get
            {
                List<double> dayReturn = new List<double>();
                if (DayReturn != "" && DayReturn!=null)
                {
                    dayReturn = DayReturn.Split(',').Select(double.Parse).ToList();
                }
                
                return dayReturn.Sum() / dayReturn.Count;
            }
        }

        public double YearReturn
        {
            get => DayAverageReturn * 252;
        }

        public double Variance
        {
            get
            {
                double sum = 0d;
                List<double> dayReturn = DayReturn.Split(',').Select(double.Parse).ToList();
                for (int i = 0; i < dayReturn.Count; i++)
                {
                    double temp = dayReturn[i];
                    sum += (temp - DayAverageReturn) * (temp - DayAverageReturn);
                }
                return sum / (dayReturn.Count - 1) * 252 * 252;
            }
        }

        public VirtualToken( string name, List<double> price)
        {
      
            Name = name;
            this.Price = string.Join(",", price.ConvertAll(s => Convert.ToString(s)));
            this.DayReturn = string.Join(",", new List<double>(price.Count - 1).ConvertAll(s => Convert.ToString(s)));
            GetDayReturn();
        }

        public VirtualToken() { }

        public void GetDayReturn()
        {
            if (Price=="")
            {
                throw new Exception("价格读入出现错误");
            }
            List<double> price = Price.Split(',').Select(double.Parse).ToList();
            List<double> dayReturn =new List<double>();
            for (int i = 0; i < price.Count - 1; i++)
            {
                dayReturn.Add(Math.Log(price[i + 1]) - Math.Log(price[i]));
            }
            this.DayReturn = string.Join(",", dayReturn.ConvertAll(s => Convert.ToString(s)));
        }


    }
}
