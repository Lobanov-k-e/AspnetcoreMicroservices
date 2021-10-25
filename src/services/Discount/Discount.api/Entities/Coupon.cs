namespace Discount.api.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }


        public static Coupon NoDiscount(string productName)
        {
            return new Coupon
            {
                Description = "No Discount",
                Amount = 0,
                ProductName = productName
            };
        }
    }
}
