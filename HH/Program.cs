using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HH.Dao;
using HH.Entities;
using HH.Services;
using Microsoft.Extensions.Options;

namespace HH
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5087");
                    });
            });
            var configuration = builder.Configuration;

            builder.Configuration.AddJsonFile("appsettings.json", optional: false);
            // Add services to the container.

            builder.Services.AddControllers();

          
           builder.Services.AddDbContext<VirtualTokenDbContext>(options =>
    options.UseMySQL(configuration.GetConnectionString("orderDB")));




            builder.Services.AddDbContext<OrderDbContext>(options =>
   options.UseMySQL(configuration.GetConnectionString("orderDB")));


            builder.Services.AddDbContext<UserDbContext>(options =>
   options.UseMySQL(configuration.GetConnectionString("orderDB")));

            builder.Services.AddScoped<MarkowitzModel>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<WebCrawler>();
         
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddScoped<HH.Services.OrderService>();
            //��DbContext���뵽����
      
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseDefaultFiles(); //����ȱʡ��̬ҳ�棨index.html��index.htm��
            app.UseStaticFiles(); //���þ�̬�ļ���ҳ�桢js��ͼƬ�ȸ���ǰ���ļ���

            app.UseHttpsRedirection();

            app.UseCors();
            /*
            // ʹ��������֤�м��
            app.UseAuthentication();
            */
            //app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();

        }



    }
}