using SalesAPI.Interfaces;
using SalesAPI.Models;
using SalesAPI.Data;


namespace SalesAPI.Repository
{
	public class AccountRepository : IAccountRepository
	{
		private readonly SalesDbContext _context;
        public AccountRepository(SalesDbContext context)
        {
            _context = context;
        }
        public async Task Register(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }
        public Account LogIn(int accountNumber)
		{
			return _context.Accounts.FirstOrDefault(i => i.AccountNumber == accountNumber);
		}
        public void Update(string username, Account account)
		{
			var acc = _context.Accounts.FirstOrDefault(i=>i.Username== username);
			acc = account;
			_context.SaveChanges();
		}
        public void Delete(int accountNumber)
		{
            var acc = _context.Accounts.FirstOrDefault(i => i.AccountNumber == accountNumber);
			_context.Remove(acc);
			_context.SaveChanges();
        }
		public Account FindByUsername(string username)
		{
            return _context.Accounts.FirstOrDefault(i => i.Username == username);

        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

