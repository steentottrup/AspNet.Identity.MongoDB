using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNet.Identity.MongoDB {

	public static class EnsureIndexes {

		public static void UserStore<TUser>(IMongoCollection<TUser> collection) where TUser : IdentityUser {
			List<BsonDocument> indexes = collection.Indexes.ListAsync().Result.ToListAsync().Result;
			if (!indexes.Any(i => i["name"].AsString != "_id_")) {
				IndexKeysDefinition<TUser> index = Builders<TUser>
					.IndexKeys
					.Ascending(u => u.LowerCaseUserName);
				CreateIndexOptions options = new CreateIndexOptions {
					Unique = true
				};
				String result = collection.Indexes.CreateOneAsync(index, options).Result;

				index = Builders<TUser>
					.IndexKeys
					.Ascending(u => u.LowerCaseEmailAddress);
				result = collection.Indexes.CreateOneAsync(index).Result;
			}
		}

		public static void RoleStore<TRole>(IMongoCollection<TRole> collection) where TRole : IdentityRole {
			List<BsonDocument> indexes = collection.Indexes.ListAsync().Result.ToListAsync().Result;
			if (!indexes.Any(i => i["name"].AsString != "_id_")) {
				IndexKeysDefinition<TRole> index = Builders<TRole>
					.IndexKeys
					.Ascending(u => u.LowerCaseName);
				CreateIndexOptions options = new CreateIndexOptions {
					Unique = true
				};
				String result = collection.Indexes.CreateOneAsync(index, options).Result;
			}
		}
	}
}
