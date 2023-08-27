using Bongo.Core.Services.IServices;
using Bongo.DataAccess.Repository.IRepository;
using Bongo.Models.Model;
using Bongo.Models.Model.VM;
using Bongo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bongo.Web.Tests
{
    [TestFixture]
    public class RoomBookingControllerTests
    {
        private RoomBookingController _controllerObj;
        private Mock<IStudyRoomBookingService> _studyRoomBookingService;


        [SetUp]
        public void SetUp()
        {
            _studyRoomBookingService = new Mock<IStudyRoomBookingService>();
            _controllerObj = new RoomBookingController(_studyRoomBookingService.Object);
        }

        [Test]
        public void IndexPage_CallRequest_VerifyGetAllInvoked()
        {
            _controllerObj.Index();
            _studyRoomBookingService.Verify(x => x.GetAllBooking(), Times.Once);
        }


        [Test]
        public void BookRoomCheck_ModelStateInvalid_ReturnView()
        {
            _controllerObj.ModelState.AddModelError("test", "test");
            var result = _controllerObj.Book(new StudyRoomBooking());
            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual("Book", viewResult.ViewName);

        }

        [Test]
        public void BookRoomCheck_NotSuccesful_NoRoomStatusCode()
        {
            _studyRoomBookingService.Setup(x => x.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
                .Returns(new StudyRoomBookingResult()
                {
                    Code = StudyRoomBookingCode.NoRoomAvailable
                });
            var result = _controllerObj.Book(new StudyRoomBooking());
            Assert.IsInstanceOf<ViewResult>(result);
            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual("No Study Room available for selected date", viewResult.ViewData["Error"]);
        }


        [Test]
        public void BookRoomCheck_Succesful_SuccessCodeAndRedirect()
        {
            //incase of succesfull,
            //1. we need to check that the return type is RedirecToAction

            //2. we need to check the result code is Succesfull. So, we need to extract this one
            //property from the result(In controller class) and see if it was the same as
            //what was being passed inside the booking

            //Humare controller ke andar Book API me BookStudyRoom call ho ra hai.
            //Below humne usko iss tarah mock karke setup kiya hai ki vo return me
            //Successfullt book karke de hame room.
            //Jab succesful hoga tabhi if(result.code=success) loop me enter karega aur
            //then redirect to Action karega
            //arrange
            _studyRoomBookingService.Setup(x => x.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
                .Returns((StudyRoomBooking booking)=> new StudyRoomBookingResult()
                {
                    Code = StudyRoomBookingCode.Success,
                    FirstName = booking.FirstName, 
                    LastName = booking.LastName,
                    Date= DateTime.Now,
                    Email = booking.Email
                });


            //act
            var result = _controllerObj.Book(new StudyRoomBooking()
            {
                Date = DateTime.Now,
                Email = "Shantanu@gmail.com",
                FirstName = "Shantanu",
                LastName = "Gupta",
                StudyRoomId = 1
            });

            //Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            RedirectToActionResult viewResult = result as RedirectToActionResult;
            Assert.AreEqual("Shantanu", viewResult.RouteValues["FirstName"]);
        }



    }
}
