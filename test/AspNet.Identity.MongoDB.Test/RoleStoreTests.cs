using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AspNet.Identity.MongoDB.Test {

	[TestClass]
	public class RoleStoreTests {
		private RoleManager<IdentityRole> rm;

		[TestInitialize]
		public void Init() {
			this.rm = Common.InitializeRoleManager();
		}

		[TestMethod]
		public void CreateRole() {
			IdentityResult result = this.rm.Create(new IdentityRole {
				Name = "first one"
			});

			result.Succeeded.Should().Be(true);
		}

		[TestMethod]
		public void CreateRoleCasing() {
			IdentityResult result = this.rm.Create(new IdentityRole {
				Name = "Another One"
			});

			IdentityRole role = this.rm.FindByName("another one");

			false.Should().Be(role == null);
		}

		[TestMethod]
		public void CreateRoleNotFound() {
			IdentityResult result = this.rm.Create(new IdentityRole {
				Name = "Ghost"
			});

			IdentityRole role = this.rm.FindByName("not ghost");

			true.Should().Be(role == null);
		}
	}
}
