using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Exceptions;

public class DemoModeEnabledException : ApiException
{
	public DemoModeEnabledException() :
		base("Demo mode is enabled. Only site admins can edit guild configs.", ApiError.NotAllowedInDemoMode)
	{
	}
}