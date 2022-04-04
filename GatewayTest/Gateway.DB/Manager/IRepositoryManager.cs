using Gateway.Domain.Enums;
namespace Gateway.DB
{
    public interface IRepositoryManager
    {
        void Insert(Payment payment);

        Task<Payment> Get(int id);

        void UpdateStatus(int id, PaymentStatus status);

        bool IsDuplicateRequest(string idempotencyKey);
    }
}