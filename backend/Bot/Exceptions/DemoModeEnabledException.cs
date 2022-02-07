using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class DemoModeEnabledException : ApiException
{
	public DemoModeEnabledException() :
		base("Demo mode is enabled. Only site admins can edit guild configs.", ApiError.NotAllowedInDemoMode)
	{
	}
}