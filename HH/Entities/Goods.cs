namespace HH.Entities
{
    public class Goods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public Goods()
        {
         
        }

        public Goods(string name, double price) : this()
        {
            Name = name;
            Price = price;
        }
        public Goods(string name, double price,int id ) : this()
        {
            Name = name;
            Price = price;
            Id = id;
        }


        public override bool Equals(object? obj)
        {
            return obj is Goods goods &&
                   Id == goods.Id &&
                   Name == goods.Name &&
                   Price == goods.Price;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Price);
        }
    }
}
