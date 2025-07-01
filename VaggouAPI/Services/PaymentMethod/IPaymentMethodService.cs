namespace VaggouAPI
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethod>> GetAllAsync();

        Task<PaymentMethod?> GetByIdAsync(Guid id);

        Task<PaymentMethod> CreateAsync(PaymentMethodDto dto);

        Task<PaymentMethod?> UpdateAsync(PaymentMethodDto dto, Guid id);

        Task DeleteAsync(Guid id);
    }
}
