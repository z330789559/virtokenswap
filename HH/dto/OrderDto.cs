using System;
namespace HH.dto
{
	public class OrderDto
	{
        public List<GoodDto> GoodIds { set; get; }
		public double Amount { get; set; }

        public OrderDto()
		{
		

		}
	}
}

