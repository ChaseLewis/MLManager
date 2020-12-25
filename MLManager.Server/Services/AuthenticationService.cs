using Dapper;
using System.Linq;
using MLManager.Database;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MLManager.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly MLManagerContext _ctx;
        private readonly IPasswordService _passwordService;

        public AuthenticationService(MLManagerContext ctx,IPasswordService passwordService)
        {
            _ctx = ctx;
            _passwordService = passwordService;
        }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return _ctx.DisposeAsync();
        }   

        public async Task<User> Authenticate(string username,string password)
        {
            User user = await _ctx.Users.Where(x => x.Username == username).AsNoTracking().FirstOrDefaultAsync();

            if(user == null)
            {
                //Should log that an inappropriate username attempt occurred
                return null;
            }

            if(!_passwordService.CheckPassword(password,user.PasswordHash))
            {
                //Should log that an inappropriate password attempt occurred for an account
                return null;
            }

            return user;
        }

        public async Task<bool> DoesUsernameExist(string username)
        {
            return await _ctx.Users.AnyAsync(x => x.Username == username);
        }

        public async Task<bool> DoesEmailExist(string email)
        {
            return await _ctx.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<User> CreateUser(CreateUserRequest userRequest)
        {
            //Make sure that our user being created meets all our validation requirements
            Validator.ValidateObject(userRequest,new ValidationContext(userRequest),true);

            var user = new User
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Username = userRequest.Username,
                PasswordHash = _passwordService.HashPassword(userRequest.Password),
                Email = userRequest.Email,
                PhoneNumber = userRequest.PhoneNumber
            };

            var userEntity = _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();
            userEntity.State = EntityState.Detached;

            return user;
        }
    }
}