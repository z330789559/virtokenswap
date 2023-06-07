﻿using HH.Entities;
using HH.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using HH.Dao;
using HH.utils;
using System.Net;
using Org.BouncyCastle.Asn1.X509;

namespace HH.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class VirtualTokenController : ControllerBase
    {
        public readonly MarkowitzModel markowitzModel;
        //public readonly VirtualTokenDbContext virtualTokenDbContext;

        public VirtualTokenController(MarkowitzModel markowitzModel)
        {
            //this.virtualTokenDbContext = virtualTokenDbContext;
            this.markowitzModel = markowitzModel;
            
        }



        // GET: api/riskAnalysis/1
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<VirtualToken>>> GetVirtualTokenPrice()
        {
            VirtualTokenDbContext virtualTokenDbContext = markowitzModel.virtualTokenDbContext;
            WebCrawler webCrawler = new WebCrawler(virtualTokenDbContext);
            webCrawler.CrawlData();
            var virtualTokens = virtualTokenDbContext.VirtualTokens.ToList();
            if (virtualTokens.Count == null)
            {
                return Ok(ApiResponseFactory.CreateErrorResponse<VirtualToken>("未发现记录", (int)HttpStatusCode.NoContent));
            }
            //int virtualTokenId = Convert.ToInt32(id);
            return Ok(ApiResponseFactory.CreateSuccessResponse<List<VirtualToken>>(virtualTokens));
        }

        [HttpPost("{resid,virtualTokenId1,virtualTokenId2}")]
        public ActionResult<ApiResponse<Result>> GetCovarianceResult(int resId, int virtualTokenId1, int virtualTokenId2)
        {
            VirtualTokenDbContext virtualTokenDbContext = markowitzModel.virtualTokenDbContext;
            WebCrawler webCrawler = new WebCrawler(virtualTokenDbContext);
            
            var virtualToken1 = virtualTokenDbContext.VirtualTokens.SingleOrDefault(o => o.Id == virtualTokenId1);
            var virtualToken2 = virtualTokenDbContext.VirtualTokens.SingleOrDefault(o => o.Id == virtualTokenId2);
            if (virtualToken1 == null || virtualToken2 == null)
            {
                return Ok(ApiResponseFactory.CreateErrorResponse<VirtualToken>("未发现记录", (int)HttpStatusCode.NoContent));
            }
            
            Result result1 = markowitzModel.GetCovarianceMatrix(resId,virtualTokenId1, virtualTokenId2);
            result1.ShapeRatio = markowitzModel.GetBestWeightInRisk(resId,virtualTokenId1, virtualTokenId2);
            virtualTokenDbContext.Add(result1);
            //virtualTokenDbContext.Results.Add(result1);
            //virtualTokenDbContext.SaveChanges();
            //Result test = virtualTokenDbContext.Results.SingleOrDefault(o=>o.Id == virtualTokenId1 + " " + virtualTokenId2);
            return Ok(ApiResponseFactory.CreateSuccessResponse<Result>(result1));
        }

        /*
        [HttpPost("questionnaire")]
        public ActionResult GetRiskParam(string score)
        {
            int riskParam = Convert.ToInt32(score);
            if(riskParam == 0)
            {
                return BadRequest("无效用值");
            }
            markowitzModel.GetRiskWeight(riskParam);
            return NoContent();
        }
        
        [HttpGet]
        public ActionResult<double> GetRiskWeight()
        {

            if (markowitzModel.riskWeight == 0)
            {
                return BadRequest("风险组合权重为0");
            }
            return markowitzModel.riskWeight;
        }

        
        [HttpGet]
        public ActionResult<double> GetFreeRiskWeight()
        {

            if (markowitzModel.freeRiskWeight == 0)
            {
                return BadRequest("风险组合权重为0");
            }
            return markowitzModel.freeRiskWeight;
        }
        */

    }

}

