using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class RoleNotFoundException : ApiException
{
    public ulong RoleId { get; set; }

    public RoleNotFoundException(ulong roleId) : base($"Role {roleId} not found.", ApiError.RoleNotFound) =>
        RoleId = roleId;
}
