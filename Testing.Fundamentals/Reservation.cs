namespace Testing.Fundamentals
{
    public class Reservation
    {
        public AppUser Reservationist { get; set; }
        public bool CanBeCancelledBy(AppUser user)
        {
            if (user.Role == AppUserRole.Admin || Reservationist == user) 
                return true;

            return false;
        }
    }
}
