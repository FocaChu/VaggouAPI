namespace VaggouAPI
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllAsync();

        Task<Payment?> GetByIdAsync(Guid id);

        Task<IEnumerable<Payment>> GetByPaymentMethodAsync(Guid paymentMethodId);

        Task<IEnumerable<Payment>> GetByMonthAsync(int year, int month);

        Task<Payment> CreateAsync(PaymentDto dto);

        Task<Payment?> UpdateAsync(PaymentDto dto, Guid id);

        Task<bool> DeleteAsync(Guid id);
    }
}
