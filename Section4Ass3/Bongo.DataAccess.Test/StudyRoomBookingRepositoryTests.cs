using Bongo.Models.Model;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Bongo.DataAccess.Repository;
using System.Collections;

namespace Bongo.DataAccess.Test
{
    [TestFixture]
    public class StudyRoomBookingRepositoryTests
    {
        private StudyRoomBooking obj1 = new StudyRoomBooking();
        private StudyRoomBooking obj2 = new StudyRoomBooking();
        private DbContextOptions<ApplicationDbContext> options;
        
        public StudyRoomBookingRepositoryTests()
        {
            obj1 = new StudyRoomBooking
            {
                FirstName = "Ram",
                LastName = "Gupta",
                Email = "ram@gmail.com",
                Date = DateTime.Now,
                BookingId = 10,
                StudyRoomId = 5
            };
            
            obj2 = new StudyRoomBooking
            {
                FirstName = "Shyam",
                LastName = "Gupta",
                Email = "Shyam@gmail.com",
                Date = DateTime.Now,
                BookingId = 11,
                StudyRoomId = 6
            };
        }

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "temp_Bongo").Options;
        }

        [Test]
        [Order(1)]
        public void SaveBooking_Booking_One_CheckTheValuesFromDatabase()
        {
            //arrange           

            //act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new StudyRoomBookingRepository(context);
                //Sending data to DB
                repository.Book(obj1);
            }


            //assert
            //Checking whether sent data is presnt in DB or not
            using (var context = new ApplicationDbContext(options))
            {
                var bookingFromDB = context.StudyRoomBookings.FirstOrDefault(u => u.BookingId == 10);
                Assert.AreEqual(obj1.BookingId, bookingFromDB.BookingId);
                Assert.AreEqual(obj1.FirstName, bookingFromDB.FirstName);
                Assert.AreEqual(obj1.LastName, bookingFromDB.LastName);
                Assert.AreEqual(obj1.StudyRoomId, bookingFromDB.StudyRoomId);
            }
        }


        [Test]
        [Order(2)]
        public void GetAllBookings_AllBookings_ReturnBookings()
        {
            //arrange- make the arrangements for the expected results
            var expectedResult = new List<StudyRoomBooking> { obj1, obj2 };
            


            //Adding both the books so that we can Assert/compare them in act phase
            using(var context = new ApplicationDbContext(options))
            {
                //In the first unit test, we have already added obj1 in our database,
                //And adding it again below will give us error as it is already present in it.
                //So, for that firstly, we have given the order of running of our tests on top
                //of methods and then we have given below statement to make sure, before adding
                //our bookings to our DB again, our dtabase is empty
                context.Database.EnsureDeleted();

                var repository = new StudyRoomBookingRepository(context);
                //Sending data to DB
                repository.Book(obj1);
                repository.Book(obj2);
            }

            //act
            List<StudyRoomBooking> actualList;
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new StudyRoomBookingRepository(context);
                actualList = repository.GetAll(null).ToList();
            }

            //assert
            CollectionAssert.AreEqual(expectedResult, actualList, new BookingCompare());
        }


        //IComparer is used to compare 2 objects
        private class BookingCompare : IComparer
        {
            public int Compare(object? x, object? y)
            {
                var booking1 = (StudyRoomBooking)x;
                var booking2 = (StudyRoomBooking)y;
                //if the bookings dont match, return 1
                if(booking1.BookingId != booking2.BookingId)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }


    }


    
}
