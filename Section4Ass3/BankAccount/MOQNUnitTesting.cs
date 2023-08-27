using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOQWithUnitTesting
{
    [TestFixture]
    public class MOQNUnitTesting
    {
        private BankAccount _bankAcc;

        [SetUp]
        public void Setup()
        {
            
        }

        /*[Test]
        public void DepositLogFakker_InputAmount_OutputIfDeposited()
        {
            //this way we have a dependency of classes and this is not wrong but since we are doing 
            //unit testing we dont want dependency as it will then be integration testing instead.
            //So, in order to remove that we will be using MOQ
            BankAccount bankAcc = new BankAccount(new BankFakker());//LogFakker is nothing but we have
            //created it to run our test for the time being without having any dependency
            //but this is not the right way to do it in the real world.


            bool result = bankAcc.Deposit(100);
            Assert.IsTrue(result);
            Assert.That(bankAcc.GetBalance(), Is.EqualTo(100));

        }*/


        [Test]
        public void DepositMOQ_InputAmount100_OutputIfDepositedTrue()
        {
            //Here we are telling Mock to mock IBankLogger interface and it
            //creates a fake object of whatever we are mocking. We are telling that
            //IBankLogger should not do anything at all while we are doing testing in BankAccount
            //class as we want to perform unit test.
            var logMock = new Mock<IBankLogger>();

            //incase when it is returning something and is altering the execution path,
            //we need the mock that what this particular message will return.
            //Below, we are sying that if the parameter value is "Cash Deposited", then
            //do something.. and if it is ""-> then do something. Full explanation in later
            //videos

            //logMock.Setup(x => x.Audit("Cash Deposited")).do something..;


            logMock.Setup(x => x.Audit(""));
            //here we are using the mock created instance instead of fake logger
            BankAccount bankAcc = new(logMock.Object);

            

            bool result = bankAcc.Deposit(100);
            Assert.IsTrue(result);
            Assert.That(bankAcc.GetBalance(), Is.EqualTo(100));

        }

        [Test]
        [TestCase(100,200)]
        public void WithdrawMOQ_Withdraw100With200Balance_OutputTrue(int amount, int balance)
        {
            var logMock = new Mock<IBankLogger>();
            //as long as its a string, it should return true. So, this line is not even important
            logMock.Setup(m => m.LogToDB(It.IsAny<string>())).Returns(true);
            logMock.Setup(m=>m.LogBalanceAfterWithdrawal(It.Is<int>(i=> i > 0))).Returns(true);
            BankAccount bankAcc = new(logMock.Object);
            bankAcc.Deposit(balance);
            bool result = bankAcc.Withdraw(amount);
            Assert.IsTrue(result);
        }


        [Test]
        [TestCase(300, 200)]

        public void WithdrawMOQ_Withdraw300With200Balance_OutputTrue(int amount, int balance)
        {
            var logMock = new Mock<IBankLogger>();
            logMock.Setup(m => m.LogBalanceAfterWithdrawal(It.Is<int>(i => i > 0))).Returns(true);
            //default value that returns is false if we dont mention anything


            //logMock.Setup(m => m.LogBalanceAfterWithdrawal(It.Is<int>(i => i < 0))).Returns(false);
            logMock.Setup(m => m.LogBalanceAfterWithdrawal(It.IsInRange<int>(int.MinValue,-1,Moq.Range.Inclusive))).Returns(false);
            BankAccount bankAcc = new(logMock.Object);
            bankAcc.Deposit(balance);
            bool result = bankAcc.Withdraw(amount);
            //Assert.IsTrue(result);
            Assert.IsFalse(result);
        }


        //This beloe is a test case for a wierd situation in which you have to test a method that
        //is not included in Withdraw function.
        //And this is tested inside logger and not BankAccount class

        [Test]
        public void BankLogDummy_LogMockString_OutputTrue()
        {
            var logMock = new Mock<IBankLogger>();
            logMock.Setup(m => m.MessageWithReturnStr(It.IsAny<string>())).Returns((string str) => str.ToLower()); ;

            Assert.That(logMock.Object.MessageWithReturnStr("HelLO"), Is.EqualTo("hello"));
        }


        [Test]
        public void BankLogDummy_LogMockStringOutputStr_OutputTrue()
        {
            var logMock = new Mock<IBankLogger>();
            string desiredOutput = "hello";
            logMock.Setup(m => m.LogWithOutputResult(It.IsAny<string>(), out desiredOutput)).Returns(true);

            //now we are checking if aboce steup goes right
            string result = "";
            Assert.IsTrue(logMock.Object.LogWithOutputResult("Ben", out result));
            //result is returning from above and being used below with its new value
            Assert.That(result, Is.EqualTo(desiredOutput));
        }
        
        [Test]
        public void BankLogDummy_LogWithRefResult_OutputTrue()
        {
            var logMock = new Mock<IBankLogger>();

            Customer customer = new();
            Customer customerNotInUse = new();
            logMock.Setup(m => m.LogWithRefObj(ref customer)).Returns(true);

            Assert.IsTrue(logMock.Object.LogWithRefObj(ref customer));

            //since we have setup only customer obj, it dosnt know what to do with customerNotInUse 
            //so test wont pass
            //Assert.IsTrue(logMock.Object.LogWithRefObj(ref customerNotInUse));
            
        }

        [Test]
        public void BankLogDummy_SetAndGetLogTypeAndSeverityMock_MockTest()
        {

            var logMock = new Mock<IBankLogger>();
            //we have to set below statement in the correct order, on the top
            //otherwise it won't work
            //By doing below we ignore the below Setup that we used and only reads the manual
            //values that we setup. In this case, it will be 100 for logSeverity
            logMock.SetupAllProperties();

            logMock.Setup(u => u.LogSeverity).Returns(10);
            logMock.Setup(u => u.LogType).Returns("warning");
            logMock.Object.LogSeverity = 100;
            Assert.That(logMock.Object.LogSeverity, Is.EqualTo(10));
            Assert.That(logMock.Object.LogType, Is.EqualTo("warning"));
        }


        //Callbacks
        [Test]
        public void LogToDB_InputWithCallback_ReturnChangedValues()
        {
            string logTemp = "Hello, ";
            var logMock = new Mock<IBankLogger>();
            //Whatever value we pass in the parameter of LogToDB below, we are accessing that value
            //and doing another operation on it
            logMock.Setup(m => m.LogToDB(It.IsAny<string>())).Returns(true)
                .Callback((string result) => logTemp += result);

            //Now calling it
            logMock.Object.LogToDB("Varun");
            Assert.That(logTemp, Is.EqualTo("Hello, Varun"));
            //---------------

            int counter = 5;

            //callbacks can be before or after returns as well
            logMock.Setup(m => m.LogToDB(It.IsAny<string>())).Callback(() => counter++)
                .Returns(true)
                .Callback(() => counter++);
            logMock.Object.LogToDB("Varun");//counter = 7, as each call is making 2 increments
            logMock.Object.LogToDB("Varun");//counter = 9
            Assert.That(counter, Is.EqualTo(9));

        }


    }
}
