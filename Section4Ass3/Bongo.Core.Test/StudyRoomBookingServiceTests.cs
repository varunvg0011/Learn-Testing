using Bongo.Core.Services;
using Bongo.DataAccess.Repository;
using Bongo.DataAccess.Repository.IRepository;
using Bongo.Models.Model;
using Bongo.Models.Model.VM;
using Moq;
using NUnit.Framework;

namespace Bongo.Core.Test
{
    [TestFixture]
    public class StudyRoomBookingServiceTests
    {
        private Mock<IStudyRoomBookingRepository> _studyRoomBookingRepoMock;

        //even though we are not using this in the Setup below, we still need to mock as
        //we have to pass this as parameter in the StudyRoomBookingService object
        private Mock<IStudyRoomRepository> _studyRoomRepoMock;
        private StudyRoomBookingService _bookingService;

        //Test2 data

        //creating a book room request obj to be passed onto the method
        private StudyRoomBooking _roomBookingRequestObj;

        //we are booking a room from the leftover/available rooms so here, we will hard code 
        //it with a bunch of rooms so we are able to book one from them
        private List<StudyRoom> _availableStudyRoomList;

        [SetUp]
        public void Setup()
        {
            _studyRoomBookingRepoMock = new Mock<IStudyRoomBookingRepository>();
            _studyRoomRepoMock = new Mock<IStudyRoomRepository>();
            _bookingService = new StudyRoomBookingService(_studyRoomBookingRepoMock.Object, _studyRoomRepoMock.Object);
            
            //Test2 setup
            _roomBookingRequestObj = new StudyRoomBooking()
            {
                FirstName = "Tarun",
                LastName = "Gupta",
                Date = new DateTime(2023-8-28),
                Email = "varun@gmail.com"
            };

            _availableStudyRoomList = new List<StudyRoom>()
            {
                new StudyRoom
                {
                    Id = 10, RoomName="michigan", RoomNumber="A202"
                }
            };

            //Seting up below so as to make sure on GetAll() it returns the above list
            //so we can continue with out testing of whether room booked succesully or not
            _studyRoomRepoMock.Setup(r => r.GetAll()).Returns(_availableStudyRoomList);
        }


        [Test]
        public void GetAllBooking_InvokeMethod_CheckIfRepoIsCalled()
        {
            _bookingService.GetAllBooking();
            _studyRoomBookingRepoMock.Verify(x=>x.GetAll(null), Times.Once());//pass
            //_studyRoomBookingRepoMock.Verify(x=>x.GetAll(null), Times.Never());
        }

        [Test]
        public void BookingException_NullRequest_ArgumentNullException()
        {
            //below statement translates to :
            //throws this type of exception when mentioned function is called
            var exception = Assert.Throws<ArgumentNullException>(
                () => _bookingService.BookStudyRoom(null));

            //if we want to be specific about the exception
            Assert.AreEqual("Value cannot be null. (Parameter 'request')", exception.Message);
            Assert.AreEqual("request", exception.ParamName);
        }


        [Test]
        public void BookingRoom_BookRoomRequest_BookedSuccesfully()
        {
            //arrange
            StudyRoomBooking savedStudyRoomBooking=null;

            //we have to mock on .Book method to tell what happends when we call it
            //in callbak, whatever variable we passed int the x.Book, we can access that and
            //here "booking" is that variable 
            _studyRoomBookingRepoMock.Setup(x => x.Book(It.IsAny<StudyRoomBooking>()))
                .Callback<StudyRoomBooking>(booking =>
                {
                    savedStudyRoomBooking = booking;
                });

            //act
            _bookingService.BookStudyRoom(_roomBookingRequestObj);


            //assert
            _studyRoomBookingRepoMock.Verify(x => x.Book(It.IsAny<StudyRoomBooking>()), Times.Once);
            Assert.NotNull(savedStudyRoomBooking);
            Assert.AreEqual(_roomBookingRequestObj.FirstName, savedStudyRoomBooking.FirstName);
            Assert.AreEqual(_roomBookingRequestObj.LastName, savedStudyRoomBooking.LastName);
            Assert.AreEqual(_roomBookingRequestObj.Date, savedStudyRoomBooking.Date);
            Assert.AreEqual(_roomBookingRequestObj.Email, savedStudyRoomBooking.Email);
            Assert.AreEqual(_availableStudyRoomList.First().Id, savedStudyRoomBooking.StudyRoomId);
        }


        [Test]
        public void StudyRoomBookingResultCheck_InputRequest_ValuesMatchInResult()
        {
            StudyRoomBookingResult result = _bookingService.BookStudyRoom(_roomBookingRequestObj);
            Assert.AreEqual(_roomBookingRequestObj.FirstName, result.FirstName);
            Assert.AreEqual(_roomBookingRequestObj.LastName, result.LastName);
            Assert.AreEqual(_roomBookingRequestObj.Date, result.Date);
            Assert.AreEqual(_roomBookingRequestObj.Email, result.Email);
        }

        //Checking what it returns in status code after succesful/failed booking
        [TestCase(true, ExpectedResult = StudyRoomBookingCode.Success)]
        [TestCase(false, ExpectedResult = StudyRoomBookingCode.NoRoomAvailable)]
        public StudyRoomBookingCode ResultCodeSuccess_RoomAvailability_ReturnsSuccessResultCode(bool roomAvailability)
        {
            if (!roomAvailability)
            {
                _availableStudyRoomList.Clear();
            }
            return _bookingService.BookStudyRoom(_roomBookingRequestObj).Code;

        }

        //checking bookingId dont get populated on no room availability
        [TestCase(0, false)]
        [TestCase(55, true)]
        public void StudyRoomBooking_BookRoomWithAvailability_ReturnsBookingId(int expectedRoomId,
            bool roomAvailability)
        {
            //arrange
            if (!roomAvailability)
            {
                _availableStudyRoomList.Clear();
            }
            
            _studyRoomBookingRepoMock.Setup(x => x.Book(It.IsAny<StudyRoomBooking>()))
                .Callback<StudyRoomBooking>(booking =>
                {
                    booking.BookingId = 55;
                });

            //act
            var result = _bookingService.BookStudyRoom(_roomBookingRequestObj);
            Assert.AreEqual(expectedRoomId, result.BookingId);
        }



        [Test]
        public void CheckingRoomBookingServiceFlow_NoRoomsAvailable_BookMethodNotCalled()
        {
            //arrange                       
           _availableStudyRoomList.Clear();
           _bookingService.BookStudyRoom(_roomBookingRequestObj);
           _studyRoomBookingRepoMock.Verify(x => x.Book(It.IsAny<StudyRoomBooking>()), Times.Never());           
        }
    }
}