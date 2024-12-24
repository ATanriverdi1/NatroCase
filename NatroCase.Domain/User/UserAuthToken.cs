using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NatroCase.Domain.Common;

namespace NatroCase.Domain.User;

public record UserAuthToken(string Token, long ExpirationDate)
{

    public static UserAuthToken Create(string secretKey, int expiresInMinutes)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        var tokenHandler = new JwtSecurityTokenHandler();
        var expires = Clock.UtcNow.AddMinutes(expiresInMinutes);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = expires,
            SigningCredentials = signingCredentials
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var userAuthToken = new UserAuthToken(jwtToken, expires.ToTimestamp());
        return userAuthToken;
    }
}