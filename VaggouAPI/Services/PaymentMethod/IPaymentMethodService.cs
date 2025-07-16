namespace VaggouAPI
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethod>> GetAllAsync();

        Task<PaymentMethod> GetByIdAsync(Guid id);

        Task<PaymentMethod> CreateAsync(CreatePaymentMethodRequestDto dto);

        Task<PaymentMethod> UpdateAsync(Guid id, CreatePaymentMethodRequestDto dto);

        Task DeleteAsync(Guid id);
    }
}