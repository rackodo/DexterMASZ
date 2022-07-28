using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levels.Models;

[Flags]
public enum XPType
{
	None = 0,
	Text = 1,
	Voice = 2,
	Any = 3
}
