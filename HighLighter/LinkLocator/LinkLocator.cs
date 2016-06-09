using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LinkViewer.LinkLocator
{
	public class LinkLocator : ILinkLocator
	{
		private readonly IEnumerable<LinkType> _linkTypes;

		public LinkLocator(IEnumerable<LinkType> linkTypes)
		{
			if (linkTypes == null) throw new ArgumentNullException("linkTypes");
			_linkTypes = linkTypes;
		}

		public IEnumerable<LocatedLink> LocateAllLinksInText(string text)
		{
			if (text == null) throw new ArgumentNullException("text");

			var linkDictionary = new Dictionary<int,LocatedLink>();

			foreach (var linkType in _linkTypes)
			{
				foreach (Match match in Regex.Matches(text, linkType.pattern))
				{
					//eliminate duplicates
					linkDictionary[match.Index] = new LocatedLink(match.Value, match.Index, match.Length, linkType.pattern, linkType.url);
				}
			}
			return linkDictionary.Values;
		}



	}
}