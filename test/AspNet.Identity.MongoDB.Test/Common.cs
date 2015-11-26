using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using System;

namespace AspNet.Identity.MongoDB.Test {

	public static class Common {
		private const String mongoUrl = "mongodb://localhost/test";

		private static void CleanUp() {
			MongoUrl url = new MongoUrl(mongoUrl);
			IMongoDatabase db = new MongoClient(url).GetDatabase(url.DatabaseName);

			db.DropCollectionAsync("user");
			db.DropCollectionAsync("role");
		}

		public static UserManager<IdentityUser> Initialize() {
			CleanUp();
			UserStore<IdentityUser>.Initialize(mongoUrl);
			return new UserManager<IdentityUser>(new UserStore<IdentityUser>(mongoUrl));
		}

		public static UserManager<IdentityUser> InitializeWithToken() {
			CleanUp();
			UserStore<IdentityUser>.Initialize(mongoUrl);
			return new UserManager<IdentityUser>(new UserStore<IdentityUser>(mongoUrl)) {
				UserTokenProvider = new CustomUserTokenProvider(),
			};
		}

		public static RoleManager<IdentityRole> InitializeRoleManager() {
			return new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(mongoUrl));
		}
	}
}
