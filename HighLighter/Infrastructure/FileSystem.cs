using System.IO;

namespace LinkViewer.Infrastructure
{
	public class FileSystem : IFileSystem
	{
		public string ReadAllTextFromFile(string fileName)
		{
			return File.ReadAllText("LinkTypes.json");
		}
	}
}