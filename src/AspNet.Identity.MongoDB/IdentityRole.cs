using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNet.Identity.MongoDB {
	
	public class IdentityRole : IRole {
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public String Id { get; set; }

		[BsonElement(FieldNames.Name)]
		public virtual String Name { get; set; }
		[BsonElement(FieldNames.LowerCaseName)]
		public virtual String LowerCaseName { get; set; }

		public static class FieldNames {
			public const String id = "_id";
			public const String Name = "na";
			public const String LowerCaseName = "ln";
		}
	}
}
