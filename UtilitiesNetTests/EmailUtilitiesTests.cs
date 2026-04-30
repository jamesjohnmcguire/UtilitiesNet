/////////////////////////////////////////////////////////////////////////////
// <copyright file="EmailUtilitiesTests.cs" company="Digital Zen Works">
// Copyright © 2006 - 2026 Digital Zen Works.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities.Tests;

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// The email utilities tests class.
/// </summary>
[TestFixture]
internal sealed class EmailUtilitiesTests
{
	/// <summary>
	/// Is valid email address full true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressFullTrue()
	{
		string emailAddress = "user@example.com";

		var tests = new (string Label, string Pattern)[]
		{
	("No consecutive dots",      @"^(?!.*\.\.)"),
	("No leading dot",           @"^(?!\.)"),
	("Local part chars",         @"^[a-zA-Z0-9._%+\-]{1,64}@"),
	("Domain no leading hyphen", @"@(?!-)"),
	("Domain start alphanum",    @"@[a-zA-Z0-9]"),
	("Domain middle",            @"@[a-zA-Z0-9][a-zA-Z0-9\-]{0,61}[a-zA-Z0-9]?"),
	("Sub-domain labels",        @"(\.[a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?)*"),
	("TLD",                      @"\.[a-zA-Z]{2,}$"),
		};

		foreach (var (label, pattern) in tests)
		{
			bool pass = Regex.IsMatch(emailAddress, pattern, RegexOptions.IgnoreCase);
			Console.WriteLine($"{(pass ? "PASS" : "FAIL")} : {label}");
		}

		bool result = EmailUtilities.IsValidEmailAddress(emailAddress);
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address full false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressFullFalise()
	{
		bool result = EmailUtilities.IsValidEmailAddress("bad-input");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address partial full true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressPartialFullTrue()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user@example.com", true);
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address partial full spaces true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressPartialFullSpacesTrue()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("  user@example.com ", true);
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address partial full multi-part true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressPartialFullMultiPartTrue()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user@example.co.jp", true);
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address partial no local part false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressPartialNoLocalPartFalse()
	{
		bool result = EmailUtilities.IsValidEmailAddress("@example.com", true);
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address partial no TLD true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressPartialNoTldTrue()
	{
		bool result = EmailUtilities.IsValidEmailAddress("user@example", true);
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address partial no dot TLD true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressPartialDotNoTldTrue()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user@example.", true);
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address partial full false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressPartialFullFalse()
	{
		bool result = EmailUtilities.IsValidEmailAddress("bad-input", true);
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address partial domain dot false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressPartialDomainDotFalse()
	{
		bool result = EmailUtilities.IsValidEmailAddress("user@.com", true);
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address partial no at false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressPartialNoAtFalse()
	{
		bool result = EmailUtilities.IsValidEmailAddress("plaintext", true);
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address partial no domain false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressPartialNoDomainFalse()
	{
		bool result = EmailUtilities.IsValidEmailAddress("missing@", true);
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Regex parts true test.
	/// </summary>
	[Test]
	public void RegexPartsTrue()
	{
		string emailAddress = "user@example.com";

		var tests = new (string Label, string Pattern)[]
		{
			("No consecutive dots",      @"^(?!.*\.\.)"),
			("No leading dot",           @"^(?!\.)"),
			("Local part chars",         @"^[a-zA-Z0-9._%+\-]{1,64}@"),
			("Domain no leading hyphen", @"@(?!-)"),
			("Domain start alphanum",    @"@[a-zA-Z0-9]"),
			("Domain middle",            @"@[a-zA-Z0-9][a-zA-Z0-9\-]{0,61}[a-zA-Z0-9]?"),
			("Sub-domain labels",        @"(\.[a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?)*"),
			("TLD",                      @"\.[a-zA-Z]{2,}$"),
		};

		foreach (var (label, pattern) in tests)
		{
			bool pass = Regex.IsMatch(emailAddress, pattern, RegexOptions.IgnoreCase);
			Assert.That(pass, Is.True, label);
		}
	}

}
