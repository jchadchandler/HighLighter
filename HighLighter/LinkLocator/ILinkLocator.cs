using System.Collections.Generic;
using System.Linq;

namespace LinkViewer.LinkLocator
{
	public interface ILinkLocator
	{
		IEnumerable<LocatedLink> LocateAllLinksInText(string text);
	}
}