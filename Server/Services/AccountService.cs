using System.Security.Claims;
using Server.Contracts;
using Server.Data;
using Server.DTOs.AccountRoles;
using Server.DTOs.Accounts;
using Server.DTOs.Employees;
using Server.Models;
using Server.Repositories;
using Server.Utilities.Handler;

namespace Server.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmailHandler _emailHandler;
    private readonly ITokenHandler _tokenHandler;
    private readonly LeaveDbContext _dbContext;

    public AccountService(IAccountRepository accountRepository, 
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        IEmailHandler emailHandler, 
        ITokenHandler tokenHandler, 
        IAccountRoleRepository accountRoleRepository,
        LeaveDbContext dbContext)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _emailHandler = emailHandler;
        _tokenHandler = tokenHandler;
        _accountRoleRepository = accountRoleRepository;
        _dbContext = dbContext;
    }

    public IEnumerable<AccountDto> GetAll()
    {
        var accounts = _accountRepository.GetAll();
        if (!accounts.Any())
        {
            return Enumerable.Empty<AccountDto>();
        }

        var accountDtos = new List<AccountDto>();
        foreach (var account in accounts)
        {
            accountDtos.Add((AccountDto)account);
        }

        return accountDtos;
    }

    public AccountDto? GetByGuid(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null)
        {
            return null;
        }

        return (AccountDto)account;
    }

    public AccountDto? Create(AccountDto newAccountDto)
    {
        var account = _accountRepository.Create(newAccountDto);
        if (account is null)
        {
            return null;
        }

        return (AccountDto)account;
    }

    public int Update(AccountDto accountDto)
    {
        var account = _accountRepository.GetByGuid(accountDto.Guid);
        if (account is null)
        {
            return -1;
        }

        Account toUpdate = accountDto;
        toUpdate.CreatedDate = account.CreatedDate;
        var result = _accountRepository.Update(toUpdate);

        return result ? 1 : 0;
    }

    public int Delete(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);
        if (account is null)
        {
            return -1;
        }

        var result = _accountRepository.Delete(account);
        return result ? 1 : 0;
    }

    public string Login(LoginDto loginDto)
    {
        var employeeAccount = (from e in _employeeRepository.GetAll()
            join a in _accountRepository.GetAll() on e.Guid equals a.Guid
            where e.Email == loginDto.Email && HashingHandler.ValidateHash(loginDto.Password, a.Password)
            select new
            {
                Email = e.Email,
                Password = a.Password,
                IsActive = a.IsActive
            }).ToList();
        
        if (!employeeAccount.Any())
        {
            return "-1"; // Email or Password incorrect.      
        } 
        else if (!employeeAccount[0].IsActive)
        {
            return "-3"; // Account need to activate
        }

        var employee = _employeeRepository.GetByEmail(loginDto.Email);
        var claims = new List<Claim>
        {
            new Claim("Guid", employee.Guid.ToString()),
            new Claim("FullName", $"{employee.FirstName} {employee.LastName}"),
            new Claim("Email", employee.Email)
        };
        
        var getRoles = _accountRoleRepository.GetRoleNamesByAccountGuid(employee.Guid);
        foreach (var role in getRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var generateToken = _tokenHandler.GenerateToken(claims);
        if (generateToken is null)
        {
            return "-2";
        }
        return generateToken;                
    }

    public int Register(RegisterDto registerDto)
    {
        // ini untuk cek emaik sama phone number udah ada atau belum
        if (!_employeeRepository.IsNotExist(registerDto.Email) ||
            !_employeeRepository.IsNotExist(registerDto.PhoneNumber))
        {
            return -1; // kalau sudah ada, pendaftaran gagal.
        }
        
        var department = _departmentRepository.GetByCode(registerDto.DepartmentCode);
        if (department is null)
        {
            return -1; // Kalau department tidak ditemukan maka pendaftaran gagal
        }

        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            var newNik =
                GenerateHandler.Nik(_employeeRepository.GetLastNik()); //karena niknya generate, sebelumnya kalo ga dikasih ini niknya null jadi error
            var employeeGuid = Guid.NewGuid(); // Generate GUID baru untuk employee

            var manager = _employeeRepository.GetByNik(registerDto.ManagerNik);
            Guid? managerGuid = null; // Inisialisasi dengan null

            if (manager is not null)
            {
                managerGuid = manager.Guid; // Set managerGuid jika manager tidak null
            }

            var employee = _employeeRepository.Create(new EmployeeDto {
                Guid = employeeGuid,
                Nik = newNik,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                BirthDate = registerDto.BirthDate,
                Gender = registerDto.Gender,
                HiringDate = registerDto.HiringDate,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                DepartmentGuid = department.Guid,
                ManagerGuid = managerGuid, // Assign managerGuid to the EmployeeDto
                LastLeaveUpdate = registerDto.HiringDate
            });
            _accountRepository.Clear();
            var accounts = _accountRepository.GetAll();
            if (!accounts.Any())// Jika Account Role Kosong maka Inputan Register pertama akan menjadi Admin
            {
                var accountadmin = _accountRepository.Create(new AccountDto
                {
                    Guid = employeeGuid, // Gunakan employeeGuid
                    IsUsed = true,
                    Password = HashingHandler.GenerateHash(registerDto.Password),
                    ExpiredTime = null,
                    IsActive = true
                });
            }
            var account = _accountRepository.Create(new AccountDto {
                Guid = employeeGuid, // Gunakan employeeGuid
                ProfilPictureUrl = null,
                IsUsed = true,
                Password = HashingHandler.GenerateHash(registerDto.Password),
                ExpiredTime = null
            });
            _accountRepository.Clear();
            var accountrole = _accountRoleRepository.GetAll();
            if (!accountrole.Any())// Jika Account Role Kosong maka Inputan Register pertama akan menjadi Admin
            {
                var accountroleadmin = _accountRoleRepository.Create(new NewAccountRoleDto
                {
                    AccountGuid = employeeGuid,
                    RoleGuid = Guid.Parse("36350d33-42d7-4c63-a244-29b0a8d13bce") // Register pertama sebagai admin
                });
                transaction.Commit();
                return 1;
            }
            var accountRole = _accountRoleRepository.Create((new NewAccountRoleDto()
            {
                AccountGuid = employeeGuid,
                RoleGuid = Guid.Parse("4887ec13-b482-47b3-9b24-08db91a71770") // Register as employee
            }));

            if (employee is null || account is null || accountRole is null)
            {
                throw new Exception("Custom error message: One or more required entities are null.");
            }

<<<<<<< HEAD
            transaction.Commit();
=======
            var getAccount = _accountRepository.GetByGuid(isExist.Guid);
            if (getAccount.Otp != changePasswordDto.OTP)
            {
                return 0;
            }

            if (getAccount.IsUsed)
            {
                return 1;
            }

            if (getAccount.ExpiredTime < DateTime.Now)
            {
                return 2;
            }

            var account = new Account
            {
                Guid = getAccount.Guid,
                IsUsed = true,
                ModifiedDate = DateTime.Now,
                CreatedDate = getAccount.CreatedDate,
                Otp = getAccount.Otp,
                ExpiredTime = getAccount.ExpiredTime,
                Password = HashingHandler.GenerateHash(changePasswordDto.NewPassword)
            };

            var isUpdated = _accountRepository.Update(account);
            if (!isUpdated)
            {
                return 0; //Account Not Update
            }

            return 3;
        }
        public int ForgotPassword(ForgotPasswordDto forgotPassword)
        {
            var getAccountDetail = (from e in _employeeRepository.GetAll()
                                    join a in _accountRepository.GetAll() on e.Guid equals a.Guid
                                    where e.Email == forgotPassword.Email
                                    select a).FirstOrDefault();

            _accountRepository.Clear();
            if (getAccountDetail is null)
            {
                return 0;
            }

            var otp = new Random().Next(111111, 999999);
            var account = new Account
            {
                Guid = getAccountDetail.Guid,
                Password = HashingHandler.GenerateHash(getAccountDetail.Password),
                ExpiredTime = DateTime.Now.AddMinutes(5),
                Otp = otp,
                IsUsed = false,
                CreatedDate = getAccountDetail.CreatedDate,
                ModifiedDate = DateTime.Now
            };

            var isUpdated = _accountRepository.Update(account);

            if (!isUpdated)
                return -1;
            
            _emailHandler.SendEmail(forgotPassword.Email,"Leave Request - Forgot Password OTP", $"Your OTP is {otp}");
            
>>>>>>> main
            return 1;
        }
        catch
        {
            transaction.Rollback();
            return -1;
        }
    }

    public int ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var isExist = _employeeRepository.CheckEmail(changePasswordDto.Email);
        if (isExist is null)
        {
            return -1; //Account not found
        }

        var getAccount = _accountRepository.GetByGuid(isExist.Guid);
        if (getAccount.Otp != changePasswordDto.OTP)
        {
            return 0;
        }

        if (getAccount.IsUsed)
        {
            return 1;
        }

        if (getAccount.ExpiredTime < DateTime.Now)
        {
            return 2;
        }

        var account = new Account
        {
            Guid = getAccount.Guid,
            IsUsed = true,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccount.CreatedDate,
            Otp = getAccount.Otp,
            ExpiredTime = getAccount.ExpiredTime,
            Password = HashingHandler.GenerateHash(changePasswordDto.NewPassword)
        };

        var isUpdated = _accountRepository.Update(account);
        if (!isUpdated)
        {
            return 0; //Account Not Update
        }

        return 3;
    }
    public int ForgotPassword(ForgotPasswordDto forgotPassword)
    {
        var getAccountDetail = (from e in _employeeRepository.GetAll()
                                join a in _accountRepository.GetAll() on e.Guid equals a.Guid
                                where e.Email == forgotPassword.Email
                                select a).FirstOrDefault();

        _accountRepository.Clear();
        if (getAccountDetail is null)
        {
            return 0;
        }

        var otp = new Random().Next(111111, 999999);
        var account = new Account
        {
            Guid = getAccountDetail.Guid,
            Password = HashingHandler.GenerateHash(getAccountDetail.Password),
            ExpiredTime = DateTime.Now.AddMinutes(5),
            Otp = otp,
            IsUsed = false,
            CreatedDate = getAccountDetail.CreatedDate,
            ModifiedDate = DateTime.Now
        };

        var isUpdated = _accountRepository.Update(account);

        if (!isUpdated)
            return -1;
        
        _emailHandler.SendEmail(forgotPassword.Email,"Booking - Forgot Password OTP", $"Your OTP is {otp}");
        
        return 1;
    }

    public IEnumerable<AccountDetailDto> GetDetailAll()
    {
        var getAccountDetail = from e in _employeeRepository.GetAll()
            join a in _accountRepository.GetAll() on e.Guid equals a.Guid
            select new AccountDetailDto()
            {
                Guid = e.Guid,
                Name = $"{e.Nik} - {e.FirstName} {e.LastName}",
                BirthDate = e.BirthDate.Date,
                Gender = e.Gender,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                IsActive = a.IsActive
            };
        var accountDetailDtos = getAccountDetail.ToList();
        if (!accountDetailDtos.Any())
        {
            return Enumerable.Empty<AccountDetailDto>();
        }
        
        return accountDetailDtos;
    }
}