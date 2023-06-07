using HH.Dao;
using HH.Entities;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using HH.dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HH.Services
{
    public class UserService
    {
        UserDbContext dbContext;

        public UserService(UserDbContext dbContext)
        {
           
            this.dbContext = dbContext;
        }


        

        //修改个人信息
        public void EditInformation(int userId, User user)
        {
            var oldUser = dbContext.Users
               .SingleOrDefault(o => o.Id == userId);
            if (oldUser == null) return;
            user.Id = userId;
            user.Asset = oldUser.Asset;
            dbContext.Users.Remove(oldUser);
            dbContext.Entry(user).State = EntityState.Added;
            dbContext.SaveChanges();
        }
        //扣除费用
        public void TakeOut(int userId,double amount)
        {
            var user = dbContext.Users
               .SingleOrDefault(o => o.Id == userId);
            if (user == null) return;
            User newUser = new User(user.Name, user.Password);
            newUser.Id = userId;
            newUser.Asset = user.Asset - amount;
            dbContext.Users.Remove(user);
            dbContext.Entry(newUser).State = EntityState.Added;
            dbContext.SaveChanges();

        }
        //归还费用
        public void SendIn(int userId, double amount)
        {
            var user = dbContext.Users
               .SingleOrDefault(o => o.Id == userId);
            if (user == null) return;
            User newUser = new User(user.Name, user.Password);
            newUser.Id = userId;
            newUser.Asset = user.Asset + amount;
            dbContext.Users.Remove(user);
            dbContext.Entry(newUser).State = EntityState.Added;
            dbContext.SaveChanges();
        }


        public UserDto Login(UserDto user)
        { 
  
            var userPo = dbContext.Users
               .SingleOrDefault(u => u.Name == user.Name && u.Password == user.Password);

            if(userPo == null)
            {
                throw new Exception("user no exist");
            }
            UserDto dto = new UserDto();
            dto.Id = userPo.Id;
            dto.Name = userPo.Name;
            dto.Password = userPo.Password;
            dto.Asset = userPo.Asset;
            dto.Token = this.GeneroterJwt(userPo);

            return dto;
        }

        public void Register(User user)
        {
            dbContext.Add(user);
            dbContext.SaveChanges();

        }
        private string GeneroterJwt(User user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("my_secret_key_1234567890_h256key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("UserId", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature)
                                                           
            };

            // 生成 JWT
            var tokenObj = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(tokenObj);
            return jwt;
        }


        
    }


}
