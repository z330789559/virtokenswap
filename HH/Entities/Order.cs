using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HH.Entities
{
    //交易订单

    public class Order : IComparable<Order>
    {

        public int OrderId { get; set; }

        public int UserId { get; set; }

        public int Status { get; set; }

        public int Type { get; set; }

        public string? Name { get; set; }
        public string? Tips { get; set; }

        public string PayAssest { get; set; }

        [Column("TotalPrice")]
        public double TotalPriceValue { get; set; }


        [ForeignKey("UserId")]
        public User? User { get; set; }

   
        public string? UserName { get => (User != null) ? User.Name : ""; }

        public DateTime CreateTime { get; set; }

        public List<OrderDetail> Details { get; set; }

        public Order()
        {
         
            Details = new List<OrderDetail>();
            CreateTime = DateTime.Now;
        }

        public Order(User user, List<OrderDetail> items) : this()
        {
            this.User = user;
            this.Details = items;
        }

        [NotMapped]
        public double TotalPrice
        {
            get => Details.Sum(item => item.TotalPrice);
          
    
        }


        public void AddDetail(OrderDetail orderItem)
        {
            if (Details.Contains(orderItem))
                throw new ApplicationException($"添加错误：订单项{orderItem.GoodsName} 已经存在!");
            Details.Add(orderItem);
        }

        public void RemoveDetail(OrderDetail orderItem)
        {
            Details.Remove(orderItem);
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append($"Id:{OrderId}, user:{User},orderTime:{CreateTime},totalPrice：{TotalPrice}");
            Details.ForEach(od => strBuilder.Append("\n\t" + od));
            return strBuilder.ToString();
        }

        public override bool Equals(object obj)
        {
            var order = obj as Order;
            return order != null &&
                   OrderId == order.OrderId;
        }

        public override int GetHashCode()
        {
            var hashCode = -531220479;
            hashCode = hashCode * -1521134295 + OrderId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserName);
            hashCode = hashCode * -1521134295 + CreateTime.GetHashCode();
            return hashCode;
        }

        public int CompareTo(Order other)
        {
            if (other == null) return 1;
            return this.OrderId.CompareTo(other.OrderId);
        }
    }


}
