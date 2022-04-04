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

        public async Task<Payment> GetPayment(int id)
        {
            return await _context.FindAsync<Payment>(id);
        }

        public void AddBankResponse(BankResponse bankResponse)
        {
            _context.BankResponses.Add(bankResponse);
        }

        public BankResponse GetBankResponse(int paymentId)
        {
            return _context.BankResponses.SingleOrDefault(x => x.PaymentId == paymentId);
        }

        public void InsertIdempotencyRecord(IdempotencyRecord idempotencyRecord)
        {
            _context.IdempotencyRecords.Add(idempotencyRecord);
        }

        public IdempotencyRecord GetIdempotencyRecord(string idempotencyKey)
        {
            return _context.IdempotencyRecords.SingleOrDefault(x => x.IdempotencyKey == idempotencyKey);
        }

        public void UpdateStatus(int id, PaymentStatus status)
        {
            var payment = _context.Payments.FirstOrDefault(x => x.Id == id);
            payment.Status = status;
            _context.SaveChanges();
        }
    }
}
