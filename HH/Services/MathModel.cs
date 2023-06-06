using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HH.Dao;
using HH.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace HH.Services
{
    public class MathModel
    {
        public VirtualTokenDbContext virtualTokenDbContext;


        public MathModel(VirtualTokenDbContext virtualTokenDbContext)
        {

            this.virtualTokenDbContext = virtualTokenDbContext;
        }

        //获取协方差
        private double GetCovariance(VirtualToken token1, VirtualToken token2)
        {
            double res = 0;
            List<double> dayReturns1 = token1.DayReturn.Split(',').Select(double.Parse).ToList();
            List<double> dayReturns2 = token2.DayReturn.Split(',').Select(double.Parse).ToList();

            if (dayReturns1.Count != dayReturns2.Count)
            {
                throw new Exception(token1.Name + "的样本数量跟" + token2.Name + "不一样");
            }
            int sampleNum = dayReturns1.Count;
            for (int i = 0; i < sampleNum; i++)
            {
                res += (dayReturns1[i] - token1.DayAverageReturn) * (dayReturns2[i] - token2.DayAverageReturn);
            }
            return res / (sampleNum - 1) * 252 * 252;
        }

        //获取协方差矩阵
        public Result GetCovarianceMatrix(int id, int id1, int id2)
        {
            Result result1 = new Result(0,id1,id2);
            double[,] covarianceMatrix = result1.covMatrix;
            List<int> strings = new List<int>() { id1, id2 };
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    covarianceMatrix[i, j] = GetCovariance(virtualTokenDbContext.VirtualTokens.SingleOrDefault(o => o.Id == strings[i]), virtualTokenDbContext.VirtualTokens.SingleOrDefault(m => m.Id == strings[j]));
                    if (i != j)
                    {
                        covarianceMatrix[j, i] = covarianceMatrix[i, j];
                    }
                }
            }

            result1.covMatrix =  covarianceMatrix;
            //virtualTokenDbContext.Entry(result1).State = EntityState.Added;
            //virtualTokenDbContext.Set<Result>().Add(result1);
            virtualTokenDbContext.Results.Add(result1);
            virtualTokenDbContext.SaveChanges();
            return result1;
        }

        /*
        public void DisplayCovarianceMatrix(double[,] covMartix)
        {
            Console.WriteLine("协方差矩阵如下所示：");
            for (int i = 0; i < covMartix.GetLength(0); i++)
            {
                for (int j = 0; j < covMartix.GetLength(1); j++)
                {
                    Console.Write(covMartix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        */
    }
}
