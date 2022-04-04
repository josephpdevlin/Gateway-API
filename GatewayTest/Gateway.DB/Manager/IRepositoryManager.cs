using Gateway.Domain.Enums;
namespace Gateway.DB
{
    public interface IRepositoryManager
    {
        void Insert(Payment payment);

        Task<Payment> GetPayment(int id);

        BankResponse GetBankResponse(int id);

        void AddBankResponse(BankResponse bankResponse);

        void InsertIdempotencyRecord(IdempotencyRecord idempotencyRecord);

        IdempotencyRecord GetIdempotencyRecord(string idempotencyKey);

        void UpdateStatus(int id, PaymentStatus status);
    }
}