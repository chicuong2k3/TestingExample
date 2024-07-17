using NUnit.Framework;
using Testing.Fundamentals;
using Assert = NUnit.Framework.Assert;

namespace Testing.UnitTests
{
    [TestFixture]
    public class ReservationTests
    {
        [Test]
        // the first part: the name of the method need to be tested
        // the second part: the scenario
        // the third part: the expected behaviour
        public void CanBeCancelledBy_UserIsAdmin_ReturnsTrue()
        {
            // Arrange
            var user = new AppUser();
            user.Role = AppUserRole.Admin;

            var reservation = new Reservation();

            // Act
            var result = reservation.CanBeCancelledBy(user);

            // Assert
            //Assert.IsTrue(result);
            Assert.That(result, Is.True);
        }

        [Test]
        public void CanBeCancelledBy_UserIsReservationist_ReturnsTrue()
        {
            // Arrange
            var user1 = new AppUser();
            var user2 = new AppUser();
            var reservation = new Reservation()
            {
                Reservationist = user1
            };

            // Act
            var result = reservation.CanBeCancelledBy(user1);

            // Assert
            Assert.That(result, Is.True);

        }

        [Test]
        public void CanBeCancelledBy_AnotherUserCancelReservation_ReturnsFalse()
        {
            // Arrange
            var user1 = new AppUser();
            var user2 = new AppUser();
            var reservation = new Reservation()
            {
                Reservationist = user1
            };

            // Act;
            var result = reservation.CanBeCancelledBy(user2);

            // Assert
            Assert.That(result, Is.False);  

        }
    }
}