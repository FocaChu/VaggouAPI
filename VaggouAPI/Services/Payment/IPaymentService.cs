namespace VaggouAPI
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetMyPaymentsAsync(Guid loggedInUserId);

        Task<Payment> GetByIdAsync(Guid paymentId, Guid loggedInUserId);

   
        Task<Payment> InitiatePaymentForReservationAsync(InitiatePaymentRequestDto dto, Guid loggedInUserId);

        
        Task<Payment> UpdatePaymentStatusAsync(Guid paymentId, Status newStatus);
    }
}