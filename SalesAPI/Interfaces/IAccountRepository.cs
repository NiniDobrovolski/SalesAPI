using SalesAPI.Models;

namespace SalesAPI.Interfaces
{
    public interface IAccountRepository
    {
        Task Register(Account account);
        Account LogIn(int accountNumber);
        void Update(string username, Account account);
        void Delete(int accountNumber);
        Account FindByUsername(string username);
        Task SaveAsync();
    }
}

