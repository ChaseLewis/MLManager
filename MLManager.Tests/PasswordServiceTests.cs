using System;
using Xunit;
using MLManager.Services;

namespace MLManager.Tests
{
    public class PasswordServiceTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("0000")]
        [InlineData("TestPassword")]
        [InlineData("123VSDFS%%%HOWUIU")]
        public void VerifyHashedPassword(string password)
        {
            PasswordService service = new PasswordService();
            string hash = service.HashPassword(password);

            Assert.NotNull(hash);
            Assert.True(hash.Length == 60,$"Password hash length is expected to be 60 no matter the input length. Password: {password}");
            Assert.True(service.CheckPassword(password,hash),$"Verify password did not return true when expected. Password: {password}");
        }

        [Fact]        
        public void HashPasswordNullInput()
        {
            PasswordService service = new PasswordService();
            Assert.Throws<ArgumentNullException>("password",() => service.HashPassword(null));
        }

        [Fact]
        public void VerifyPasswordNullInput()
        {
            PasswordService service = new PasswordService();
            Assert.Throws<ArgumentNullException>(() => service.CheckPassword(null,null));
            Assert.Throws<ArgumentNullException>("password",() => service.CheckPassword(null,"TestHash"));
            Assert.Throws<ArgumentNullException>("passwordHash",() => service.CheckPassword("TestPassword",null));         
        }

        [Theory]
        [InlineData("THISISMYPASSWORD","THISISNOTMYPASSWORD")]
        public void TestPasswordMismatch(string password,string verification)
        {
            PasswordService service = new PasswordService();
            string hash = service.HashPassword(password);
            Assert.False(service.CheckPassword(verification,hash),"Verification should have failed.");
        }
    }
}
