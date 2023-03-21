/////////////////////////////////////////////////////////////////////////////
// <copyright file="UnitTests.cs" company="James John McGuire">
// Copyright © 2006 - 2023 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using NUnit.Framework;
using System;
using System.IO;

[assembly: CLSCompliant(true)]

namespace DigitalZenWorks.Common.Utilities.Tests
{
	/// <summary>
	/// Automated Tests for UtilitiesNET.
	/// </summary>
	[TestFixture]
	public static class UnitTests
	{
		private const string TestData = "Lorem ipsum dolor sit " +
			"amet, consectetur adipiscing elit. Proin auctor pharetra " +
			"ipsum, ac molestie nibh. Sed interdum, eros mollis " +
			"pellentesque rhoncus, erat odio pretium erat, in finibus orci " +
			"turpis ac mi. Vestibulum efficitur odio non ex tempus commodo. " +
			"Maecenas vitae lectus non ex laoreet egestas. Fusce vitae " +
			"felis quis ligula sagittis blandit at a risus. Nunc fringilla, " +
			"augue non accumsan vestibulum, orci mi bibendum metus, eget " +
			"fermentum lectus erat vitae nisi. Vivamus ac lobortis felis, " +
			"ac molestie nisi. Suspendisse accumsan nibh sit amet massa " +
			"finibus, ac rhoncus magna scelerisque. Aenean ut tortor sed " +
			"dui eleifend consectetur eget a urna. Etiam sit amet est sem. " +
			"Nullam ultricies mollis diam, vel laoreet eros tincidunt vel. " +
			"Aliquam at vestibulum tortor. Curabitur gravida, nisi vitae " +
			"porttitor placerat, arcu tortor gravida purus, at vulputate " +
			"turpis ante sed lorem. Integer in fermentum lacus, quis tempor " +
			"elit. Praesent elementum nisi vitae egestas elementum.";

		/////////////////////////////////////////////////////////////////////
		/// Method <c>Teardown.</c>
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

		/// <summary>
		/// Are files the same fail test.
		/// </summary>
		[Test]
		public static void AreFilesTheSameFail()
		{
			string filePath =
				GetXmlResourceFile("UtilitiesNetTests.test.xml", ".xml");
			string filePath2 =
				GetXmlResourceFile("UtilitiesNetTests.test.xsd", ".xsd");

			bool result = FileUtils.AreFilesTheSame(filePath, filePath2);
			Assert.False(result);

			result = File.Exists(filePath);
			Assert.True(result);

			if (result == true)
			{
				File.Delete(filePath);
			}

			result = File.Exists(filePath2);
			Assert.True(result);

			if (result == true)
			{
				File.Delete(filePath2);
			}
		}

		/// <summary>
		/// Are files the same success test.
		/// </summary>
		[Test]
		public static void AreFilesTheSameSuccess()
		{
			string filePath =
				GetXmlResourceFile("UtilitiesNetTests.test.xml", ".xml");
			string filePath2 = filePath + "2";
			File.Copy(filePath, filePath2);

			bool result = FileUtils.AreFilesTheSame(filePath, filePath2);
			Assert.True(result);

			result = File.Exists(filePath);
			Assert.True(result);

			if (result == true)
			{
				File.Delete(filePath);
			}

			result = File.Exists(filePath2);
			Assert.True(result);

			if (result == true)
			{
				File.Delete(filePath2);
			}
		}

		/// <summary>
		/// Get embedded resource test.
		/// </summary>
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

		/// <summary>
		/// Get file hash test.
		/// </summary>
		[Test]
		public static void GetFileHash()
		{
			string filePath = Path.GetTempFileName();
			File.WriteAllText(filePath, TestData);

			byte[] hash = FileUtils.GetFileHash(filePath);
			Assert.NotNull(hash);

			string hashText = BitConverter.ToString(hash);
			hashText = hashText.Replace(
				"-", string.Empty, StringComparison.Ordinal);

			string compareHash = "3163EC56BAB76F5BA3C2CFAC49915EE83C10BC" +
				"4975FD70BB01F4B30501AA9856";

			Assert.AreEqual(hashText, compareHash);

			bool result = File.Exists(filePath);
			if (result == true)
			{
				File.Delete(filePath);
			}
		}

		/// <summary>
		/// Object from xml test.
		/// </summary>
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
