namespace WolfDen.Domain.Entity
{
    public class Designation 
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        private Designation()
        {

        }
        public Designation(string name)
        {
            Name = name;

        }
    }
}
