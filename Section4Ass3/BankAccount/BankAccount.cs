namespace MOQWithUnitTesting
{
    public class BankAccount
    {
        private readonly IBankLogger _logger;
        public int Balance { get; set; }
        public BankAccount(IBankLogger logger)
        {
            Balance = 0;
            _logger = logger;

        }

        public bool Deposit(int amount)
        {
            _logger.Audit("Cash Deposited");//incase qhen we calling with parameter ,if
                                            //this was returning something lets say true,
                                            //how are we going to make sure that while
                                            //testing, it is not doing anything.
            //_logger.Audit("");//lets say thi is retrning false incase of no parameters.
            Balance += amount;
            return true;
        }

        public bool Withdraw(int amount)
        {
            _logger.LogToDB("withdrawal Amount: " + amount.ToString());
            if (Balance >= amount)
            {
                Balance -= amount;
                return _logger.LogBalanceAfterWithdrawal(Balance);
            }
            return _logger.LogBalanceAfterWithdrawal(Balance - amount);
        }

        public int GetBalance() 
        { 
            return Balance;
        }
    }
}