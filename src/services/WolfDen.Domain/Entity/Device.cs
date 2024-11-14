namespace WolfDen.Domain.Entity
{
    public class Device
    {
        public int Id { get;private set; }
        public int DeviceId { get;private set; }
        public string Name { get;private set; }
        public Device()
        {
            
        }
        public Device(int deviceId, string name)
        {
            DeviceId = deviceId;
            Name = name;
        }
    }
}
