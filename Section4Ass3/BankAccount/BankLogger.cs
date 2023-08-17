using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOQWithUnitTesting
{
    public class BankLogger : IBankLogger
    {
        public int LogSeverity { get; set; }
        public string LogType { get; set; }
        public void Audit(string message)
        {
            Console.WriteLine(message);
        }

        public bool LogToDB(string message)
        {
            Console.WriteLine(message);
            return true;
        }

        public bool LogBalanceAfterWithdrawal(int balanceAfterWithdrawal)
        {
            if (balanceAfterWithdrawal >= 0)
            {
                Console.WriteLine("Success");
                return true;
            }
            Console.WriteLine("failure");
            return false;
        }


        public string MessageWithReturnStr(string message)
        {
            Console.WriteLine(message);
            return message.ToLower();
        }

        public bool LogWithOutputResult(string str, out string outputStr)
        {
            outputStr = "Hello " + str;
            return true;
        }

        public bool LogWithRefObj(ref Customer customer)
        {
            return true;
        }
    }
}
