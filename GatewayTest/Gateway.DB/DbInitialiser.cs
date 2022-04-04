using Gateway.Domain.Enums;

namespace Gateway.DB
{
    public static class DbInitialiser
    {
        public static void Initialise(GatewayDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var payment = new Payment()
            {
                Id = 1,
                Amount = 100,
                Currency = "GBP",
                Status = PaymentStatus.Succeded,
                Card = new Card()
                {
                    Id = 1,
                    Number = "4012888888881881",
                    Name = "Jenny Gray",
                    ExpiryMonth = 1,
                    ExpiryYear = 25,
                }
            };

            dbContext.Add(payment);
            dbContext.SaveChanges();
        }
    }
}
