namespace Tams.Api.Models
{
    public class HardwareModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HardwareCategoryId { get; set; }
        public HardwareCategory HardwareCategory { get; set; }
    }
}