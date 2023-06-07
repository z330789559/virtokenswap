using HH.Dao;
using HH.Entities;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HH.dto;
//using System.Data.Entity;

namespace HH.Services
{
    
    public class OrderService
    {

        OrderDbContext orderDbContext;
        UserDbContext userDbContext;


        public OrderService(OrderDbContext orderDbContext,UserDbContext userDbContext)
        {

           
            this.orderDbContext = orderDbContext;
            this.userDbContext = userDbContext;

        }

        /*
        public string ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

                // Configure validation parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                // Validate token
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                // Extract UserId from token claims
                var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return userId;
            }
            catch (Exception)
            {
                // Token validation failed
                return null;
            }
        }
        */

        public List<Order> GetAllOrders()
        {
            return orderDbContext.Orders
                .Include(o => o.Details)
                .ThenInclude(d => d.GoodsItem)
                .Include(o => o.User)
                .ToList<Order>();
        }


        public Order GetOrder(int id)
        {
            return orderDbContext.Orders
                .Include(o => o.Details)
                .ThenInclude(d => d.GoodsItem)
                .Include(o => o.User)
                .SingleOrDefault(o => o.OrderId == id);
        }

        //扣除费用
        public void TakeOut(int userId, double amount)
        {
            var user = userDbContext.Users
               .SingleOrDefault(o => o.Id == userId);
            if (user == null) return;
            User newUser = new User(user.Name, user.Password);
            newUser.Id = userId;
            newUser.Asset = user.Asset - amount;
            if(newUser.Asset < 0)
            {
                throw new Exception("金额不足");
            }
            userDbContext.Users.Remove(user);
            userDbContext.Entry(newUser).State = EntityState.Added;
            userDbContext.SaveChanges();

        }
        //归还费用
        public void SendIn(int userId, double amount)
        {
            var user = userDbContext.Users
               .Include(o => o.Id)
               .SingleOrDefault(o => o.Id == userId);
            if (user == null) return;
            User newUser = new User(user.Name, user.Password);
            newUser.Id = userId;
            newUser.Asset = user.Asset + amount;
            userDbContext.Users.Remove(user);
            userDbContext.Entry(newUser).State = EntityState.Added;
            userDbContext.SaveChanges();
        }

        public void AddOrder(Order order)
        {
            FixOrder(order);
            orderDbContext.Entry(order).State = EntityState.Added;
            orderDbContext.SaveChanges();
        }

        public void AddOrderDetail(List<OrderDetail> items)
        {
            orderDbContext.OrderDetails.AddRange(items);
            orderDbContext.SaveChanges();
        }

        public void RemoveOrder(int orderId)
        {
            var order = orderDbContext.Orders
                .Include(o => o.Details)
                .SingleOrDefault(o => o.OrderId == orderId);
            if (order == null) return;
            orderDbContext.OrderDetails.RemoveRange(order.Details);
            orderDbContext.Orders.Remove(order);
            orderDbContext.SaveChanges();
        }

        public void AddOrderByUser(int userId, Order order)
        {
          
            TakeOut(userId, order.TotalPrice);
            order.TotalPriceValue = order.TotalPrice;
            AddOrder(order);
            AddOrderDetail(order.Details);
        }

        public void DeleteOrderByUser(int orderId)
        {
            var order = GetOrder(orderId);
            SendIn(order.UserId, order.TotalPrice);
            RemoveOrder(orderId);
        }

        //
        public Order QueryOrderByOrderId(int userId, int orderId)
        {
          
            return orderDbContext.Orders
                .Include(o => o.Details)
                .ThenInclude(d => d.GoodsItem)
            .Include(o => o.User)
                .SingleOrDefault(o => (o.User.Id == userId) && (o.OrderId == orderId));

        }


        //
        public List<Order> QueryOrdersByGoodsName(int userId, string goodsName)
        {
            var query = orderDbContext.Orders
                .Include(o => o.Details)
                .ThenInclude(d => d.GoodsItem)
                .Include(o => o.User)
                .Where(order => (order.User.Id == userId));
            if(goodsName!=null)
            {
                query = query.Where(order => order.Details.Any(item => EF.Functions.Like(item.GoodsItem.Name, $"%{goodsName}%")));
            }
            // && (order.Details.Any(item => item.GoodsItem.Name == goodsName))
            return query.ToList();
        }
        //
        public List<Order> QueryOrdersByUserName(int userId)
        {
            return orderDbContext.Orders
                .Include(o => o.Details)
                .ThenInclude(d => d.GoodsItem)
                .Include("User")
              .Where(order => order.User.Id == userId)
              .ToList();
        }

        public void UpdateOrder(Order newOrder)
        {
            RemoveOrder(newOrder.OrderId);
            AddOrder(newOrder);
        }

        

        //避免级联添加或修改User和VirtualCoin
        private static void FixOrder(Order newOrder)
        {
            if (newOrder.User != null)
            {
                newOrder.UserId = newOrder.User.Id;
            }
            newOrder.User = null;
            newOrder.Details.ForEach(d => {
                if (d.GoodsItem != null)
                {
                    d.GoodsId = d.GoodsItem.Id;
                }
                d.GoodsItem = null;
            });
        }

        public void Export(String fileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                xs.Serialize(fs, GetAllOrders());
            }
        }

        public void Import(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Order>));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                List<Order> temp = (List<Order>)xs.Deserialize(fs);
                temp.ForEach(order => {
                    if (orderDbContext.Orders.SingleOrDefault(o => o.OrderId == order.OrderId) == null)
                    {
                        FixOrder(order);
                        orderDbContext.Orders.Add(order);
                    }
                });
                orderDbContext.SaveChanges();
            }
        }
        //
        public object QueryByTotalAmount(int userId, float amount)
        {
            return orderDbContext.Orders.Include(o => o.Details).ThenInclude(d => d.GoodsItem).Include("User")
            .Where(order => (order.User.Id == userId) && (order.Details.Sum(d => d.Quantity * d.GoodsItem.Price) > amount))
            .ToList();
        }

      
    }


}
