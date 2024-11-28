namespace WolfDen.Domain.Entity
{
    public class Device
    {
        public int Id { get; }
        public string Name { get;private set; }
        private Device()
        {
            
        }
        public Device(string name)
        {
            Name = name;
        }
    }
}
