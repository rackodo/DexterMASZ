﻿using DSharpPlus.Entities;
using masz.Events;
using masz.Exceptions;
using masz.Models;
using masz.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace masz.Services
{
    public class IdentityManager : IIdentityManager
    {
        private Dictionary<string, Identity> identities = new Dictionary<string, Identity>();
        private readonly ILogger<IdentityManager> _logger;
        private readonly IInternalConfiguration _config;
        private readonly IDiscordAPIInterface _discord;
        private readonly IEventHandler _eventHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public IdentityManager() { }

        public IdentityManager(ILogger<IdentityManager> logger, IInternalConfiguration config, IDiscordAPIInterface discord, IEventHandler eventHandler, IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _config = config;
            _discord = discord;
            _eventHandler = eventHandler;
            _serviceProvider = serviceProvider;
            _serviceScopeFactory = serviceScopeFactory;
        }

        private async Task<Identity> RegisterNewIdentity(HttpContext httpContext)
        {
            string key = String.Empty;
            Identity identity = null;
            if (httpContext.Request.Headers.ContainsKey("Authorization")) {
                _logger.LogInformation("Registering new TokenIdentity.");
                key = "/api/" + httpContext.Request.Headers["Authorization"];
                string fullToken = httpContext.Request.Headers["Authorization"];
                string token = String.Empty;
                try
                {
                    token = fullToken.Split(' ')[1];  // exclude "Bearer" prefix
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while parsing token: " + e.Message);
                }
                APIToken registeredToken = null;
                try
                {
                    registeredToken = await TokenRepository.CreateDefault(_serviceProvider).GetToken();
                } catch (ResourceNotFoundException) { }
                identity = new TokenIdentity(token, _serviceProvider, registeredToken, _serviceScopeFactory);
            } else {
                key = httpContext.Request.Cookies["masz_access_token"];
                _logger.LogInformation("Registering new DiscordIdentity.");
                string token = await httpContext.GetTokenAsync("Cookies", "access_token");
                identity = await DiscordOAuthIdentity.Create(token, _serviceProvider, _serviceScopeFactory);
            }
            identities[key] = identity;
            await _eventHandler.InvokeIdentityRegistered(new IdentityRegisteredEventArgs(identity));
            return identity;
        }

        private async Task<Identity> RegisterNewIdentity(DiscordUser user)
        {
            string key = $"/discord/cmd/{user.Id}";
            Identity identity = await DiscordCommandIdentity.Create(user, _serviceProvider, _serviceScopeFactory);
            identities[key] = identity;
            await _eventHandler.InvokeIdentityRegistered(new IdentityRegisteredEventArgs(identity));
            return identity;
        }

        public async Task<Identity> GetIdentity(HttpContext httpContext)
        {
            string key = String.Empty;
            if (httpContext.Request.Headers.ContainsKey("Authorization")) {
                key = "/api/" + httpContext.Request.Headers["Authorization"];
            } else {
                key = httpContext.Request.Cookies["masz_access_token"];
            }
            if (identities.ContainsKey(key))
            {
                Identity identity = identities[key];
                if (identity.ValidUntil >= DateTime.UtcNow)
                {
                    return identity;
                } else
                {
                    identities.Remove(key);
                }
            }

            return await RegisterNewIdentity(httpContext);
        }

        public async Task<Identity> GetIdentity(DiscordUser user)
        {
            if (user == null)
            {
                return null;
            }
            string key = $"/discord/cmd/{user.Id}";
            if (identities.ContainsKey(key))
            {
                Identity identity = identities[key];
                if (identity.ValidUntil >= DateTime.UtcNow)
                {
                    return identity;
                } else
                {
                    identities.Remove(key);
                }
            }

            return await RegisterNewIdentity(user);
        }

        public List<Identity> GetCurrentIdentities()
        {
            return this.identities.Values.ToList();
        }

        public void ClearAllIdentities()
        {
            this.identities.Clear();
        }

        public void ClearOldIdentities()
        {
            _logger.LogInformation("IdentityManager | Clearing old identities.");
            foreach (var key in this.identities.Keys)
            {
                if (this.identities[key].ValidUntil < DateTime.UtcNow) {
                    _logger.LogInformation($"IdentityManager | Clearing {key.ToString()}.");
                    this.identities.Remove(key);
                }
            }
            _logger.LogInformation("IdentityManager | Cleared old identities.");
        }

        public void ClearTokenIdentities()
        {
            _logger.LogInformation("IdentityManager | Clearing token identities.");
            foreach (var key in this.identities.Keys)
            {
                if (this.identities[key] is TokenIdentity) {
                    _logger.LogInformation($"IdentityManager | Clearing {key.ToString()}.");
                    this.identities.Remove(key);
                }
            }
            _logger.LogInformation("IdentityManager | Cleared token identities.");
        }

        public Task<Identity> GetIdentityByUserId(ulong userId)
        {
            throw new NotImplementedException();
        }
    }
}
