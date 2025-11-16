using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.IdentityModel.Tokens;
using SorokChatServer.Core.Options;
using SorokChatServer.Logic.Contracts;
using SorokChatServer.Logic.Services;

namespace SorokChatServer.Core.Services;

public class JwtSerializerService : ITokenSerializerService
{
    private const string TokenNotValid = "Токен авторизації не коректний";

    private readonly JwtOptions _options;

    public JwtSerializerService(JwtOptions options)
    {
        _options = options;
    }

    public async Task<string> SerializeTokenAsync(Token token, CancellationToken cancellationToken = default)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, token.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, token.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)token.CreatedAt).ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Exp, ((DateTimeOffset)token.ExpiresAt).ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            expires: token.ExpiresAt,
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public Task<Result<Token>> DeserializeTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_options.SecretKey);
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _options.Issuer,
                ValidateAudience = true,
                ValidAudience = _options.Audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.FromMinutes(1)
            };
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            if (validatedToken is not JwtSecurityToken jwtToken)
                return Task.FromResult(Result.Failure<Token>(TokenNotValid));

            var issued = jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Iat);
            var id = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            var expires = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            if (id is null || email is null || expires is null || issued is null)
                return Task.FromResult(Result.Failure<Token>(TokenNotValid));

            if (!Guid.TryParse(id.Value, out var tokenId)) return Task.FromResult(Result.Failure<Token>(TokenNotValid));

            if (!long.TryParse(issued.Value, out var issuedAt))
                return Task.FromResult(Result.Failure<Token>(TokenNotValid));

            if (!long.TryParse(expires.Value, out var expiresTime))
                return Task.FromResult(Result.Failure<Token>(TokenNotValid));

            var expiresAt = DateTimeOffset.FromUnixTimeSeconds(expiresTime).UtcDateTime;
            var issuedTime = DateTimeOffset.FromUnixTimeSeconds(issuedAt).UtcDateTime;
            return Task.FromResult(Result.Success(new Token(tokenId, email.Value, issuedTime, expiresAt)));
        }
        catch (SecurityTokenExpiredException)
        {
            return Task.FromResult(Result.Failure<Token>("Час токена вийшов."));
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            return Task.FromResult(Result.Failure<Token>(TokenNotValid));
        }
        catch (Exception exception)
        {
            return Task.FromResult(Result.Failure<Token>(exception.Message));
        }
    }
}