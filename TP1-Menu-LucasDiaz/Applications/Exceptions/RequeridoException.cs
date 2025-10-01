using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Exceptions
{
    public class RequeridoException : Exception
    {
        public RequeridoException(string message) : base(message) { }
    }
}
