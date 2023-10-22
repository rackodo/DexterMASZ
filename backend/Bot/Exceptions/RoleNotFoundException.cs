using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class RoleNotFoundException(ulong roleId) : ApiException($"Role {roleId} not found.", ApiError.RoleNotFound)
{
    public ulong RoleId { get; set; } = roleId;
}
