using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SorokChatServer.Domain.Models;
using SorokChatServer.Domain.Options;

namespace SorokChatServer.Domain.Services;

public class JwtTokenService : ITokenSerializer, ITokenDeserializer
{
    private const string UnknownError = "Невідома помилка.";
    private const string TokenNotValidError = "Токен доступу не коректний.";

    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(IOptions<JwtOptions> jwtOptions, ILogger<JwtTokenService> logger)
    {
        _logger = logger;
        _jwtOptions = jwtOptions.Value;
    }

    public Result<Token, Error> Deserialize(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.Zero
        };
        try
        {
            var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);
            if (validatedToken is not JwtSecurityToken) throw new SecurityTokenException(TokenNotValidError);

            var id = Guid.Parse(
                principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.ToString() ??
                string.Empty);
            var email = principal.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.ToString() ?? string.Empty;
            var createdAt = DateTime.Parse(principal.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Iss)?.ToString() ?? string.Empty);
            var expiredAt = DateTime.Parse(principal.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.ToString() ?? string.Empty);
            return new Token(id, email, createdAt, expiredAt);
        }
        catch (SecurityTokenException exception)
        {
            _logger.LogDebug(exception, exception.Message);
            return new Error(TokenNotValidError, HttpStatusCode.BadRequest);
        }
    }

    public Result<string, Error> Serialize(Token token)
    {
        try
        {
            var keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
            var jwtHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, token.Id.ToString()),
                new(JwtRegisteredClaimNames.Sub, token.Email),
                new(JwtRegisteredClaimNames.Exp, token.ExpiresAt.ToString(CultureInfo.InvariantCulture)),
                new(JwtRegisteredClaimNames.Iss, token.CreatedAt.ToString(CultureInfo.InvariantCulture))
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = token.ExpiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = jwtHandler.CreateToken(tokenDescriptor);
            return jwtHandler.WriteToken(securityToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            return new Error(UnknownError, HttpStatusCode.InternalServerError);
        }
    }
}