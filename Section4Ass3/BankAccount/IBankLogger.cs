using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace MOQWithUnitTesting
{
    public interface IBankLogger
    {
        public int LogSeverity { get; set; }
        public string LogType { get; set; }
        void Audit(string message);
        bool LogToDB(string message);
        bool LogBalanceAfterWithdrawal(int balanceAfterWithdrawal);
        string MessageWithReturnStr(string message);
        bool LogWithOutputResult(string str, out string outputStr);
        bool LogWithRefObj(ref Customer customer);
    }

    
    

    //public class BankFakker : IBankLogger
    //{
    //    public void Audit(string message)
    //    {

    //    }
    //}
}
