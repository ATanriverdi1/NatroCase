using System.Security.Cryptography;
using System.Text;
using NatroCase.Domain.Common;
using NatroCase.Domain.User.Entities;

namespace NatroCase.Domain.User;

public class UserAggregate : AggregateRoot
{
    public UserAggregate()
    {
        
    }

    public UserAggregate(string email, string name, string password)
    {
        Email = email;
        Name = name;
        Password = password;
        Favorites = new List<Favorite>();
        SetAsCreated();
    }

    public string Email { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public List<Favorite> Favorites { get; set; }

    public static UserAggregate Create(string email, string name, string password)
    {
        return new UserAggregate(email, name, password);
    }

    public void AddFavorite(string domainName, bool isAvailable)
    {
        SetAsModified();
        Favorites.Add(new Favorite(domainName, isAvailable, Clock.UtcNow.ToTimestamp()));
    }

    public void RemoveFavorite(string domainName)
    {
        SetAsModified();
        Favorites.RemoveAll(f => f.DomainName == domainName);
    }
    
    public UserAuthToken Authorize(
        string secretKey,
        int expiresInMinutes)
    {
        var userAuth = UserAuthToken.Create(secretKey, expiresInMinutes);
        return userAuth;
    }
    
    public static string GeneratePassword(string password, string salt)
    {
        var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt));
        var bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        var encryptedValue = Convert.ToBase64String(bytes);
        return encryptedValue;
    }
}