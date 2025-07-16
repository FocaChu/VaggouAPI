using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace VaggouAPI
{
    public class PaymentService : IPaymentService
    {
        private readonly Db _context;
        private readonly IMapper _mapper;

        public PaymentService(Db context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Payment>> GetMyPaymentsAsync(Guid loggedInUserId)
        {
            return await _context.Payments
                .Include(p => p.Reservation)
                .Where(p => p.Reservation.ClientId == loggedInUserId)
                .Include(p => p.PaymentMethod)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Payment> GetByIdAsync(Guid paymentId, Guid loggedInUserId)
        {
            var payment = await _context.Payments
                .Include(p => p.Reservation)
                .Include(p => p.PaymentMethod)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == paymentId)
                    ??throw new NotFoundException("Payment not found.");

            if (payment.Reservation?.ClientId != loggedInUserId)
                throw new UnauthorizedException("You do not have permission to view this payment.");

            return payment;
        }

        public async Task<Payment> InitiatePaymentForReservationAsync(InitiatePaymentRequestDto dto, Guid loggedInUserId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.ParkingSpot)
                .FirstOrDefaultAsync(r => r.Id == dto.ReservationId && r.ClientId == loggedInUserId);


            if (reservation == null)
                throw new NotFoundException("Reservation not found or does not belong to the user.");


            var existingPayment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == reservation.PaymentId && p.Status == Status.Success);
            if (existingPayment != null)
                throw new BusinessException("This reservation has already been paid successfully.");


            var paymentMethod = await _context.PaymentMethods.FindAsync(dto.PaymentMethodId)
                ?? throw new BusinessException("Selected payment method is not valid.");


            var duration = reservation.timeEnd - reservation.timeStart;
            decimal amount = (decimal)duration.TotalHours * (decimal)reservation.ParkingSpot.PricePerHour;


            if (amount <= 0)
                throw new BusinessException("Calculated amount for the reservation is invalid.");

            var paymentEntity = new Payment
            {
                Amount = amount, 
                Status = Status.Pending,
                PaidAt = DateTime.UtcNow,
                PaymentMethodId = dto.PaymentMethodId,
                Id = reservation.PaymentId
            };

            _context.Payments.Update(paymentEntity); 
            await _context.SaveChangesAsync();

            paymentEntity.PaymentMethod = paymentMethod;
            return paymentEntity;
        }

        public async Task<Payment> UpdatePaymentStatusAsync(Guid paymentId, Status newStatus)
        {
            var payment = await _context.Payments.FindAsync(paymentId)
                ?? throw new NotFoundException("Payment to update not found.");

            payment.Status = newStatus;
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}