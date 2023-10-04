namespace qcs-product.API
{
    //Model for New Users 
    public class NewUsers
    {
        public int Id {get; set;}
        public string FirstName {get; set;} = string.Empty;
        public string LastName {get; set;} = string.Empty;
        public int Height {get; set;}
        public int Weight {get; set;}
    }
}