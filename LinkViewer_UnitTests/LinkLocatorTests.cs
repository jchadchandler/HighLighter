using System;
using System.Collections.Generic;
using System.Linq;
using LinkViewer;
using LinkViewer.LinkLocator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkViewer_UnitTests
{
	[TestClass]
	public class LinkLocatorTests
	{
		private LinkLocator _linkLocator;
		private List<LinkType> _linkTypes;

		[TestInitialize]
		public void Setup()
		{
			_linkTypes = new List<LinkType>()
			{
				new LinkType() {name="hashTag", pattern="(^|\\s)(#)(\\S+)", url = "https://www.instagram.com/tags/$3" },
				new LinkType() {name="user", pattern="(^|\\s)(@)(\\S+)", url = "https://www.instagram.com/tags/$3" }

			};
			_linkLocator = new LinkLocator(_linkTypes.AsQueryable());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructingWithANullLinkTypeCollectionThrowsAnException()
		{
			// ReSharper disable once ObjectCreationAsStatement
			new LinkLocator(null);
		}

		[TestMethod]
		public void ConstructingWithAnEmptyLinkTypeCollectionDoesNotThrowAnException()
		{
			// ReSharper disable once ObjectCreationAsStatement
			new LinkLocator(new List<LinkType>().AsQueryable());
		}


		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SearchingNullTextThrowsAnException()
		{
			_linkLocator.LocateAllLinksInText(null);
		}

		[TestMethod]
		public void SearchingEmptyTextDoesNotThrowAnException()
		{
			_linkLocator.LocateAllLinksInText(string.Empty);
		}

		[TestMethod]
		public void SearchDoesNotFindAnyLinksReturnsEmptyLinkCollection()
		{
			var result = _linkLocator.LocateAllLinksInText("Nothing here to find");
			Assert.AreEqual(0, result.Count());
		}

		[TestMethod]
		public void SearchingTextThatHasOneLinkAndNoOtherTextReturnsOneLink()
		{
			var result = _linkLocator.LocateAllLinksInText("@userName");
			Assert.AreEqual(1, result.Count());
		}


		[TestMethod]
		public void SearchingTextThatBeginsWithALinkAndHasAdditionalTextReturnsOneLink()
		{
			var result = _linkLocator.LocateAllLinksInText("@userName additionalText");
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public void SearchingTextThatBeginsWithAdditionalTextAndEndsInALinkReturnsOneLink()
		{
			var result = _linkLocator.LocateAllLinksInText("Additional Text @userName");
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public void SearchingTextThatEndssWithALinkAndThenASpaceReturnsOneLink()
		{
			var result = _linkLocator.LocateAllLinksInText("Additional Text @userName ");
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public void SearchingTextThatContainsALinkBetweenOtherTextReturnsOneLink()
		{
			var result = _linkLocator.LocateAllLinksInText("Additional @userName Text");
			Assert.AreEqual(1, result.Count());
		}

		[TestMethod]
		public void SearchingTextThatContainsJustTwoLinksThatAreOfTheSameTypeReturnsTwoLinks()
		{
			var result = _linkLocator.LocateAllLinksInText("@userName1 @userName2");
			Assert.AreEqual(2, result.Count());
		}

		[TestMethod]
		public void SearchingTextThatContainsTwoLinksThatAreOfTheSameTypeSeparatedByTextReturnsTwoLinks()
		{
			var result = _linkLocator.LocateAllLinksInText("@userName1 additional text @userName2");
			Assert.AreEqual(2, result.Count());
		}


		[TestMethod]
		public void SearchingTextThatContainsTwoLinksThatAreOfTheSameTypeSurroundedByTextReturnsTwoLinks()
		{
			var result = _linkLocator.LocateAllLinksInText("additional @userName1 @userName2 text ");
			Assert.AreEqual(2, result.Count());
		}

		[TestMethod]
		public void SearchingTextThatContainsTwoLinksThatAreOfDifferentLinkTypesReturnsTwoLinks()
		{
			var result = _linkLocator.LocateAllLinksInText("@userName1 #hashtext");
			Assert.AreEqual(2, result.Count());
		}

		[TestMethod]
		public void SearchingTextThatContainsTwoLinksThatAreOfDifferentTypesSeparatedByTextReturnsTwoLinks()
		{
			var result = _linkLocator.LocateAllLinksInText("@userName1 additional text #hasText");
			Assert.AreEqual(2, result.Count());
		}

		[TestMethod]
		public void SearchingTextThatContainsTwoLinksThatAreOfDifferentTypesSurroundedByTextReturnsTwoLinks()
		{
			var result = _linkLocator.LocateAllLinksInText("additional #hastag @userName2 text ");
			Assert.AreEqual(2, result.Count());
		}


	}
}
