using Bot.Abstractions;
using Bot.Services;
using Levels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levels.Controllers;

public class LevelsConfigController : AuthenticatedController
{
	private readonly GuildUserLevelRepository _levelsRepository;
	private readonly GuildLevelConfigRepository _levelsConfigRepository;

	public LevelsConfigController(IdentityManager identityManager, GuildUserLevelRepository levelsRepository, GuildLevelConfigRepository levelsConfigRepository) :
		base(identityManager, levelsRepository, levelsConfigRepository)
	{
		_levelsRepository = levelsRepository;
		_levelsConfigRepository = levelsConfigRepository;
	}
}
