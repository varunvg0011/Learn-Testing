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
        public int OrderTotal { get; set; }
        public int Discount { get; set; } = 20;
        public string CustGreetingFullName(string firstName, string lastName)
        {

            if(string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("Empty First Name");
            }
            GreetMsg = "Hello, " + firstName + " " + lastName;
            Discount = 35;
            return GreetMsg;

        }       

        public CustomerType GetCustomerDetails()
        {
            if (OrderTotal < 100)
            {
                return new BasicCustomer();
            }
            return new PlatinumCustomer();
        }
    }


    public class CustomerType { }
    public class BasicCustomer :CustomerType{ }
    public class PlatinumCustomer: CustomerType { }
}
