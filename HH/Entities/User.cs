using System.ComponentModel.DataAnnotations.Schema;
namespace HH.Entities
{
    //用户
    [Table("User")]
    public class User
    {
        //public string Token { get; set; }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        
        public double Asset { get; set; }  //总资产

        public User()
        {
 
            //Token = Guid.NewGuid().ToString();
        }

        public User(string name, string password) : this()
        {
            Name = name;
            Password = password;
            Asset = 0;
        }

        


        public override bool Equals(object? obj)
        {
            return obj is User user &&
                   //Token == user.Token &&
                   Id == user.Id &&
                   Name == user.Name &&
                   Password == user.Password;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Password);
        }

        /*
        var hashCode = 1479869798;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        */



    }


}
