namespace WolfDen.Domain.Entity
{
    public class Device
    {
        public int Id { get; }
        public int DeviceId { get;private set; }
        public string Name { get;private set; }
        private Device()
        {
            
        }
        public Device(int deviceId, string name)
        {
            DeviceId = deviceId;
            Name = name;
        }
    }
}
