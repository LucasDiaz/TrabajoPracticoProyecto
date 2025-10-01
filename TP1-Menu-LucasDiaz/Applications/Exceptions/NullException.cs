using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Exceptions
{
   
    public class NullException : Exception
    {
        public NullException(string message) : base(message) { }
    }
    //public class DishNullException : NullException
    //{
    //    public DishNullException()
    //        : base("Required dish data.") { }
    //}

    //public class DishNameNullException : NullException
    //{
    //    public DishNameNullException()
    //        : base("Name is required.") { }
    //}
    //public class CategoryNullException : NullException
    //{
    //    public CategoryNullException()
    //        : base("Required Category data.") { }
    //}
}
