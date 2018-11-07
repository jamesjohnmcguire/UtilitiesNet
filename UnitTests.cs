﻿using System.IO;
using NUnit.Framework;

namespace DigitalZenWorks.Common.Utils
{
	public class OrderedItem
	{
		public string ItemName { get; set; }
		public string Description { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public decimal LineTotal { get; set; }
	}

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
				"DigitalZenWorks.Common.Utils.test.xml", "test.xml");
			Assert.True(result);

			result = File.Exists("test.xml");
			Assert.True(result);
		}

		[Test]
		public static void ObjectFromXml()
		{
			bool result = FileUtils.CreateFileFromEmbeddedResource(
				"DigitalZenWorks.Common.Utils.test.xsd", "test.xsd");
			Assert.True(result);

			result = File.Exists("test.xsd");
			Assert.True(result);

			result = FileUtils.CreateFileFromEmbeddedResource(
				"DigitalZenWorks.Common.Utils.test.xml", "test.xml");
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