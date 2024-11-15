using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Domain.Entity
{
    public class Designation 
    {
        public int Id { get; private set; }
        public string DesignationName { get; private set; }
        public Designation(string designationName)
        {
            DesignationName = designationName;
        }
    }
}
