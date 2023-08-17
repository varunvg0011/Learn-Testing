using NUnit.Framework;
using Section4Ass3;

namespace FiboNUnitTest
{
    [TestFixture]
    public class FiboTest
    {
        private Fibo fibo;

        [SetUp]
        public void Setup()
        {
            fibo = new Fibo();
        }

        [Test]
        [TestCase(1)]
        public void GetFiboSeries_InputListRange_OutputList(int range)
        {
            //arrange
            fibo.Range = range;


            //act
            List<int> list = fibo.GetFiboSeries();


            //assert
            Assert.That(list, Is.Not.Empty);
            Assert.That(list, Is.Ordered);
            Assert.That(list, Is.EquivalentTo(new List<int> { 0 }));
        }


        [Test]
        [TestCase(6)]
        public void GetFiboSeries_InputListRange_CheckConstraintsInList(int range)
        {
            //arrange
            fibo.Range = 6;


            //act
            List<int> list = fibo.GetFiboSeries();


            //assert
            Assert.That(list, Does.Contain(3));
            Assert.That(list.Count, Is.EqualTo(6));
            Assert.That(list, Does.Not.Contain(4));
            //or
            Assert.That(list, Has.No.Member(4));
            Assert.That(list, Is.EquivalentTo(new List<int> { 0,1,1,2,3,5 }));
        }
    }
}