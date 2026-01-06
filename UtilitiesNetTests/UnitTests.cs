/////////////////////////////////////////////////////////////////////////////
// <copyright file="UnitTests.cs" company="James John McGuire">
// Copyright © 2006 - 2026 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

[assembly: System.CLSCompliant(true)]

namespace DigitalZenWorks.Common.Utilities.Tests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DigitalZenWorks.Common.Utilities;
using DigitalZenWorks.Common.Utilities.Extensions;
using NUnit.Framework;

/// <summary>
/// Automated Tests for UtilitiesNET.
/// </summary>
[TestFixture]
internal static class UnitTests
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

	/// Method <c>Teardown.</c>
	/// <summary>
	/// function that is called just after each test method is called.
	/// </summary>
	[TearDown]
	public static void Teardown()
	{
		bool result = File.Exists("test.xml");
		if (result == true)
		{
			File.Delete("test.xml");
		}

		result = File.Exists("test.xsd");
		if (result == true)
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
		string filePath = GetXmlResourceFile(
			"DigitalZenWorks.Common.Utilities.Tests.test.xml", ".xml");
		string filePath2 = GetXmlResourceFile(
			"DigitalZenWorks.Common.Utilities.Tests.test.xsd", ".xsd");

		bool result = FileUtils.AreFilesTheSame(filePath, filePath2);
		Assert.That(result, Is.False);

		result = File.Exists(filePath);
		Assert.That(result, Is.True);

		if (result == true)
		{
			File.Delete(filePath);
		}

		result = File.Exists(filePath2);
		Assert.That(result, Is.True);

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
		string filePath = GetXmlResourceFile(
			"DigitalZenWorks.Common.Utilities.Tests.test.xml", ".xml");
		string filePath2 = filePath + "2";
		File.Copy(filePath, filePath2);

		bool result = FileUtils.AreFilesTheSame(filePath, filePath2);
		Assert.That(result, Is.True);

		result = File.Exists(filePath);
		Assert.That(result, Is.True);

		if (result == true)
		{
			File.Delete(filePath);
		}

		result = File.Exists(filePath2);
		Assert.That(result, Is.True);

		if (result == true)
		{
			File.Delete(filePath2);
		}
	}

	/// <summary>
	/// Convert to camel case from casel case test.
	/// </summary>
	[Test]
	public static void ConvertToCamelCaseFromKnr()
	{
		string name = "name_english";

		string? output =
			TextCase.ConvertToCamelCaseFromKnr(name);

		string compareText = "nameEnglish";

		Assert.That(output, Is.EqualTo(compareText));
	}

	/// <summary>
	/// ConvertToSnakeCaseFromPascalCase test.
	/// </summary>
	[Test]
	public static void ConvertToSnakeCaseFromPascalCase()
	{
		string name = "LandingPages";

		string? output =
			TextCase.ConvertToSnakeCaseFromPascalCase(name);

		string compareText = "landing_pages";

		Assert.That(output, Is.EqualTo(compareText));
	}

	/// <summary>
	/// get casing for camel case test.
	/// </summary>
	[Test]
	public static void GetCasingCamelCase()
	{
		string name = "nameEnglish";

		CaseTypes caseTypes = TextCase.GetCaseTypes(name);

		bool isCamelCase = caseTypes.HasFlag(CaseTypes.CamelCase);
		bool isKabobCase = caseTypes.HasFlag(CaseTypes.KabobCase);
		bool isPascalCase = caseTypes.HasFlag(CaseTypes.PascalCase);
		bool isSnakeCase = caseTypes.HasFlag(CaseTypes.SnakeCase);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(isCamelCase, Is.True);
			Assert.That(isKabobCase, Is.False);
			Assert.That(isPascalCase, Is.False);
			Assert.That(isSnakeCase, Is.False);
		}
	}

	/// <summary>
	/// get casing for single word upper first letter upper case test.
	/// </summary>
	[Test]
	public static void GetCasingSingleWordFirstUpperCase()
	{
		string name = "Brands";

		CaseTypes caseTypes = TextCase.GetCaseTypes(name);

		bool isCamelCase = caseTypes.HasFlag(CaseTypes.CamelCase);
		bool isKabobCase = caseTypes.HasFlag(CaseTypes.KabobCase);
		bool isPascalCase = caseTypes.HasFlag(CaseTypes.PascalCase);
		bool isSnakeCase = caseTypes.HasFlag(CaseTypes.SnakeCase);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(isCamelCase, Is.False);
			Assert.That(isKabobCase, Is.False);
			Assert.That(isPascalCase, Is.True);
			Assert.That(isSnakeCase, Is.False);
		}
	}

	/// <summary>
	/// get casing for single word lower case test.
	/// </summary>
	[Test]
	public static void GetCasingSingleWordLowerCase()
	{
		string name = "brands";

		CaseTypes caseTypes = TextCase.GetCaseTypes(name);

		bool isCamelCase = caseTypes.HasFlag(CaseTypes.CamelCase);
		bool isKabobCase = caseTypes.HasFlag(CaseTypes.KabobCase);
		bool isPascalCase = caseTypes.HasFlag(CaseTypes.PascalCase);
		bool isSnakeCase = caseTypes.HasFlag(CaseTypes.SnakeCase);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(isCamelCase, Is.True);
			Assert.That(isKabobCase, Is.True);
			Assert.That(isPascalCase, Is.False);
			Assert.That(isSnakeCase, Is.True);
		}
	}

	/// <summary>
	/// get casing for single word upper case test.
	/// </summary>
	[Test]
	public static void GetCasingSingleWordUpperCase()
	{
		string name = "BRANDS";

		CaseTypes caseTypes = TextCase.GetCaseTypes(name);

		bool isCamelCase = caseTypes.HasFlag(CaseTypes.CamelCase);
		bool isKabobCase = caseTypes.HasFlag(CaseTypes.KabobCase);
		bool isPascalCase = caseTypes.HasFlag(CaseTypes.PascalCase);
		bool isSnakeCase = caseTypes.HasFlag(CaseTypes.SnakeCase);
		bool isOtherCase = caseTypes.HasFlag(CaseTypes.Other);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(isCamelCase, Is.False);
			Assert.That(isKabobCase, Is.False);
			Assert.That(isPascalCase, Is.False);
			Assert.That(isSnakeCase, Is.False);
			Assert.That(isOtherCase, Is.True);
		}
	}

	/// <summary>
	/// get casing for pascal case test.
	/// </summary>
	[Test]
	public static void GetCasingPascalCase()
	{
		string name = "NameEnglish";

		CaseTypes caseTypes = TextCase.GetCaseTypes(name);

		bool isCamelCase = caseTypes.HasFlag(CaseTypes.CamelCase);
		bool isKabobCase = caseTypes.HasFlag(CaseTypes.KabobCase);
		bool isPascalCase = caseTypes.HasFlag(CaseTypes.PascalCase);
		bool isSnakeCase = caseTypes.HasFlag(CaseTypes.SnakeCase);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(isCamelCase, Is.False);
			Assert.That(isKabobCase, Is.False);
			Assert.That(isPascalCase, Is.True);
			Assert.That(isSnakeCase, Is.False);
		}
	}

	/// <summary>
	/// get casing for snake case test.
	/// </summary>
	[Test]
	public static void GetCasingSnakeCase()
	{
		string name = "name_english";

		CaseTypes caseTypes = TextCase.GetCaseTypes(name);

		bool isCamelCase = caseTypes.HasFlag(CaseTypes.CamelCase);
		bool isKabobCase = caseTypes.HasFlag(CaseTypes.KabobCase);
		bool isPascalCase = caseTypes.HasFlag(CaseTypes.PascalCase);
		bool isSnakeCase = caseTypes.HasFlag(CaseTypes.SnakeCase);

		using (Assert.EnterMultipleScope())
		{
			Assert.That(isCamelCase, Is.False);
			Assert.That(isKabobCase, Is.False);
			Assert.That(isPascalCase, Is.False);
			Assert.That(isSnakeCase, Is.True);
		}
	}

	/// <summary>
	/// Flatten test.
	/// </summary>
	[Test]
	public static void ExtractContentTest()
	{
		string content = "[:en]\r\nThis\r\nis a\r\ntest.[:ja]\r\n" +
			"<section class=\"slice sct-color-1 bb\">\r\n[:]";
		string compareText = "This\r\nis a\r\ntest.";

		string? innerContent = FileUtils.ExtractContent(
			content, @"\[:en\]\r\n", @"\[:ja\]\r\n");

		Assert.That(innerContent, Is.EqualTo(compareText));
	}

	/// <summary>
	/// Flatten test.
	/// </summary>
	[Test]
	public static void Flatten()
	{
		DirectoryInfo tempDirectoryTop = Directory.CreateTempSubdirectory();
		string tempDirectoryTopPath = tempDirectoryTop.FullName;

		string subPath1 = tempDirectoryTopPath + Path.DirectorySeparatorChar +
			"TestDirectory1" + Path.DirectorySeparatorChar + "TestSubDirectory1";

		string subPath2 = tempDirectoryTopPath + Path.DirectorySeparatorChar +
			"TestDirectory2" + Path.DirectorySeparatorChar + "TestSubDirectory2";

		Directory.CreateDirectory(subPath1);
		Directory.CreateDirectory(subPath2);

		string file1 = Path.Combine(subPath1, "log1.txt");
		File.WriteAllText(file1, "some text");

		string file2 = Path.Combine(subPath2, "log2.txt");
		File.WriteAllText(file2, "some text");

		FileUtils.Flatten(tempDirectoryTopPath);

		file1 = tempDirectoryTopPath + "_" + "TestDirectory1" + "_" +
			"TestSubDirectory1" + "_log1.txt";

		file2 = tempDirectoryTopPath + "_" + "TestDirectory2" + "_" +
			"TestSubDirectory2" + "_log2.txt";

		bool result = File.Exists(file1);
		Assert.That(result, Is.True);

		if (result == true)
		{
			File.Delete(file1);
		}

		result = File.Exists(file2);
		Assert.That(result, Is.True);

		if (result == true)
		{
			File.Delete(file2);
		}

		Directory.Delete(tempDirectoryTopPath, true);
	}

	/// <summary>
	/// Get embedded resource test.
	/// </summary>
	[Test]
	public static void GetEmbeddedResource()
	{
		string filePath = GetXmlResourceFile(
			"DigitalZenWorks.Common.Utilities.Tests.test.xml", ".xml");

		bool result = File.Exists(filePath);
		Assert.That(result, Is.True);

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

		byte[]? hash = FileUtils.GetFileHash(filePath);
		Assert.That(hash, Is.Not.Null);

		string hashText = BitConverter.ToString(hash);
		hashText = hashText.Replace(
			"-", string.Empty, StringComparison.Ordinal);

		string compareHash = "3163EC56BAB76F5BA3C2CFAC49915EE83C10BC" +
			"4975FD70BB01F4B30501AA9856";

		Assert.That(compareHash, Is.EqualTo(hashText));

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
		string xmlFilePath = GetXmlResourceFile(
			"DigitalZenWorks.Common.Utilities.Tests.test.xml", ".xml");
		bool result = File.Exists(xmlFilePath);
		Assert.That(result, Is.True);

		string xsdFilePath = GetXmlResourceFile(
			"DigitalZenWorks.Common.Utilities.Tests.test.xsd", ".xsd");
		result = File.Exists(xmlFilePath);
		Assert.That(result, Is.True);

		OrderedItem? item = (OrderedItem?)XmlUtilities.LoadWithValidation(
			xsdFilePath, xmlFilePath, typeof(OrderedItem));

		using (Assert.EnterMultipleScope())
		{
			Assert.That(item, Is.Not.Null);
			Assert.That(item!.ItemName, Is.EqualTo("Widget"));
			Assert.That(item.Description, Is.EqualTo("Regular Widget"));
			Assert.That(item.UnitPrice, Is.EqualTo(2.3));
			Assert.That(item.Quantity, Is.EqualTo(10));
			Assert.That(item.LineTotal, Is.EqualTo(23));
		}

		if (result == true)
		{
			File.Delete(xmlFilePath);
			File.Delete(xsdFilePath);
		}
	}

	/// <summary>
	/// To camel case test.
	/// </summary>
	[Test]
	public static void ToCamelCase()
	{
		string? title = "WAR AND PEACE";

		title = title.ToCamelCase();

		string expected = "warAndPeace";

		Assert.That(title, Is.EqualTo(expected));
	}

	/// <summary>
	/// To camel case test.
	/// </summary>
	[Test]
	public static void ToCamelCaseFromSnakeCase()
	{
		string? title = "war_and_peace";

		title = title.ToCamelCase();

		string expected = "warAndPeace";

		Assert.That(title, Is.EqualTo(expected));
	}

	/// <summary>
	/// To title case test.
	/// </summary>
	[Test]
	public static void ToHex()
	{
		string result = GeneralUtilities.ToHex("2048");

		string expected = "32303438";

		Assert.That(result, Is.EqualTo(expected));
	}

	/// <summary>
	/// To title case test.
	/// </summary>
	[Test]
	public static void ToHexBytes()
	{
		byte[] input = [50, 48, 52, 56];

		string result = GeneralUtilities.ToHex(input);

		string expected = "32303438";

		Assert.That(result, Is.EqualTo(expected));
	}

	/// <summary>
	/// To camel case test.
	/// </summary>
	[Test]
	public static void ToPascalCase()
	{
		string? title = "WAR AND PEACE";

		title = title.ToPascalCase();

		string expected = "WarAndPeace";

		Assert.That(title, Is.EqualTo(expected));
	}

	/// <summary>
	/// To camel case test.
	/// </summary>
	[Test]
	public static void ToPascalCaseFromSnakeCase()
	{
		string? title = "war_and_peace";

		title = title.ToPascalCase();

		string expected = "WarAndPeace";

		Assert.That(title, Is.EqualTo(expected));
	}

	/// <summary>
	/// To title case test.
	/// </summary>
	[Test]
	public static void ToTitleCase()
	{
		string? title = "WAR AND PEACE";

		title = title.ToTitleCase();

		string expected = "War and Peace";

		Assert.That(title, Is.EqualTo(expected));
	}

	/// <summary>
	/// To title case test.
	/// </summary>
	[Test]
	public static void ToTitleCaseBegin()
	{
		string? title = "THE WAY WE WERE";

		title = title.ToTitleCase();

		string expected = "The Way We Were";

		Assert.That(title, Is.EqualTo(expected));
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

		Assert.That(result, Is.True);

		return filePath;
	}
}
