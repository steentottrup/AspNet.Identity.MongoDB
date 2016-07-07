using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet.Identity.MongoDB {

    public class RoleStore<TRole> :
        IRoleStore<TRole>,
        IQueryableRoleStore<TRole>
        where TRole : IdentityRole {

        private readonly IMongoDatabase database;
        private readonly IMongoCollection<TRole> collection;
        private Boolean disposed;

        public RoleStore(String connectionNameOrUrl) {
            String roleCollectionName = MongoDBIdentitySettings.Settings != null ? MongoDBIdentitySettings.Settings.RoleCollectionName : "role";
            this.database = MongoDBUtilities.GetDatabase(connectionNameOrUrl);
            this.collection = database.GetCollection<TRole>(MongoDBUtilities.GetRoleCollectionName());
            this.disposed = false;
        }

        public static void Initialize(String connectionNameOrUrl) {
            IMongoCollection<TRole> collection = MongoDBUtilities.GetDatabase(connectionNameOrUrl).GetCollection<TRole>(MongoDBUtilities.GetRoleCollectionName());
            EnsureIndexes.RoleStore<TRole>(collection);
        }

        public Task CreateAsync(TRole role) {
            if (role == null) {
                throw new ArgumentNullException("role");
            }

            // To avoid using regular expressions (with 'ignore case') when searching, we're lower-casing this field.
            // Should ensure best performance when finding roles by name
            role.LowerCaseName = role.Name.ToLowerInvariant();

            return this.collection.InsertOneAsync(role);
        }

        public Task DeleteAsync(TRole role) {
            if (role == null) {
                throw new ArgumentNullException("role");
            }
            // TODO: Delete or mark deleted?
            return this.collection.DeleteOneAsync(r => r.Id == role.Id);
        }

        public Task<TRole> FindByIdAsync(String roleId) {
            if (String.IsNullOrWhiteSpace(roleId)) {
                throw new ArgumentNullException("roleId");
            }
            return this.collection.Find<TRole>(r => r.Id == roleId)
                .FirstOrDefaultAsync();
        }

        public Task<TRole> FindByNameAsync(String roleName) {
            if (String.IsNullOrWhiteSpace(roleName)) {
                throw new ArgumentNullException("roleName");
            }
            String lowerCaseName = roleName.ToLowerInvariant();
            return this.collection.Find<TRole>(u => u.LowerCaseName == lowerCaseName)
                .FirstOrDefaultAsync();
        }

        public Task UpdateAsync(TRole role) {
            if (role == null) {
                throw new ArgumentNullException("role");
            }

            FilterDefinition<TRole> filter = Builders<TRole>
                .Filter
                .Eq(r => r.Id, role.Id);
            // TODO: ??? Check result??
            return this.collection.ReplaceOneAsync(filter, role);
        }

        public void Dispose() {
            if (!this.disposed) {
                this.disposed = true;
            }
        }

        public IQueryable<TRole> Roles {
            get {
                return this.collection.AsQueryable();
            }
        }
    }
}
