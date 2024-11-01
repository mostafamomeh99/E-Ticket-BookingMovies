using BookingMovies.Data;
using BookingMovies.Repository.IRepository;

namespace BookingMovies.Repository
{
    public class AccountRepositry : IAccountRepository
    {
        private readonly ApplicationDbContext dbContext;

        public AccountRepositry(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
       
     public string CreateUserName(string FirstName, string LastName)
        {
            string UserName= FirstName+LastName+ Guid.NewGuid().ToString("N");

            return UserName;

        }
        public bool CheckUniqueEmail(string email)
        {
            var ExistingEmail = dbContext.Users.Where(e => e.Email == email).FirstOrDefault();

            if (ExistingEmail != null) {
                return false;
            }
            return true;
        }



    }
}
