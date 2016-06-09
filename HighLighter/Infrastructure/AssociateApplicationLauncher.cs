using System.Diagnostics;

namespace LinkViewer.Infrastructure
{
	public class AssociateApplicationLauncher : IAssociatedApplicationLauncher
	{
		public void LaunchApplicationFor(string url)
		{
			Process.Start(url);
		}
	}
}