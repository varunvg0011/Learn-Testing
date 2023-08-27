using Bongo.Models.ModelValidations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.Models
{
    [TestFixture]
    public class DateInFutureAttributeTest
    {
        [Test]
        [TestCase(1,ExpectedResult =true)]
        [TestCase(0,ExpectedResult =false)]
        [TestCase(-1,ExpectedResult =false)]
        public bool DateValidator_InputExpectedDateRange_DateValidity(int addedHours)
        {
            DateInFutureAttribute dateObj = new(()=>DateTime.Now);
            //since we have added 1 hour to above, it should pass the test as
            //we are booking the study room foir future time and not current and before
            //time
            return dateObj.IsValid(DateTime.Now.AddHours(addedHours));          
        }


        [Test]
        public void DateValidator_NotValidDate_ReturnErrorMessage()
        {
            var result = new DateInFutureAttribute();
            Assert.AreEqual("Date must be in the future", result.ErrorMessage);
        }
    }
}
