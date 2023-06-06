using HH.Dao;
using HH.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace HH.Services
{
    public class MarkowitzModel : MathModel
    {
    

        public MarkowitzModel(VirtualTokenDbContext virtualTokenDbContext) : base(virtualTokenDbContext)
        {
          

        
          }

        //两个币种的比较
        //返回夏普比率
        public double GetBestWeightInRisk(int resId, int tokenId1, int tokenId2)
        {
            double freeReturn = 0.03;
            var virtualToken1 = virtualTokenDbContext.VirtualTokens.SingleOrDefault(o => o.Id == tokenId1);
            var virtualToken2 = virtualTokenDbContext.VirtualTokens.SingleOrDefault(o => o.Id == tokenId2);
            double returnOne = virtualToken1.YearReturn - freeReturn;
            double returnTwo = virtualToken2.YearReturn - freeReturn;
            double covValue = 0;
            Result result1 = this.virtualTokenDbContext.Results.SingleOrDefault(o => o.Id == resId);
           
            double[,] covarianceMatrix = new double[0, 0];
            if (result1 != null)
            {
                covarianceMatrix= result1.covMatrix;
            }
           

            if (result1 == null)
            {
                result1 = new Result();
            }
            if (covarianceMatrix.GetLength(0) == 2)
            {
                covValue = covarianceMatrix[0, 1];
            }
            result1.Weight1 = (returnOne * virtualToken2.Variance - returnTwo * covValue) / (returnOne * virtualToken2.Variance + returnTwo * virtualToken1.Variance - (returnOne + returnTwo) * covValue);
            result1.Weight2 = 1 - result1.Weight1;
            double weight1 = result1.Weight1;
            double weight2 = result1.Weight2;
            result1.BestReturn = virtualToken1.YearReturn * weight1 + virtualToken2.YearReturn * weight2;
            result1.BestVariance = weight1 * weight1 * virtualToken1.Variance + weight2 * weight2 * virtualToken2.Variance + 2 * weight1 * weight2 * covValue;
            double bestVariance = result1.BestVariance;
            double bestReturn = result1.BestReturn;
            double standardDeviation = Math.Sqrt(bestVariance);
            //最佳夏普比率
            return (bestReturn - freeReturn) / standardDeviation;
        }


        //计算风险组合权重和无风险组合权重
        public Result GetRiskWeight(int resId, int id1, int id2,int riskParam)
        {
            //double riskWeight = 0;
            //double freeRiskWeight = 0;
            
            //Questionnaire queryQuetionnaire = virtualTokenDbContext.Questionnaires.SingleOrDefault(x => x.Id == questionnaireId);
            
            Result result1 = this.virtualTokenDbContext.Results.SingleOrDefault(o => o.Id == resId);
            double bestReturn = result1.BestReturn;
            double bestVariance = result1.BestVariance;
            result1.RiskWeight = (bestReturn - 0.03) / (riskParam * bestVariance);
            result1.FreeRiskWeight = 1 - result1.RiskWeight;
            return result1;
        }


        
    }
}
