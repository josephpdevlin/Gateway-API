using Gateway.Domain.Enums;

namespace Gateway.DB
{
    public class RepositoryManager : IRepositoryManager
    {
        private GatewayDbContext _context;
       
        public RepositoryManager(GatewayDbContext context)
        {
            _context = context;
        }

        public void Insert(Payment payment)
        {
            _context.Add(payment);
            _context.SaveChanges();
        }

        public async Task<Payment> Get(int id)
        {
            return await _context.FindAsync<Payment>(id);
        }

        public void UpdateStatus(int id, PaymentStatus status)
        {
            var payment = _context.Payments.FirstOrDefault(x => x.Id == id);
            payment.Status = status;
            _context.SaveChanges();
        }

        public bool IsDuplicateRequest(string idempotencyKey)
        {
            return _context.IdempotencyRecord.Any(x => x.IdempotencyKey == idempotencyKey);
        }
    }
}
