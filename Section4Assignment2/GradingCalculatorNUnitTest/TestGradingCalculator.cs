using NUnit.Framework;
using Section4Assignment2;
using static System.Formats.Asn1.AsnWriter;

namespace GradingCalculatorNUnitTest
{
    [TestFixture]
    public class TestGradingCalculator
    {
        private GradingCalculator _gC;
        [SetUp]
        public void Setup()
        {
            _gC = new GradingCalculator();
        }

        [Test]
        public void GetGrade_InputScoreAndAttendance_OutputGradeA()
        {
            //if score 95 and attendance 90 ->A
            _gC.Score = 95;
            _gC.AttendancePercentage = 65;
            string grade = _gC.GetGrade();
            //Assert.That(grade, Is.EqualTo("A"));

            

            

            
        }

        [Test]
        public void GetGrade_InputScoreAndAttendance_OutputGradeB1()
        {
            _gC.Score = 85;
            _gC.AttendancePercentage = 90;
            string grade = _gC.GetGrade();

            //if score 85 and attendance 90->B
            Assert.That(grade, Is.EqualTo("B"));
        }


        [Test]
        public void GetGrade_InputScoreAndAttendance_OutputGradeC()
        {
            _gC.Score = 65;
            _gC.AttendancePercentage = 90;
            string grade = _gC.GetGrade();

            //if score 65 and attendance 90->C
            Assert.That(grade, Is.EqualTo("C"));
        }


        [Test]
        public void GetGrade_InputScoreAndAttendance_OutputGradeB2()
        {
            _gC.Score = 95;
            _gC.AttendancePercentage = 65;
            string grade = _gC.GetGrade();
            //if score 95 and attendance 65 ->B
            Assert.That(grade, Is.EqualTo("B"));
        }


        [Test]
        [TestCase(95,55)]
        [TestCase(65,55)]
        [TestCase(50,90)]
        public void GetGrade_InputScoreAndAttendance_OutputGradeF(int score, int attendance)
        {
            _gC.Score = score;
            _gC.AttendancePercentage = attendance;
            string grade = _gC.GetGrade();
            //if score 95 and attendance 65 ->B
            Assert.That(grade, Is.EqualTo("F"));
        }
        
        [Test]
        [TestCase(95,90, ExpectedResult ="A")]
        [TestCase(85,90, ExpectedResult ="B")]
        [TestCase(65,90, ExpectedResult ="C")]
        [TestCase(95,65, ExpectedResult ="B")]
        [TestCase(95,55, ExpectedResult ="F")]
        [TestCase(65,55, ExpectedResult = "F")]
        [TestCase(50,90, ExpectedResult = "F")]
        public string GetGrade_InputScoreAndAttendance_OutputGradeWithExpectation(int score, int attendance)
        {
            _gC.Score = score;
            _gC.AttendancePercentage = attendance;
            return _gC.GetGrade();
        }
    }
}