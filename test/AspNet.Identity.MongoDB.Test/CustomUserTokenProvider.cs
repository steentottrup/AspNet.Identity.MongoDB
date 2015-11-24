using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace AspNet.Identity.MongoDB.Test {

	public class CustomUserTokenProvider : IUserTokenProvider<IdentityUser, String> {
		private const String hardcodedToken = "MyCustomToken";

		public async Task<String> GenerateAsync(String purpose, UserManager<IdentityUser, String> manager, IdentityUser user) {
			return await Task.FromResult(hardcodedToken);
		}

		public async Task<Boolean> IsValidProviderForUserAsync(UserManager<IdentityUser, String> manager, IdentityUser user) {
			return await Task.FromResult(true);
		}

		public async Task NotifyAsync(String token, UserManager<IdentityUser, String> manager, IdentityUser user) {
		}

		public async Task<Boolean> ValidateAsync(String purpose, String token, UserManager<IdentityUser, String> manager, IdentityUser user) {
			return await Task.FromResult(token == hardcodedToken);
		}
	}
}
