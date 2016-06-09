namespace LinkViewer.LinkLocator
{
	public class LocatedLink
	{
		public string LinkText { get; private set; }
		public int Offset { get; private set; }
		public int Length { get; private set; }
		public string UrlPattern { get; private set; }

		public string LinkPattern { get; private set; }


		public LocatedLink(string linkText, int offset, int length, string linkPattern, string urlPattern)
		{
			LinkText = linkText;
			Offset = offset;
			Length = length;
			LinkPattern = linkPattern;
			UrlPattern = urlPattern;
		}
	}
}