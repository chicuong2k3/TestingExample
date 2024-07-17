namespace Testing.Fundamentals
{
    public class AppUser
    {
        public AppUserRole Role { get; set; } = AppUserRole.None;
    }

    public enum AppUserRole
    {
        None,
        Admin,
        Customer
    }
}
