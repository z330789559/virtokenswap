using HH.Entities;
using HH.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using HH.dto;
using Microsoft.AspNetCore.Routing;

namespace HH.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/orders")]
    //[Route("api/users")]
    public class OrderUserController : ControllerBase
    {
        public readonly OrderService orderService;
        public readonly UserService userService;

        public OrderUserController(OrderService orderService, UserService userService)
        {
            this.orderService = orderService;
            this.userService = userService;
            
        }
        // GET: api/Order
        [HttpGet]
        public ActionResult<List<Order>> GetOrders()
        {
            return orderService.GetAllOrders();
        }

        // GET: api/Order/1
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            var order = orderService.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }



        

        [HttpGet("{userId,orderId}")]
        public ActionResult<Order> QueryOrderByOrderId(int userId, int orderId)
        {
            
            if (userId == null) { return NotFound(); }
            var order = orderService.QueryOrderByOrderId(userId, orderId);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }


        [HttpGet("{goodsName}")]
        public ActionResult<List<Order>> QueryOrdersByGoodsName( string goodsName)
        {
            string sid = "";
            if (HttpContext.Items.ContainsKey("Sid"))
            {
                 sid = HttpContext.Items["Sid"].ToString();
            }
            else
            {
                return BadRequest("用户未登录");
            }
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
            int userId =int.Parse(sid);
            if (userId == 0) { return NotFound(); }
            return orderService.QueryOrdersByGoodsName(userId, goodsName);
        }

        [HttpGet("{userId,amount}")]
        public ActionResult<object> QueryByTotalAmount(int userId, float amount)
        {
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
            if (userId == null) { return NotFound(); }
            return orderService.QueryByTotalAmount(userId, amount);
        }



        [HttpPost("login")]
        public ActionResult<UserDto> Login([FromBody]UserDto user)
        {
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
        
                if ( user.Name == "" || user.Password=="") { return BadRequest("用户名称和密码不能为null"); }
                return this.userService.Login(user);


        }

        [HttpPut()]
        public ActionResult<Order> AddOrder(int userId,  Order order)
        {
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
            try
            {
                if (userId == null) { return NotFound(); }
                orderService.AddOrderByUser(userId, order);
                
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }

            return NoContent();
        }
       
        // Post: api/Order
        [HttpPost()]
        public ActionResult<Order> udpateOrder(Order order)
        {
   
            try
            {
                orderService.UpdateOrder(order);
            }
            catch (Exception e)
            {
                string error = e.Message;
                if (e.InnerException != null) error = e.InnerException.Message;
                return BadRequest(error);
            }
            return NoContent();
        }

        


        [HttpDelete("{userId,orderId}")]
        public ActionResult<Order> DeleteOrder(int userId, int orderId)
        {
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
            try
            {
                if (userId == null) { return NotFound(); }
                orderService.DeleteOrderByUser(userId, orderId);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return NoContent();
        }

    }

}

