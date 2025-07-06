namespace VaggouAPI
{
    public class CreateParkingLotResponseDto
    {
        public ParkingLot CreatedParkingLot { get; set; }

        public string? NewToken { get; set; }
    }
}
