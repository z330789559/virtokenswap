using System;
using System.IO;
using System.Text;
using HtmlAgilityPack;
using HH.Dao;
using HH.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace HH.Services
{
    public class WebCrawler
    {
        private readonly VirtualTokenDbContext _dbContext;

        //public List<VirtualToken> virtualTokens;

        public WebCrawler( VirtualTokenDbContext _dbContext)
        {
          
            this._dbContext = _dbContext;
            //this.virtualTokens = virtualTokens;
        }

        public void CrawlData()
        {
            var filePath = @"/Users/libaozhong/Desktop/zuoye/r/HH(vt version1)/HH/bin/Debug/net7.0/huobi.html";
            var html = File.ReadAllText(filePath, Encoding.UTF8);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var table = htmlDocument.DocumentNode.SelectSingleNode("//table");
            var rows = table.SelectNodes("//tr");

            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var cells = row.SelectNodes("td");
                    if (cells?.Count > 0)
                    {
                        var currency = cells[0].InnerText;
                        List<double> prices = new List<double>();
                        for (int i = 1; i < 31; i++)
                        {
                            prices.Add(double.Parse(cells[i].InnerText));
                        }
                        
                        // Create VirtualToken object
                        int id;
                        switch(currency)
                        {
                            case "比特币":
                                id = 1;break;
                            case "以太坊":
                                id = 2; break;
                            case "泰达币":
                                id = 4; break;
                            case "波场币":
                                id = 5; break;
                            case "狗狗币":
                                id = 3; break;
                            default:throw new Exception("货币不存在");
                        }
                        var virtualToken = new VirtualToken(currency, prices);

                        // Add VirtualToken to the database
                        _dbContext.VirtualTokens.Add(virtualToken);
                        //_dbContext.SaveChanges();
                    }
                }
                try
                {
                    _dbContext.SaveChanges();
                }catch(Exception e)
                {

                }


                // Save changes to the database


            }
        }
    }
}


