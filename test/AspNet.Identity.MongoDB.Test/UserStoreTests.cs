using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AspNet.Identity.MongoDB.Test {

	[TestClass]
	public class UserStoreTests {
		private UserManager<IdentityUser> um;

		[TestInitialize]
		public void Init() {
			this.um = new UserManager<IdentityUser>(new UserStore<IdentityUser>("mongodb://localhost/test"));
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
	}
}
