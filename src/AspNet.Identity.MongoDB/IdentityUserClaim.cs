using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AspNet.Identity.MongoDB {

	public class IdentityUserClaim {
		[BsonElement(FieldNames.Value)]
		public String Value { get; set; }
		[BsonElement(FieldNames.Type)]
		public String Type { get; set; }

		public static class FieldNames {
			public const String Value = "v";
			public const String Type = "t";
		}
	}
}
