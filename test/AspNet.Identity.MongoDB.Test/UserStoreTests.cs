using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System;

namespace AspNet.Identity.MongoDB.Test {

	[TestClass]
	public class UserStoreTests {
		private UserManager<IdentityUser> um;

		[TestInitialize]
		public void Init() {
			this.um = Common.InitializeWithToken();
		}

		[TestMethod]
		public void CreateAndFindByEmail() {
			this.um.Create(new IdentityUser {
				EmailAddress = "test@test.com",
				UserName = "Test"
			});

			IdentityUser user = this.um.FindByEmail("test@test.com");

			user.UserName.Should().Be("Test");
		}

		[TestMethod]
		public void CreateAndFindByUserName() {
			this.um.Create(new IdentityUser {
				EmailAddress = "test2@test.com",
				UserName = "MrTest"
			});

			IdentityUser user = this.um.FindByName("mrtest");

			user.UserName.Should().Be("MrTest");
		}

		[TestMethod]
		public void ConfirmEmailAddressSuccess() {
			this.um.Create(new IdentityUser {
				EmailAddress = "test3@test.com",
				UserName = "MrTest3"
			});

			IdentityUser user = this.um.FindByName("mrtest3");

			user.EmailAddressConfirmed.Should().Be(false);

			String token = this.um.GenerateEmailConfirmationToken(user.Id);

			IdentityResult result = this.um.ConfirmEmail(user.Id, token);

			result.Succeeded.Should().Be(true);

			user = this.um.FindByName("mrtest3");

			user.EmailAddressConfirmed.Should().Be(true);
		}

		[TestMethod]
		public void FindByLogin() {
			this.um.Create(new IdentityUser {
				EmailAddress = "test5@test.com",
				UserName = "MrTest5"
			});

			IdentityUser user = this.um.Find(new UserLoginInfo("Twitter", "ewrthyjkk"));
		}

		[TestMethod]
		public void AddLogin() {
			this.um.Create(new IdentityUser {
				EmailAddress = "test6@test.com",
				UserName = "MrTest6"
			});

			IdentityUser user = this.um.FindByEmail("test6@test.com");

			this.um.AddLogin(user.Id, new UserLoginInfo("Twitter", "blablabla"));
		}

		[TestMethod]
		public void ConfirmEmailAddressFailure() {
			this.um.Create(new IdentityUser {
				EmailAddress = "test4@test.com",
				UserName = "MrTest4"
			});

			IdentityUser user = this.um.FindByName("mrtest4");

			user.EmailAddressConfirmed.Should().Be(false);

			String token = this.um.GenerateEmailConfirmationToken(user.Id);

			IdentityResult result = this.um.ConfirmEmail(user.Id, token + "gerger");

			result.Succeeded.Should().Be(false);

			user = this.um.FindByName("mrtest4");

			user.EmailAddressConfirmed.Should().Be(false);
		}
	}
}
