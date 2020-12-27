using Moq;
using Xunit;
using System;
using MLManager.Services;
using MLManager.Database;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace MLManager.Tests
{
    public class AuthenticationTests
    {
        public const string ConnectionString = "Server=127.0.0.1;Port=5432;Database=MLManagerTests;User Id=postgres;Password=Void$1988;Application Name=MLManager-Tests;Pooling=True;";
        //Should build a test database and server to work with it from.

        public AuthenticationTests()
        {
            using(MLManagerContext db = new MLManagerContext(ConnectionString))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }

        private async Task<User> TestCreateUser(CreateUserRequest request)
        {
            using(MLManagerContext dbContext = new MLManagerContext(ConnectionString))
            {
                IPasswordService passwordService = new PasswordService();
                IAuthenticationService authenticationService = new AuthenticationService(dbContext,null,passwordService);
                return await authenticationService.CreateUser(request);
            }            
        }

//         private async Task TestValidationAttribute<V>(CreateUserRequest request)
//         {
//             try
//             {
//                 await TestCreateUser(request);
//                 throw new Xunit.Sdk.XunitException($"TestCreateUser should not have succeeded. Expected validation exception error.");
//             }
//             catch(ValidationException ex)
//             {
//                 /* We got a lot more to do! */
//                 if(!(ex.ValidationAttribute is V))
//                 {
//                     throw new Xunit.Sdk.XunitException($"Validation attribute, {typeof(V).Name}, did not trigger as expected.");
//                 }
//             }            
//         }

        private CreateUserRequest MakeUserRequest()
        {
            return new CreateUserRequest
            {
                FirstName = "First",
                LastName = "Last",
                Username = "TestUsername5555",
                Password = "Test$7373",
                Email = "TestUsername5555@gmail.com",
                PhoneNumber = "5553332222"
            };
        }

        [Fact]
        public async Task CreateUserSuccess()
        {
            CreateUserRequest request = MakeUserRequest();
            User user = await TestCreateUser(request);

            Assert.NotNull(user);
            Assert.True(user.UserId >= 1,"User Id from created user does not match an expected value.");
            Assert.Equal(user.Username,request.Username);
            Assert.True(new PasswordService().CheckPassword(request.Password,user.PasswordHash));

            Assert.Equal(user.FirstName,request.FirstName);
            Assert.Equal(user.LastName,request.LastName);
            Assert.Equal(user.PhoneNumber,request.PhoneNumber);
            Assert.Equal(user.Email,request.Email);
            Assert.Null(user.VerifiedEmailTimestamp);
            Assert.True((user.RegistrationTimestamp - DateTime.UtcNow).TotalMinutes <= 5,"RegistrationTimestamp is not close to the current UTC Time.");
        }

//         [Fact]
//         public async Task Authenticate()
//         {
//             using(MLManagerContext ctx = new MLManagerContext(ConnectionString))
//             {
//                 IPasswordService passwordService = new PasswordService();
//                 IAuthenticationService authenticationService = new AuthenticationService(ctx,passwordService);

//                 string username = "TestUser9999";
//                 string password = "Test@7777";
//                 Assert.Null(await authenticationService.Authenticate(username,password));
//                 CreateUserRequest request = MakeUserRequest();
//                 request.Username = username;
//                 request.Email = $"{username}@gmail.com";
//                 request.Password = password;

//                 await authenticationService.CreateUser(request);
//                 User user = await authenticationService.Authenticate(username,password);
//                 Assert.NotNull(user);
//                 Assert.Equal(user.Username,username);
//                 Assert.True(passwordService.CheckPassword(password,user.PasswordHash));

//                 Assert.Null(await authenticationService.Authenticate(username,"NotTheSamePassword"));
//             }
//         }

//         [Fact]
//         public async Task CheckForDuplicateUsername()
//         {
//             using(MLManagerContext dbContext = new MLManagerContext(ConnectionString))
//             {
//                 IPasswordService passwordService = new PasswordService();
//                 IAuthenticationService authenticationService = new AuthenticationService(dbContext,passwordService);
                
//                 const string testUsername = "TestUsername2323";
//                 string testEmail = $"{testUsername}@gmail.com";
//                 Assert.False(await authenticationService.DoesUsernameExist(testUsername),"Username should not already exist. Either the method is faulty or the test structure is faulty.");
                
//                 CreateUserRequest request = MakeUserRequest();
//                 request.Username = testUsername;
//                 request.Email = testEmail;

//                 await authenticationService.CreateUser(request);
//                 Assert.True(await authenticationService.DoesUsernameExist(testUsername),"Username should exist. Method is faulty.");
//             }
//         }

//         [Fact]
//         public async Task CheckForDuplicateEmail()
//         {
//             using(MLManagerContext dbContext = new MLManagerContext(ConnectionString))
//             {
//                 IPasswordService passwordService = new PasswordService();
//                 IAuthenticationService authenticationService = new AuthenticationService(dbContext,passwordService);
                
//                 const string testUsername = "TestUsername4646";
//                 string testEmail = $"{testUsername}@gmail.com";
//                 Assert.False(await authenticationService.DoesEmailExist(testEmail),"Email should not already exist. Either the method is faulty or the test structure is faulty.");
                
//                 CreateUserRequest request = MakeUserRequest();
//                 request.Username = testUsername;
//                 request.Email = testEmail;

//                 await authenticationService.CreateUser(request);
//                 Assert.True(await authenticationService.DoesEmailExist(testEmail),"Email should exist. Method is faulty.");
//             }            
//         }

//         [Fact]
//         public async Task CreateUserShortPassword()
//         {
//             CreateUserRequest request = MakeUserRequest();
//             request.Password = "Aa0";

//             await TestValidationAttribute<MinLengthAttribute>(request);
//         }

//         [Fact]
//         public async Task CreateUserLongPassword()
//         {
//             CreateUserRequest request = MakeUserRequest();
//             request.Password = "dd40nenh0f80yQOvkaKur14h9k7fuLBlmsN4MHvdGabKANgbTOQBtj8044OT";
//             await TestValidationAttribute<MaxLengthAttribute>(request);            
//         }
    }
}
