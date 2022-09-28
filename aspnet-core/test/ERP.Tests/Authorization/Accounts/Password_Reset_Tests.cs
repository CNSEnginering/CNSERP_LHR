using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Identity;
using ERP.Authorization.Accounts;
using ERP.Authorization.Accounts.Dto;
using ERP.Authorization.Users;
using ERP.Test.Base;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ERP.Tests.Authorization.Accounts
{
    // ReSharper disable once InconsistentNaming
    public class Password_Reset_Tests : AppTestBase
    {
        [Fact]
        public async Task Should_Reset_Password()
        {
            //Arrange

            var user = await GetCurrentUserAsync();

            string passResetCode = null;

            var fakeUserEmailer = Substitute.For<IUserEmailer>();
            var localUser = user;
            fakeUserEmailer.SendPasswordResetLinkAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(callInfo =>
            {
                var calledUser = callInfo.Arg<User>();
                calledUser.EmailAddress.ShouldBe(localUser.EmailAddress);
                passResetCode = calledUser.PasswordResetCode; //Getting the password reset code sent to the email address
                return Task.CompletedTask;
            });

            LocalIocManager.IocContainer.Register(Component.For<IUserEmailer>().Instance(fakeUserEmailer).IsDefault());
            
            var accountAppService = Resolve<IAccountAppService>();

            //Act

            await accountAppService.SendPasswordResetCode(
                new SendPasswordResetCodeInput
                {
                    EmailAddress = user.EmailAddress
                }
            );

            await accountAppService.ResetPassword(
                new ResetPasswordInput
                {
                    Password = "New@Passw0rd",
                    ResetCode = passResetCode,
                    UserId = user.Id
                }
            );

            //Assert

            user = await GetCurrentUserAsync();
            LocalIocManager
                .Resolve<IPasswordHasher<User>>()
                .VerifyHashedPassword(user, user.Password, "New@Passw0rd")
                .ShouldBe(PasswordVerificationResult.Success);
        }
    }
}