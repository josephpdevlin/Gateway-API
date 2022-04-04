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
                Status = "Succeded",
                Number = "************1881",
                Name = "Jenny Gray",
                ExpiryMonth = 1,
                ExpiryYear = 25
            };

            dbContext.Payments.Add(payment);
            dbContext.SaveChanges();
        }
    }
}
