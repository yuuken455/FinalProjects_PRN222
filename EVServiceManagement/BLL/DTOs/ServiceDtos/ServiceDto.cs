using BLL.DTOs.PartDtos;

namespace BLL.DTOs.ServiceDtos
{
    public class ServiceDto
    {
        public int ServiceId { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int Price { get; set; }

        public string Type { get; set; } = null!;

        public int Duration { get; set; }

        public decimal? Km { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<PartDto> PartDtos { get; set; } = new List<PartDto>();
    }
}
