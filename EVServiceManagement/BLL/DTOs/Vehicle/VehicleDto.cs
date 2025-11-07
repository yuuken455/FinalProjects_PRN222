namespace BLL.DTOs.Vehicle
{
    public class VehicleDto
    {
        public int VehicleId { get; set; }

        public int CustomerId { get; set; }

        public string Model { get; set; } = null!;

        public string? Vin { get; set; }

        public string LicensePlate { get; set; } = null!;

        public decimal? CurrentKm { get; set; }

        public string Status { get; set; } = null!;
    }
}
