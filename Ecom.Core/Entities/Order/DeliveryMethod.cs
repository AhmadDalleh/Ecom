namespace Ecom.Core.Entities.Order
{
    public class DeliveryMethod : BaseEntity<int>
    {
        public DeliveryMethod()
        {

        }

        public DeliveryMethod(string name, decimal price, string description, string deliveryTime)
        {
            Name = name;
            Price = price;
            Description = description;
            DeliveryTime = deliveryTime;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string DeliveryTime { get; set; }

    }
}
