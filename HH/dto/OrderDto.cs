using System;
namespace HH.dto
{
	public class OrderDto
	{
        public List<GoodDto> GoodIds { set; get; }

		public  string PayAssest { set; get; }

        public OrderDto()
		{
		

		}
	}
}

