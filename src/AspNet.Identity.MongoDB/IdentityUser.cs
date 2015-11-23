using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace AspNet.Identity.MongoDB {

	public class IdentityUser : IUser {
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public virtual String Id { get; set; }

		[BsonElement(FieldNames.UserName)]
		public virtual String UserName { get; set; }
		[BsonElement(FieldNames.LowerCaseUserName)]
		public virtual String LowerCaseUserName { get; set; }

		[BsonElement(FieldNames.EmailAddress)]
		public virtual String EmailAddress { get; set; }
		[BsonElement(FieldNames.LowerCaseEmailAddress)]
		public virtual String LowerCaseEmailAddress { get; set; }
		[BsonElement(FieldNames.EmailAddressConfirmed)]
		public virtual Boolean EmailAddressConfirmed { get; set; }

		[BsonElement(FieldNames.PhoneNumber)]
		public virtual String PhoneNumber { get; set; }
		[BsonElement(FieldNames.PhoneNumberConfirmed)]
		public virtual Boolean PhoneNumberConfirmed { get; set; }

		[BsonElement(FieldNames.PasswordHash)]
		public virtual String PasswordHash { get; set; }
		[BsonElement(FieldNames.SecurityStamp)]
		public virtual String SecurityStamp { get; set; }

		[BsonIgnoreIfNull]
		[BsonElement(FieldNames.LockoutEndDateUtc)]
		public virtual DateTimeOffset? LockoutEndDateUtc { get; set; }
		[BsonElement(FieldNames.LockoutEnabled)]
		public virtual Boolean LockoutEnabled { get; set; }
		[BsonElement(FieldNames.AccessFailedCount)]
		public virtual Int32 AccessFailedCount { get; set; }

		[BsonElement(FieldNames.TwoFactorEnabled)]
		public virtual Boolean TwoFactorEnabled { get; set; }

		[BsonIgnoreIfNull]
		[BsonElement(FieldNames.Roles)]
		public virtual ICollection<IdentityUserRole> Roles { get; set; }
		[BsonIgnoreIfNull]
		[BsonElement(FieldNames.Claims)]
		public virtual ICollection<IdentityUserClaim> Claims { get; set; }
		[BsonIgnoreIfNull]
		[BsonElement(FieldNames.Logins)]
		public virtual ICollection<IdentityUserLogin> Logins { get; set; }
		public override String ToString() {
			return this.UserName;
		}

		public static class FieldNames {
			public const String Id = "_id";
			public const String UserName = "un";
			public const String LowerCaseUserName = "lun";
			public const String EmailAddress = "em";
			public const String LowerCaseEmailAddress = "lem";
			public const String EmailAddressConfirmed = "ec";
			public const String PhoneNumber = "pn";
			public const String PhoneNumberConfirmed = "pc";
			public const String PasswordHash = "ph";
			public const String SecurityStamp = "ss";
			public const String LockoutEnabled = "lo";
			public const String LockoutEndDateUtc = "le";
			public const String AccessFailedCount = "af";
			public const String TwoFactorEnabled = "tf";
			public const String Logins = "li";
			public const String Claims = "cl";
			public const String Roles = "ro";
		}
	}
}
