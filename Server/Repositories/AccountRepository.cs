using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class AccountRepository : GeneralRepository<Account>, IAccountRepository
{
    public AccountRepository(LeaveDbContext context) : base(context)
    {
    }
}