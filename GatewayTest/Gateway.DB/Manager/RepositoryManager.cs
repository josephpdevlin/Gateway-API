using Gateway.Domain;

namespace Gateway.DB
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly GatewayDbContext _context;
       
        public RepositoryManager(GatewayDbContext context)
        {
            _context = context;
        }

        public void Insert(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
        }

        public async Task<Payment> GetPayment(int id)
        {
            return await _context.FindAsync<Payment>(id);
        }

        public void InsertIdempotencyRecord(IdempotencyRecord idempotencyRecord)
        {
            _context.IdempotencyRecords.Add(idempotencyRecord);
        }

        public IdempotencyRecord GetIdempotencyRecord(string idempotencyKey)
        {
            return _context.IdempotencyRecords.SingleOrDefault(x => x.IdempotencyKey == idempotencyKey);
        }

        public void UpdatePaymentRecord(int id, BankResponse bankResponse)
        {
            var payment = _context.Payments.FirstOrDefault(x => x.Id == id);
            payment.Status = bankResponse.Status;
            payment.IssuingBank = bankResponse.IssuingBank;
            _context.SaveChanges();
        }
    }
}
