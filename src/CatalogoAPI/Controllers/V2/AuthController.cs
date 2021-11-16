using CatalogoAPI.Controllers.Base;
using CatalogoAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoAPI.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/authentication")]
    public class AuthController : MainController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration,
                              UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public ActionResult<string> Get()
        {
            return "AuthController :: Acessado em : " + DateTime.Now.ToLongDateString();
        }

        [HttpPost("register")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO usuarioDTORequest)
        {
            var user = new IdentityUser
            {
                UserName = usuarioDTORequest.Email,
                Email = usuarioDTORequest.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioDTORequest.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await _signInManager.SignInAsync(user, false);

            return Ok(GeraToken(usuarioDTORequest));
        }

        /// <summary>
        /// Verifica as credenciais de um usuário
        /// </summary>
        /// <param name="usuarioDTORequest">Um objeto do tipo UsuarioDTO</param>
        /// <returns>Status 200 e o token do usuário</returns>
        /// <remarks>Status 200 e o token do usuário</remarks>
        [HttpPost("login")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO usuarioDTORequest)
        {
            var result = await _signInManager.PasswordSignInAsync(usuarioDTORequest.Email,
                                                                  usuarioDTORequest.Password,
                                                                  false,
                                                                  false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Login inválido");
                return BadRequest();
            }

            return Ok(GeraToken(usuarioDTORequest));
        }

        private UsuarioTokenDTO GeraToken(UsuarioDTO usuarioDTORequest)
        {
            //define declarações do usuário
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.UniqueName, usuarioDTORequest.Email),
                 new Claim("meuClaim", "claimMaster"),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            // Gerando chave privada
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["AppSettingsJWT:Secret"]));
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Expiração Token
            var expiracao = _configuration["AppSettingsJWT:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            // Gerando o token
            JwtSecurityToken token = new JwtSecurityToken(
              issuer: _configuration["AppSettingsJWT:Issuer"],
              audience: _configuration["AppSettingsJWT:Audience"],
              claims: claims,
              expires: expiration,
              signingCredentials: credenciais);

            return new UsuarioTokenDTO()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT OK"
            };

        }
    }
}

