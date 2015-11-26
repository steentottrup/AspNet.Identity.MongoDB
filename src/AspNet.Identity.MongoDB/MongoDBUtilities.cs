using MongoDB.Driver;
using System;
using System.Configuration;

namespace AspNet.Identity.MongoDB {

	public static class MongoDBUtilities {
		private static String userCollectionName = String.Empty;
		private static String roleCollectionName = String.Empty;

		public static String GetRoleCollectionName() {
			if (String.IsNullOrWhiteSpace(roleCollectionName)) {
				roleCollectionName = MongoDBIdentitySettings.Settings != null ? MongoDBIdentitySettings.Settings.RoleCollectionName : "role";
			}
			return roleCollectionName;
		}

		public static String GetUserCollectionName() {
			if (String.IsNullOrWhiteSpace(userCollectionName)) {
				userCollectionName = MongoDBIdentitySettings.Settings != null ? MongoDBIdentitySettings.Settings.UserCollectionName : "user";
			}
			return userCollectionName;
		}

		public static IMongoDatabase GetDatabase(String connectionNameOrUrl) {
			if (connectionNameOrUrl.ToLower().StartsWith("mongodb://")) {
				return GetDatabaseFromUrl(new MongoUrl(connectionNameOrUrl));
			}
			else {
				String connStringFromManager = ConfigurationManager.ConnectionStrings[connectionNameOrUrl].ConnectionString;
				if (connStringFromManager.ToLower().StartsWith("mongodb://")) {
					return GetDatabaseFromUrl(new MongoUrl(connStringFromManager));
				}
				else {
					return GetDatabaseFromSqlStyle(connStringFromManager);
				}
			}
		}

		public static IMongoDatabase GetDatabaseFromSqlStyle(String connectionString) {
			MongoUrl url = MongoUrl.Create(connectionString);
			if (url.DatabaseName == null) {
				throw new Exception("No database name specified in connection string");
			}
			MongoClientSettings settings = MongoClientSettings.FromUrl(url);
			return new MongoClient(settings).GetDatabase(url.DatabaseName);
		}

		public static IMongoDatabase GetDatabaseFromUrl(MongoUrl url) {
			if (url.DatabaseName == null) {
				throw new Exception("No database name specified in connection string");
			}
			return new MongoClient(url).GetDatabase(url.DatabaseName); // WriteConcern defaulted to Acknowledged
		}

	}
}
