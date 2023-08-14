using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    public class Customer
    {
        public string GreetMsg { get; set; }
        public string CustFullName(string firstName, string lastName)
        {
            GreetMsg = firstName + " " + lastName;
            return GreetMsg;
        }
    }
}
