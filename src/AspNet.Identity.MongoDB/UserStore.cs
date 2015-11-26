using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNet.Identity.MongoDB {

	public class UserStore<TUser> :
			IUserLoginStore<TUser>,
			IUserClaimStore<TUser>,
			IUserRoleStore<TUser>,
			IUserPasswordStore<TUser>,
			IUserSecurityStampStore<TUser>,
			IQueryableUserStore<TUser>,
			IUserEmailStore<TUser>,
			IUserPhoneNumberStore<TUser>,
			IUserTwoFactorStore<TUser, String>,
			IUserLockoutStore<TUser, String>,
			IUserStore<TUser>
			where TUser : IdentityUser {

		private readonly IMongoDatabase database;
		private readonly IMongoCollection<TUser> collection;
		private readonly IMongoCollection<IdentityRole> roleCollection;
		private Boolean disposed;

		public UserStore(String connectionNameOrUrl) {
			this.database = MongoDBUtilities.GetDatabase(connectionNameOrUrl);
			this.collection = database.GetCollection<TUser>(MongoDBUtilities.GetUserCollectionName());
			this.roleCollection = database.GetCollection<IdentityRole>(MongoDBUtilities.GetRoleCollectionName());
			this.disposed = false;
		}

		public static void Initialize(String connectionNameOrUrl) {
			IMongoCollection<TUser> collection = MongoDBUtilities.GetDatabase(connectionNameOrUrl).GetCollection<TUser>(MongoDBUtilities.GetUserCollectionName());
			EnsureIndexes.UserStore<TUser>(collection);
		}

		public async Task CreateAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			// TODO: ??
			user.LowerCaseEmailAddress = user.EmailAddress.ToLowerInvariant();
			user.LowerCaseUserName = user.UserName.ToLowerInvariant();
			user.AccessFailedCount = 0;
			user.EmailAddressConfirmed = false;
			user.LockoutEnabled = false;
			user.PhoneNumberConfirmed = false;
			user.TwoFactorEnabled = false;

			await this.collection.InsertOneAsync(user);
		}

		public async Task DeleteAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}
			// TODO: Delete or mark deleted?
			await this.collection.DeleteOneAsync(u => u.Id == user.Id);
		}

		public async Task<TUser> FindByIdAsync(String userId) {
			if (String.IsNullOrWhiteSpace(userId)) {
				throw new ArgumentNullException("userId");
			}
			return await this.collection.Find<TUser>(u => u.Id == userId)
				.FirstOrDefaultAsync();
		}

		public async Task<TUser> FindByNameAsync(String userName) {
			if (String.IsNullOrWhiteSpace(userName)) {
				throw new ArgumentNullException("userName");
			}
			String lowerCaseName = userName.ToLowerInvariant();
			return await this.collection.Find<TUser>(u => u.LowerCaseUserName == lowerCaseName)
				.FirstOrDefaultAsync();
		}

		public async Task UpdateAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			// Let's make sure the "system" fields are correct, we use these for searching
			user.LowerCaseEmailAddress = user.EmailAddress.ToLowerInvariant();
			// Let's make sure the "system" fields are correct, we use these for searching
			user.LowerCaseUserName = user.UserName.ToLowerInvariant();

			FilterDefinition<TUser> filter = Builders<TUser>
				.Filter
				.Eq(u => u.Id, user.Id);
			ReplaceOneResult result = await this.collection.ReplaceOneAsync(filter, user);
			// TODO: ???
		}

		public void Dispose() {
			if (!this.disposed) {
				this.disposed = true;
			}
		}

		public async Task<Int32> GetAccessFailedCountAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			return await Task.FromResult(user.AccessFailedCount);
		}

		public async Task<Boolean> GetLockoutEnabledAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			return await Task.FromResult(user.LockoutEnabled);
		}

		public async Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			return await
				Task.FromResult(user.LockoutEndDateUtc.HasValue
					? user.LockoutEndDateUtc.Value
					: new DateTimeOffset());
		}

		public async Task<Int32> IncrementAccessFailedCountAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			UpdateDefinition<TUser> update = Builders<TUser>
				.Update
				.Inc(u => u.AccessFailedCount, 1);
			await this.UpdateUserAsync(user, update);
			TUser temp = await this.FindByIdAsync(user.Id);
			return await Task.FromResult(temp.AccessFailedCount);
		}

		public Task ResetAccessFailedCountAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			user.AccessFailedCount = 0;
			return Task.FromResult(0);
		}

		public Task SetLockoutEnabledAsync(TUser user, Boolean enabled) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			user.LockoutEnabled = enabled;
			return Task.FromResult(0);
		}

		private async Task UpdateUserAsync(TUser user, UpdateDefinition<TUser> update) {
			FilterDefinition<TUser> filter = Builders<TUser>
				.Filter
				.Eq(u => u.Id, user.Id);
			await this.collection.UpdateOneAsync(filter, update);
		}

		public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			user.LockoutEndDateUtc = lockoutEnd;
			return Task.FromResult(0);
		}

		public async Task<Boolean> GetTwoFactorEnabledAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			return await Task.FromResult(user.TwoFactorEnabled);
		}

		public Task SetTwoFactorEnabledAsync(TUser user, Boolean enabled) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			user.TwoFactorEnabled = enabled;
			return Task.FromResult(0);
		}

		public async Task<String> GetPhoneNumberAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			return await Task.FromResult(user.PhoneNumber);
		}

		public async Task<Boolean> GetPhoneNumberConfirmedAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			return await Task.FromResult(user.PhoneNumberConfirmed);
		}

		public Task SetPhoneNumberAsync(TUser user, String phoneNumber) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			user.PhoneNumber = phoneNumber;
			return Task.FromResult(0);
		}

		public Task SetPhoneNumberConfirmedAsync(TUser user, Boolean confirmed) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			user.PhoneNumberConfirmed = confirmed;
			return Task.FromResult(0);
		}

		public async Task<TUser> FindByEmailAsync(String email) {
			if (String.IsNullOrWhiteSpace(email)) {
				throw new ArgumentNullException("email");
			}

			email = email.ToLowerInvariant();
			return await this.collection
				.Find(u => u.LowerCaseEmailAddress == email)
				.FirstOrDefaultAsync();
		}

		public async Task<String> GetEmailAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			return await Task.FromResult(user.EmailAddress);
		}

		public async Task<Boolean> GetEmailConfirmedAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			return await Task.FromResult(user.EmailAddressConfirmed);
		}

		public Task SetEmailAsync(TUser user, String email) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			user.EmailAddress = email;
			user.LowerCaseEmailAddress = email.ToLowerInvariant();
			return Task.FromResult(0);
		}

		public Task SetEmailConfirmedAsync(TUser user, Boolean confirmed) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			user.EmailAddressConfirmed = confirmed;
			return Task.FromResult(0);
		}

		public IQueryable<TUser> Users {
			get {
				return this.collection.AsQueryable();
			}
		}

		public async Task<String> GetSecurityStampAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			return await Task.FromResult(user.SecurityStamp);
		}

		public Task SetSecurityStampAsync(TUser user, String stamp) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			user.SecurityStamp = stamp;
			return Task.FromResult(0);
		}

		public async Task<String> GetPasswordHashAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			return await Task.FromResult(user.PasswordHash);
		}

		public async Task<Boolean> HasPasswordAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			return await Task.FromResult(!String.IsNullOrWhiteSpace(user.PasswordHash));
		}

		public Task SetPasswordHashAsync(TUser user, String passwordHash) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			user.PasswordHash = passwordHash;
			return Task.FromResult(0);
		}

		public async Task AddToRoleAsync(TUser user, String roleName) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			if (String.IsNullOrEmpty(roleName)) {
				throw new ArgumentException("roleName.");
			}

			IdentityRole role = await this.roleCollection.Find(r => r.Name == roleName).FirstOrDefaultAsync();
			if (role != null) {
				// Do the user already belong to this role??
				if (user.Roles == null || (user.Roles != null && !user.Roles.Any(r => r.RoleId == role.Id))) {
					// Nope, let's add him!
					if (user.Roles == null) {
						user.Roles = new List<IdentityUserRole>();
					}
					user.Roles.Add(new IdentityUserRole { RoleId = role.Id, Name = role.Name });
				}
			}
		}

		public async Task<IList<String>> GetRolesAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}
			// TODO: Validate against the roles collection??
			return await Task.FromResult(user.Roles == null ? new List<String>() : user.Roles.Select(r => r.Name).ToList());
		}

		public async Task<Boolean> IsInRoleAsync(TUser user, String roleName) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}
			if (String.IsNullOrWhiteSpace(roleName)) {
				throw new ArgumentNullException(roleName);
			}
			// TODO: Validate against the roles collection??
			return await Task.FromResult(user.Roles == null ? false : user.Roles.Any(r => r.Name == roleName));
		}

		public async Task RemoveFromRoleAsync(TUser user, String roleName) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}
			if (String.IsNullOrWhiteSpace(roleName)) {
				throw new ArgumentNullException("roleName");
			}

			IdentityRole role = await this.roleCollection.Find(r => r.Name == roleName).FirstOrDefaultAsync();
			if (role != null) {
				if (user.Roles != null && user.Roles.Any(r => r.RoleId == role.Id)) {
					user.Roles.Remove(user.Roles.FirstOrDefault(r => r.RoleId == role.Id));
				}
			}
		}

		public Task AddClaimAsync(TUser user, Claim claim) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}
			if (claim == null) {
				throw new ArgumentNullException("claim");
			}

			if (user.Claims == null) {
				user.Claims = new List<IdentityUserClaim>();
			}
			user.Claims.Add(new IdentityUserClaim { Type = claim.Type, Value = claim.Value });
			return Task.FromResult(0);
		}

		public async Task<IList<Claim>> GetClaimsAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			List<IdentityUserClaim> claims = new List<IdentityUserClaim>();
			if (user.Claims != null) {
				claims = user.Claims.ToList();
			}
			return await Task.FromResult(
				claims.Select(cl => new Claim(cl.Type, cl.Value))
				.ToList()
			);
		}

		public Task RemoveClaimAsync(TUser user, Claim claim) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}
			if (claim == null) {
				throw new ArgumentNullException("claim");
			}


			if (user.Claims != null && user.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value)) {
				user.Claims.Remove(user.Claims.FirstOrDefault(c => c.Type == claim.Type && c.Value == claim.Value));

			}
			return Task.FromResult(0);
		}

		public Task AddLoginAsync(TUser user, UserLoginInfo login) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}
			if (login == null) {
				throw new ArgumentNullException("login");
			}

			if (user.Logins == null) {
				user.Logins = new List<IdentityUserLogin>();
			}
			if (!user.Logins.Any(l => l.ProviderKey == login.ProviderKey && l.LoginProvider == login.LoginProvider)) {
				user.Logins.Add(new IdentityUserLogin { LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
			}
			return Task.FromResult(0);
		}

		public async Task<TUser> FindAsync(UserLoginInfo login) {
			if (login == null) {
				throw new ArgumentNullException("login");
			}

			FilterDefinition<TUser> filter = Builders<TUser>
				.Filter
				.AnyEq(u => u.Logins, new IdentityUserLogin { LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });

			return await this.collection
				.Find(filter)
				.FirstOrDefaultAsync();
		}

		public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}

			ICollection<IdentityUserLogin> logins = user.Logins;
			return await Task.FromResult(
				logins.Select(iul => new UserLoginInfo(iul.LoginProvider, iul.ProviderKey))
				.ToList()
			);
		}

		public Task RemoveLoginAsync(TUser user, UserLoginInfo login) {
			if (user == null) {
				throw new ArgumentNullException("user");
			}
			if (login == null) {
				throw new ArgumentNullException("login");
			}

			if (user.Logins != null && user.Logins.Any(l => l.ProviderKey == login.ProviderKey && l.LoginProvider == login.LoginProvider)) {
				user.Logins.Remove(user.Logins.FirstOrDefault(l => l.ProviderKey == login.ProviderKey && l.LoginProvider == login.LoginProvider));
			}
			return Task.FromResult(0);
		}
	}
}
