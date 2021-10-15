using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using masz.Models;
using masz.Dtos.Tokens;
using masz.Exceptions;
using masz.Enums;
using masz.Repositories;

namespace masz.Controllers
{
    [ApiController]
    [Route("api/v1/token")]
    [Authorize]
    public class TokenController : SimpleController
    {
        private readonly ILogger<TokenController> _logger;

        public TokenController(IServiceProvider serviceProvider, ILogger<TokenController> logger) : base(serviceProvider) {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] TokenForCreateDto tokenDto)
        {
            await RequireSiteAdmin();
            if (await GetIdentity() is TokenIdentity)
            {
                throw new BaseAPIException("Tokens cannot manage this resource.", APIError.TokenCannotManageThisResource);
            }

            var repo = TokenRepository.CreateDefault(_serviceProvider);
            try
            {
                if (await repo.GetToken() != null)
                {
                    throw new BaseAPIException("There already is a token.", APIError.TokenAlreadyRegistered);
                }
            } catch (ResourceNotFoundException) { }

            _identityManager.ClearTokenIdentities();

            return Ok(await repo.RegisterToken(tokenDto.Name));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteToken()
        {
            await RequireSiteAdmin();
            if (await GetIdentity() is TokenIdentity) {
                throw new BaseAPIException("Tokens cannot manage this resource.", APIError.TokenCannotManageThisResource);
            }

            await TokenRepository.CreateDefault(_serviceProvider).DeleteToken();

            _identityManager.ClearTokenIdentities();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetTokenInfo()
        {
            await RequireSiteAdmin();
            if (await GetIdentity() is TokenIdentity) {
                throw new BaseAPIException("Tokens cannot manage this resource.", APIError.TokenCannotManageThisResource);
            }

            APIToken apiToken = await TokenRepository.CreateDefault(_serviceProvider).GetToken();

            apiToken.TokenHash = null;
            apiToken.TokenSalt = null;

            return Ok(apiToken);
        }
    }
}