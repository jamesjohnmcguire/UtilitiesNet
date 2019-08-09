/////////////////////////////////////////////////////////////////////////////
// <copyright file="UnitTests.cs" company="James John McGuire">
// Copyright © 2006 - 2019 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using DigitalZenWorks.Common.Utilities;
using NUnit.Framework;
using System.IO;

namespace DigitalZenWorks.Common.Utilities.Tests
{
	[TestFixture]
	public static class UnitTests
	{
		/////////////////////////////////////////////////////////////////////
		/// Method <c>Teardown</c>
		/// <summary>
		/// function that is called just after each test method is called.
		/// </summary>
		/////////////////////////////////////////////////////////////////////
		[TearDown]
		public static void Teardown()
		{
			bool result = File.Exists("test.xml");
			if (true == result)
			{
				File.Delete("test.xml");
			}

			result = File.Exists("test.xsd");
			if (true == result)
			{
				File.Delete("test.xsd");
			}
		}

		[Test]
		public static void GetEmbeddedResource()
		{
			bool result = FileUtils.CreateFileFromEmbeddedResource(
				"DigitalZenWorks.Common.Utilities.Tests.test.xml", "test.xml");
			Assert.True(result);

			result = File.Exists("test.xml");
			Assert.True(result);
		}

		[Test]
		public static void ObjectFromXml()
		{
			bool result = FileUtils.CreateFileFromEmbeddedResource(
				"DigitalZenWorks.Common.Utilities.Tests.test.xsd", "test.xsd");
			Assert.True(result);

			result = File.Exists("test.xsd");
			Assert.True(result);

			result = FileUtils.CreateFileFromEmbeddedResource(
				"DigitalZenWorks.Common.Utilities.Tests.test.xml", "test.xml");
			Assert.True(result);

			result = File.Exists("test.xml");
			Assert.True(result);

			OrderedItem item = (OrderedItem)XmlUtilities.LoadWithValidation(
				"test.xsd", "test.xml", typeof(OrderedItem));

			Assert.NotNull(item);
			Assert.AreEqual(item.ItemName, "Widget");
			Assert.AreEqual(item.Description, "Regular Widget");
			Assert.AreEqual(item.UnitPrice, 2.3);
			Assert.AreEqual(item.Quantity, 10);
			Assert.AreEqual(item.LineTotal, 23);
		}
	}
}
