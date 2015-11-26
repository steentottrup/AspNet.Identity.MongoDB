AspNet.Identity.MongoDB
=======================

An updated version of an ASP.NET Identity provider using MongoDB for storage. This started out as a fork on the original project [https://github.com/InspectorIT/MongoDB.AspNet.Identity](MongoDB.AspNet.Identity by InspectorIT), but it seems the author has abandoned the project, so I've decided to create my own repository.

[![Build status](https://ci.appveyor.com/api/projects/status/1knmbosmm45mdr48/branch/master?svg=true)](https://ci.appveyor.com/project/SteenTttrup/aspnet-identity-mongodb/branch/master)

## Purpose ##

ASP.NET MVC 5 shipped with a new Identity system (in the Microsoft.AspNet.Identity.Core package) in order to support both local login and remote logins via OpenID/OAuth, but only ships with an Entity Framework provider (Microsoft.AspNet.Identity.EntityFramework).

## News ##
__22-11-2015__ - The repository was created to take the code to the latest version of the Identity assemblies and the MongoDB driver.

## Features ##
* Drop-in replacement ASP.NET Identity with MongoDB as the backing store.
* Requires only 2 mongo document type, one for users and one for roles
* Supports additional profile properties on your application's user model.
* Provides UserStore<TUser> implementation that implements these interfaces:
	* IUserLoginStore<TUser>
	* IUserClaimStore<TUser>
	* IUserRoleStore<TUser>
	* IUserPasswordStore<TUser>
	* IUserSecurityStampStore<TUser>
	* IQueryableUserStore<TUser>
	* IUserEmailStore<TUser>
	* IUserPhoneNumberStore<TUser>
	* IUserTwoFactorStore<TUser, string>
	* IUserLockoutStore<TUser, string>
	* IUserStore<TUser>
* Provides RoleStore<TRole> implementation that implements this interface:
	* IQueryableRoleStore<TRole>

## Instructions ##
These instructions assume you know how to set up MongoDB within an MVC application.

1. Create a new ASP.NET MVC 5 project, choosing the Individual User Accounts authentication type.
2. Remove the Entity Framework packages and replace with MongoDB Identity:

```PowerShell
Uninstall-Package Microsoft.AspNet.Identity.EntityFramework
Uninstall-Package EntityFramework
Install-Package AspNetIdentity.MongoDB
```
    
3. In ~/Models/IdentityModels.cs:
    * Remove the namespace: Microsoft.AspNet.Identity.EntityFramework
    * Add the namespace: AspNet.Identity.MongoDB
	* Remove the ApplicationDbContext class completely.
4. In ~/Controllers/AccountController.cs
    * Remove the namespace: Microsoft.AspNet.Identity.EntityFramework
    * Add the connection string name to the constructor of the UserStore. Or empty constructor will use DefaultConnection

```C#
public AccountController()
{
    this.UserManager = new UserManager<ApplicationUser>(
        new UserStore<ApplicationUser>("Mongo");
}
```

```C#
public Application_Start()
{
        UserStore<ApplicationUser>.Initialize("Mongo");
        RoleStore<ApplicationUser>.Initialize("Mongo");
}
```

## Connection Strings ##
The UserStore has multiple constructors for handling connection strings. Here are some examples of the expected inputs and where the connection string should be located.

### 1. SQL Style ###
```C#
UserStore(string connectionNameOrUrl)
```
<code>UserStore("Mongo")</code>

**web.config**
```xml
<add name="Mongo" connectionString="Server=localhost:27017;Database={YourDataBase}" />
```

### 2. Mongo Style ###
```C#
UserStore(string connectionNameOrUrl)
```
<code>UserStore("Mongo")</code>

**web.config**
```xml
<add name="Mongo" connectionString="mongodb://localhost/{YourDataBase}" />
```

**OR**

```C#
UserStore(string connectionNameOrUrl)
```
<code>UserStore("mongodb://localhost/{YourDataBase}")</code>
