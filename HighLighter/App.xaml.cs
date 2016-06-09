using System.Windows;

namespace LinkViewer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			var mainWindow = new MainWindow();
			ApplicationSetup.Setup(mainWindow);
			mainWindow.Show();
		}
	}
}
