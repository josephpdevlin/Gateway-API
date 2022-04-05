using Gateway.Domain;
using Gateway.Domain.Enums;
namespace Gateway.DB
{
    public interface IRepositoryManager
    {
        void Insert(Payment payment);

        Task<Payment> GetPayment(int id);

        void InsertIdempotencyRecord(IdempotencyRecord idempotencyRecord);

        IdempotencyRecord GetIdempotencyRecord(string idempotencyKey);

        void UpdatePaymentRecord(int id, BankResponse bankResponse);
    }
}