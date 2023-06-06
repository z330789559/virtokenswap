using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH.Entities
{
    [Table("Result")]
    public class Result
    {
        public int Id { get; set; }
        public int VirtualToken1 { get; set; }
        public int VirtualToken2 { get; set; }
        public double[,] covMatrix;
        public double BestReturn { get; set; }
        public double BestVariance { get; set; }
        public double Weight1 { get; set; }
        public double Weight2 { get; set; }
        public double ShapeRatio { get; set; }
        public double RiskWeight { get; set; }
        public double FreeRiskWeight { get; set; }

        public Result(int id, int virtualToken1, int virtualToken2)
        {
            this.Id = id;
            this.VirtualToken1 = virtualToken1;
            this.VirtualToken2 = virtualToken2;
            this.covMatrix = new double[2,2];
            this.BestReturn = 0;
            this.BestVariance = 0;
            this.Weight1 = 0;
            this.Weight2 = 0;
            this.ShapeRatio = 0;
            this.RiskWeight = 0;
            this.FreeRiskWeight = 0;
        }

        public Result() { }
    }
}
