using System;
using System.Configuration;

namespace AspNet.Identity.MongoDB {

	public class MongoDBIdentitySettings : ConfigurationSection {
		private static MongoDBIdentitySettings settings = ConfigurationManager.GetSection("mongoDBIdentitySettings") as MongoDBIdentitySettings;

		private const String userCollectionName = "userCollectionName";
		private const String roleCollectionName = "roleCollectionName";

		public static MongoDBIdentitySettings Settings {
			get {
				return settings;
			}
		}

		//[ConfigurationProperty("frontPagePostCount", DefaultValue = 20, IsRequired = false)]
		//[IntegerValidator(MinValue = 1, MaxValue = 100)]
		//public int FrontPagePostCount {
		//	get { return (int)this["frontPagePostCount"]; }
		//	set { this["frontPagePostCount"] = value; }
		//}

		[ConfigurationProperty(userCollectionName, IsRequired = true, DefaultValue = "user")]
		[RegexStringValidator("^[a-zA-Z]+$")]
		public String UserCollectionName {
			get {
				return (String)this[userCollectionName];
			}
			set {
				this[userCollectionName] = value;
			}
		}

		[ConfigurationProperty(roleCollectionName, IsRequired = true, DefaultValue = "role")]
		[RegexStringValidator("^[a-zA-Z]+$")]
		public String RoleCollectionName {
			get {
				return (String)this[roleCollectionName];
			}
			set {
				this[roleCollectionName] = value;
			}
		}
	}
}
