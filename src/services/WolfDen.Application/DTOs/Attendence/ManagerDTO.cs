using WolfDen.Domain.Entity;

namespace WolfDen.Application.DTOs.Attendence
{
    public class ManagerDTO
    {
        public Employee Manager { get; set; }
        public List<SubOrdinateDetailsDTO> Subordinates { get; set; }  

    }
}
