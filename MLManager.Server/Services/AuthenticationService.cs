using System;
using Dapper;
using System.Linq;
using MLManager.Database;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MLManager.Services
{
    //Should rename this the user service and make it able to handle permissions also
    public class AuthenticationService : IAuthenticationService
    {
        private readonly MLManagerContext _ctx;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;

        public AuthenticationService(MLManagerContext ctx,IJwtService jwtService,IPasswordService passwordService)
        {
            _ctx = ctx;
            _jwtService = jwtService;
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

        public async Task<JwtResponse> Refresh(int userId, Guid refreshToken, Guid? deviceId)
        {
            var reader = await _ctx.Database.GetDbConnection().QueryMultipleAsync(@"
                UPDATE public.jwt_securities
                SET last_updated_timestamp = timezone('utc',now()), refresh_token = uuid_generate_v4()
                WHERE device_id = @deviceId AND user_id = @user_id AND refresh_token = @refreshToken
                RETURNING *;

                SELECT * FROM public.users
                WHERE user_id = @userId;
            ",new
            {
                deviceId,
                refreshToken,
                userId
            });

            JwtSecurity security = reader.Read<JwtSecurity>().FirstOrDefault();
            User user = reader.Read<User>().FirstOrDefault();

            if(security == null || user == null)
            {
                return null;
            }

            var jwtResult = _jwtService.CreateJwt(user);

            return new JwtResponse
            {
                Token = jwtResult.AccessToken,
                RefreshToken = security.RefreshToken,
                Expiration = jwtResult.ExpirationTimestamp
            };
        }

        public async Task<JwtResponse> Authenticate(string username,string password,Guid? deviceId) //This should take a deviceId also ...
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

            var jwtResult = _jwtService.CreateJwt(user);
            if(deviceId == null || deviceId == Guid.Empty)
            {
                deviceId = Guid.NewGuid();
            }

            var security = await _ctx.Database.GetDbConnection().QueryFirstAsync<JwtSecurity>(@"
                INSERT INTO public.jwt_securities
                (device_id,user_id,refresh_token)
                VALUES 
                (@deviceId,@userId,uuid_generate_v4()) 
                ON CONFLICT (device_id,user_id) 
                DO
                UPDATE SET refresh_token = uuid_generate_v4(), last_updated_timestamp = timezone('utc',now())
                RETURNING *;
            ",new
            {
                deviceId,
                userId = user.UserId
            });

            return new JwtResponse
            {
                Token = jwtResult.AccessToken,
                RefreshToken = security.RefreshToken,
                Expiration = jwtResult.ExpirationTimestamp
            };
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