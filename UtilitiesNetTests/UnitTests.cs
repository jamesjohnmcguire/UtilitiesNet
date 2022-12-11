/////////////////////////////////////////////////////////////////////////////
// <copyright file="UnitTests.cs" company="James John McGuire">
// Copyright © 2006 - 2022 James John McGuire. All Rights Reserved.
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
			string filePath =
				GetXmlResourceFile("UtilitiesNetTests.test.xml", ".xml");

			bool result = File.Exists(filePath);
			Assert.True(result);

			if (result == true)
			{
				File.Delete(filePath);
			}
		}

		[Test]
		public static void ObjectFromXml()
		{
			string xmlFilePath =
				GetXmlResourceFile("UtilitiesNetTests.test.xml", ".xml");
			bool result = File.Exists(xmlFilePath);
			Assert.True(result);

			string xsdFilePath =
				GetXmlResourceFile("UtilitiesNetTests.test.xsd", ".xsd");
			result = File.Exists(xmlFilePath);
			Assert.True(result);

			OrderedItem item = (OrderedItem)XmlUtilities.LoadWithValidation(
				xsdFilePath, xmlFilePath, typeof(OrderedItem));

			Assert.NotNull(item);
			Assert.AreEqual(item.ItemName, "Widget");
			Assert.AreEqual(item.Description, "Regular Widget");
			Assert.AreEqual(item.UnitPrice, 2.3);
			Assert.AreEqual(item.Quantity, 10);
			Assert.AreEqual(item.LineTotal, 23);

			if (result == true)
			{
				File.Delete(xmlFilePath);
				File.Delete(xsdFilePath);
			}
		}

		private static string GetXmlResourceFile(
			string resource, string extension)
		{
			string fileName = Path.GetTempFileName();

			// A 0 byte sized file is created.  Need to remove it.
			File.Delete(fileName);
			string filePath = Path.ChangeExtension(fileName, extension);

			bool result = FileUtils.CreateFileFromEmbeddedResource(
				resource, filePath);

			Assert.True(result);

			return filePath;
		}
	}
}
