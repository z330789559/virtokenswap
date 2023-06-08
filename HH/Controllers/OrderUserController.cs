using HH.Entities;
using HH.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using HH.dto;
using Microsoft.AspNetCore.Routing;
using HH.utils;
using System.Net;
using System.Runtime.CompilerServices;
using Org.BouncyCastle.Asn1.X509;

namespace HH.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api")]
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
        public ActionResult<ApiResponse<IEnumerable<List<Order>>>> GetOrders()
        {
            return Ok(ApiResponseFactory.CreateSuccessResponse<List<Order>>(orderService.GetAllOrders()));
        }

        // GET: api/Order/1
        [HttpGet("orders/{id}")]
        public ActionResult<ApiResponse<Order>> GetOrder(int id)
        {
            var order = orderService.GetOrder(id);
            if (order == null)
            {
               var fail= ApiResponseFactory.CreateErrorResponse<Order>("未发现记录",(int)HttpStatusCode.NoContent);
                return Ok(fail);
            }
            var response = ApiResponseFactory.CreateSuccessResponse<Order>(order);
            return Ok(response);
        }



        

        [HttpGet("orders")]
        public ActionResult<ApiResponse<IEnumerable<Order>>> QueryOrderByOrderId()
        {
            string sid = "";
            if (HttpContext.Items.ContainsKey("UserId"))
            {
                string? v = (string?)HttpContext.Items["UserId"];
                sid = v;
            }
            else
            {
                return Unauthorized("用户未登录");
            }
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
            int userId = int.Parse(sid);
            if (userId == 0) { return Unauthorized("用户未登录"); }
            var order = orderService.QueryOrdersByUserName(userId);
            if (order == null)
            {
                var fail = ApiResponseFactory.CreateErrorResponse<List<Order>>("未发现记录", (int)HttpStatusCode.NoContent);
                return Ok(fail);
            }
            return Ok(ApiResponseFactory.CreateSuccessResponse<List<Order>>(order));
        }


        [HttpGet("good")]
        public ActionResult<ApiResponse<IEnumerable<Order>>> QueryOrdersByGoodsName( string? goodsName)
        {
            string sid = "";
            if (HttpContext.Items.ContainsKey("UserId"))
            {
                string? v = (string?)HttpContext.Items["UserId"];
                sid = v;
            }
            else
            {
                return Unauthorized("用户未登录");
            }
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
            int userId =int.Parse(sid);
            if (userId == 0) { return Unauthorized("用户未登录"); }
            return Ok(ApiResponseFactory.CreateSuccessResponse<List<Order>>(orderService.QueryOrdersByGoodsName(userId, goodsName)));
        }

        [HttpGet("user/assest")]
        public ActionResult<ApiResponse<object>> QueryByTotalAmount(float amount)
        {
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
            string sid = "";
            if (HttpContext.Items.ContainsKey("UserId"))
            {
                string? v = (string?)HttpContext.Items["UserId"];
                sid = v;
            }
            else
            {
                return Unauthorized("用户未登录");
            }
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
            int userId = int.Parse(sid);
            if (userId == 0) { return Unauthorized("用户未登录"); }
            return Ok(ApiResponseFactory.CreateSuccessResponse<object>(orderService.QueryByTotalAmount(userId, amount)));
        }


        [HttpGet("user/{id}")]
        public ActionResult<ApiResponse<User>> User(int id)
        {
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);

            User user = this.userService.GetUser(id);
                if (user == null)
            {
                Ok(ApiResponseFactory.CreateErrorResponse<User>("用户不存在", (int)HttpStatusCode.BadRequest));
            }
            return Ok(ApiResponseFactory.CreateSuccessResponse<User>(user));


        }



        [HttpPost("login")]
        public ActionResult<ApiResponse<UserDto>> Login([FromBody]UserDto user)
        {
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
        
                if ( user.Name == "" || user.Password=="") { return Ok(ApiResponseFactory.CreateErrorResponse<UserDto>("用户名称和密码不能为null",(int)HttpStatusCode.BadRequest)); }
                return Ok(ApiResponseFactory.CreateSuccessResponse<UserDto>(this.userService.Login(user)));


        }

        [HttpPost("register")]
        public ActionResult<ApiResponse<User>> Register([FromBody] UserDto user)
        {
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);

            if (user.Name == "" || user.Password == "") { return Ok(ApiResponseFactory.CreateErrorResponse<User>("用户名称和密码不能为null", (int)HttpStatusCode.BadRequest)); }

            if(user.Password !=user.confirmPassword) { return Ok(ApiResponseFactory.CreateErrorResponse<User>("密码和确认密码不匹配", (int)HttpStatusCode.BadRequest)); }
            User userDo = new User();
            userDo.Name = user.Name;
            userDo.Password = user.Password;
            this.userService.Register(userDo);
            return Ok(ApiResponseFactory.CreateSuccessResponse<User>(userDo));


        }

        [HttpPost("order/add")]
        public ActionResult<ApiResponse<Order>> AddOrder([FromBody]OrderDto  orderDto)
        {
            string sid = "";
            if (HttpContext.Items.ContainsKey("UserId"))
            {
                string? v = (string?)HttpContext.Items["UserId"];
                sid = v;
            }
            else
            {
                return Unauthorized("用户未登录");
            }
            //var token = httpContext.Request.Hea   ders["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
            int userId = int.Parse(sid);
            if (userId == 0) { return NotFound(); }
            try
            {
                Order order = new Order();
                order.UserId = userId;
                order.PayAssest = orderDto.PayAssest;
                order.Name = "购买";
                order.Tips = "点我康康";
                order.Details = orderDto.GoodIds.Select(g => new OrderDetail( g.id,new Goods(g.name,g.price,g.id),g.quality)).ToList();
                orderService.AddOrderByUser(userId, order);
                return Ok(ApiResponseFactory.CreateSuccessResponse<Order>(order));
            }
            catch (Exception e)
            {
                return Ok(ApiResponseFactory.CreateErrorResponse<User>(e.ToString(), (int)HttpStatusCode.BadRequest)); ;
            }

        }
       
        // Post: api/Order
        [HttpPost("order")]
        public ActionResult<ApiResponse<Order>> udpateOrder(Order order)
        {

            string sid = "";
            if (HttpContext.Items.ContainsKey("UserId"))
            {
                string? v = (string?)HttpContext.Items["UserId"];
                sid = v;
            }
            else
            {
                return Unauthorized("用户未登录");
            }
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
            int userId = int.Parse(sid);
            if (userId == 0) { return Unauthorized("用户未登录"); }
            try
            {
                orderService.UpdateOrder(order);
            }
            catch (Exception e)
            {
                string error = e.Message;
                if (e.InnerException != null) error = e.InnerException.Message;
                return Ok(ApiResponseFactory.CreateErrorResponse<Order>(e.ToString(), (int)HttpStatusCode.BadRequest));
            }
                return Ok(ApiResponseFactory.CreateSuccessResponse<Order>(order));
        }

        


        [HttpDelete("order/{id}")]
        public ActionResult<ApiResponse<Order>> DeleteOrder( int orderId)
        {
            string sid = "";
            if (HttpContext.Items.ContainsKey("UserId"))
            {
                string? v = (string?)HttpContext.Items["UserId"];
                sid = v;
            }
            else
            {
                return     Unauthorized("用户未登录");
            }
            //var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // 解码和验证令牌
            //var userId = orderService.ValidateToken(token);
            int userId = int.Parse(sid);
            if (userId == 0) { Unauthorized("用户未登录"); }
            try
            {
                orderService.DeleteOrderByUser(orderId);
            }
            catch (Exception e)
            {
                return Ok(ApiResponseFactory.CreateErrorResponse<Order>(e.ToString(), (int)HttpStatusCode.BadRequest));
            }
            return Ok(ApiResponseFactory.CreateSuccessResponse<Order>(null));
        }

    }

}

