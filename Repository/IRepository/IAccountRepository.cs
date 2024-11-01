namespace BookingMovies.Repository.IRepository
{
    public interface IAccountRepository
    {
        string CreateUserName(string FirstName, string LastName);
        bool CheckUniqueEmail(string email);
    }
}
