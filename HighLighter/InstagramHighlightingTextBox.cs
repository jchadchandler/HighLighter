using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using LinkViewer.Infrastructure;
using LinkViewer.LinkLocator;

namespace LinkViewer
{
	public class InstagramHighlightingTextBox : RichTextBox
	{
		private bool _updating;
		private readonly ILinkLocator _locator;
		private readonly IAssociatedApplicationLauncher _launcher;


		public  InstagramHighlightingTextBox(ILinkLocator locator, IAssociatedApplicationLauncher launcher, string initialTextFileName)
		{
			if (locator == null) throw new ArgumentNullException("locator");
			if (launcher == null) throw new ArgumentNullException("launcher");
						
			IsDocumentEnabled = true;
			_locator = locator;
			_launcher = launcher;

			LoadInitialText(initialTextFileName);
			Document.FontSize = 12;
		}

		private void LoadInitialText(string initialTextFileName)
		{
			if (string.IsNullOrEmpty(initialTextFileName))
				return;
			if (!File.Exists(initialTextFileName))
			{
				MessageBox.Show(string.Format("Could not find file: {0}", initialTextFileName), "File not found");
				return;
			}
			var textRange = new TextRange(Document.ContentStart, Document.ContentEnd);
			using (var fileStream = new FileStream(initialTextFileName, FileMode.OpenOrCreate))
			{
				textRange.Load(fileStream, DataFormats.Rtf);
			}

		}


		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			if (_updating) return;
			_updating = true;
			try
			{					
				ClearProperties();
				var linksInRuns = FindLinksInRuns(Document.ContentStart, Document.ContentEnd);
				foreach (var key in linksInRuns.Keys)
				{
					AddHyperLinksToRun(key,linksInRuns[key]);
				}
				base.OnTextChanged(e);
			}
			finally
			{
				_updating = false;
			}
		}

		private static IEnumerable<Run> GetRuns(TextPointer contentStart, TextPointer contentEnd)
		{
			var runs = new List<Run>();
			var textPointer = contentStart;
			while (textPointer != null && textPointer.CompareTo(contentEnd) < 0)
			{
				var run = textPointer.Parent as Run;
				if (run != null)
				{
					runs.Add(run);
				}
				textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
			}
			return runs;
		}

		private IDictionary<Run,IEnumerable<LocatedLink>> FindLinksInRuns(TextPointer contentStart, TextPointer contentEnd)
		{
			var locatedLinks = new Dictionary<Run,IEnumerable<LocatedLink>>();
			foreach (var run in GetRuns(contentStart, contentEnd))
			{
				var links = _locator.LocateAllLinksInText(run.Text);
				locatedLinks[run] = links;
			}
			return locatedLinks;
		}

		private void AddHyperLinksToRun(TextElement run,IEnumerable<LocatedLink> linksToAdd)
		{

			foreach (var linkLocation in linksToAdd)
			{
				var start = run.ContentStart.GetPositionAtOffset(linkLocation.Offset);
				if (start == null) return;

				var end = run.ContentStart.GetPositionAtOffset(linkLocation.Offset + linkLocation.Length);
				if (end == null) return;

				var uriText = Regex.Replace(linkLocation.LinkText, linkLocation.LinkPattern, linkLocation.UrlPattern);



				try
				{
					var link = new Hyperlink(start, end)
					{
						NavigateUri = new Uri(uriText),
					};

					link.Click += (s, e) =>
					{
						var text = link.NavigateUri.ToString();
						_launcher.LaunchApplicationFor(text);
					};
				}
				catch (UriFormatException)
				{
					MessageBox.Show( string.Format("Invalid uri: {0}",uriText),"Invalid URI" );
				}


			}

		}


		private void ClearProperties()
		{
			var documentRange = new TextRange(Document.ContentStart,Document.ContentEnd);
			documentRange.ClearAllProperties();
		}
	}
}