namespace WolfDen.Domain.Entity
{
    public class Department
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        private Department()
        {

        }
        public Department(string name)
        {
            Name = name;

        }
    }
}
