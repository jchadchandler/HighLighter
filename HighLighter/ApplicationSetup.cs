using System.Collections.Generic;
using System.Windows.Controls;
using LinkViewer.Infrastructure;
using Newtonsoft.Json;

namespace LinkViewer
{
	public class ApplicationSetup
	{
		public static void Setup(MainWindow mainApplicationWindow)
		{
			var fileSystem = new FileSystem();
			var text = fileSystem.ReadAllTextFromFile("LinkTypes.json");
			var links = JsonConvert.DeserializeObject<List<LinkType>>(text);
			var locator = new LinkLocator.LinkLocator(links);
			var launcher = new AssociateApplicationLauncher();
			var dockPanel = new DockPanel {LastChildFill = true};
			dockPanel.Children.Add(new InstagramHighlightingTextBox(locator, launcher, "Hi.rtf"));
			mainApplicationWindow.Content = dockPanel;
		}
	}
}