using System;
using MLManager.Database;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace MLManager.Services
{
    public class CreateUserRequest
    {
        [Required]
        [MinLength(8)]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(54)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$")]
        public string Password { get; set; }

        [Required]
        [MinLength(1)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(1)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
    public interface IAuthenticationService : IDisposable, IAsyncDisposable
    {
        Task<bool> DoesUsernameExist(string username);
        Task<bool> DoesEmailExist(string email);
        Task<User> CreateUser(CreateUserRequest user);
        Task<JwtResponse> Refresh(int userId, Guid refreshToken, Guid? deviceId);
        Task<JwtResponse> Authenticate(string username,string password,Guid? deviceId);
    }
}