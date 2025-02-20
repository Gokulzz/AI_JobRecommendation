using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Infrastructure.SignalR
{
    public class UserActivity : Hub
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<UserActivity> _logger;
        private const string ActiveUsersKey = "ActiveUsers";
        public UserActivity(IConnectionMultiplexer redis, ILogger<UserActivity> logger)

        {
            _redis = redis;
            _logger = logger;
        }
        public override async Task OnConnectedAsync()
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    _logger.LogInformation($"User {userId} connected at {DateTime.UtcNow}");

                    var db = _redis.GetDatabase();

                    // Add user ID to the ActiveUsers set
                    await db.SetAddAsync(ActiveUsersKey, userId);

                    // Map connection ID to user ID for future reference
                    await db.StringSetAsync($"ConnectionToUser:{Context.ConnectionId}", userId);
                }
                else
                {
                    _logger.LogWarning("User ID not found in JWT token.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during user connection: {ex.Message}");
            }
            finally
            {
                await base.OnConnectedAsync();
            }
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var db = _redis.GetDatabase();

                // Retrieve user ID from connection ID
                var userId = await db.StringGetAsync($"ConnectionToUser:{Context.ConnectionId}");
                if (!string.IsNullOrEmpty(userId))
                {
                    _logger.LogInformation($"User {userId} disconnected at {DateTime.UtcNow}");

                    // Remove connection-to-user mapping
                    await db.KeyDeleteAsync($"ConnectionToUser:{Context.ConnectionId}");

                    // Check if multiple connections exist for the same user
                    var connectionKey = $"UserConnections:{userId}";
                    await db.SetRemoveAsync(connectionKey, Context.ConnectionId);

                    // Remove user from ActiveUsers set only if no connections remain
                    if ((await db.SetLengthAsync(connectionKey)) == 0)
                    {
                        await db.SetRemoveAsync(ActiveUsersKey, userId);
                        _logger.LogInformation($"User {userId} fully removed from ActiveUsers.");
                    }
                }
                else
                {
                    _logger.LogWarning("User ID not found for disconnection.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during user disconnection: {ex.Message}");
            }
            finally
            {
                await base.OnDisconnectedAsync(exception);
            }
        }


        //check if the user is active
        public async Task<bool> IsUserActive(Guid userId, IConnectionMultiplexer redis)
        {
            var db = _redis.GetDatabase();
            return await db.SetContainsAsync(ActiveUsersKey, userId.ToString());
        }
    }
}
