namespace WebApp.Models.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public ICollection<OrderRowsEntity> OrderRows { get; set; } = new List<OrderRowsEntity>();
        public UserProfileEntity User { get; set; } = null!;
    }
    public class OrderRowsEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int OrderId { get; set; }

        public int ProductId { get; set; }
        public OrderEntity Order { get; set; }
        public ProductEntity Product { get; set; }

    }

}
