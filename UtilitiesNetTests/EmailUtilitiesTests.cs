/////////////////////////////////////////////////////////////////////////////
// <copyright file="EmailUtilitiesTests.cs" company="Digital Zen Works">
// Copyright © 2006 - 2026 Digital Zen Works.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities.Tests;

using System.Text.RegularExpressions;
using NUnit.Framework;

/// <summary>
/// The email utilities tests class.
/// </summary>
[TestFixture]
internal sealed class EmailUtilitiesTests
{
	/// <summary>
	/// Is valid email address edge cases test.
	/// </summary>
	/// <param name="email">The email address to test.</param>
	/// <param name="expected">The expected result of the validation.</param>
	[TestCase(
		"user..x@example.com",
		false,
		TestName = "IsValidEmailConsecutiveDots")]
	[TestCase(
		".user@example.com",
		false,
		TestName = "IsValidEmailLeadingDotLocal")]
	[TestCase(
		"user@-example.com",
		false,
		TestName = "IsValidEmailLeadingHyphenDomain")]
	[TestCase(
		"user.@example.com",
		false,
		TestName = "IsValidEmailTrailingDotLocal")]
	[TestCase(
		"user@example-.com",
		false,
		TestName = "IsValidEmailTrailingHyphenDomain")]
	[TestCase(
		"user@example.com",
		true,
		TestName = "IsValidEmailValidBaseline")]
	public void IsValidEmailAddressEdgeCases(string email, bool expected)
	{
		bool result = EmailUtilities.IsValidEmailAddress(email);
		Assert.That(result, Is.EqualTo(expected));
	}

	/// <summary>
	/// Is valid email address full false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressFullFalse()
	{
		bool result = EmailUtilities.IsValidEmailAddress("bad-input");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address full true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressFullTrue()
	{
		string emailAddress = "user@example.com";

		bool result = EmailUtilities.IsValidEmailAddress(emailAddress);
		Assert.That(result, Is.True);
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
	/// Regex parts true test.
	/// </summary>
	[Test]
	public void RegexPartsTrue()
	{
		string emailAddress = "user@example.com";

		(string Label, string Pattern)[] tests = new (string Label, string Pattern)[]
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

		foreach ((string? label, string? pattern) in tests)
		{
			bool pass = Regex.IsMatch(emailAddress, pattern, RegexOptions.IgnoreCase);
			Assert.That(pass, Is.True, label);
		}
	}

	/// <summary>
	/// Is valid email address valid subdomain true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressValidSubdomainTrue()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user@mail.example.com");
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address valid multi-part TLD true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressValidMultiPartTldTrue()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user@mail.example.co.jp");
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address valid plus tag true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressValidPlusTagTrue()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user+tag@example.com");
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address valid dots true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressValidDotsTrue()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("first.last@example.com");
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address valid long TLD true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressValidLongTldTrue()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user@example.photography");
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address valid mixed case true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressValidMixedCaseTrue()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("User@Example.COM");
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address valid with whitespace true test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressValidWithWhitespaceTrue()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("  user@example.com  ");
		Assert.That(result, Is.True);
	}

	/// <summary>
	/// Is valid email address null input false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressNullInputFalse()
	{
		// Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8625
		bool result =
			EmailUtilities.IsValidEmailAddress(null);
#pragma warning restore CS8625
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address empty input false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressEmptyInputFalse()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress(string.Empty);
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address whitespace only false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressWhitespaceOnlyFalse()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("   ");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address no @ sign false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressNoAtSignFalse()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("userexample.com");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address invalid characters false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressNoLocalPartFalse()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("@example.com");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address no domain false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressNoDomainFalse()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user@");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address no TLD false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressNoTldFalse()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user@domain");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address consecutive dots false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressConsecutiveDotsFalse()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user..name@example.com");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address leading dot in local part false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressLeadingDotLocalFalse()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress(".user@example.com");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address trailing dot in local part false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressTrailingDotLocalFalse()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user.@example.com");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address leading hyphen in domain false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressHyphenLeadingDomainFalse()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user@-example.com");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address trailing hyphen in domain false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressHyphenTrailingDomainFalse()
	{
		bool result =
			EmailUtilities.IsValidEmailAddress("user@example-.com");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address domain label too long false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressExceedsMaxLengthFalse()
	{
		string longLocal = new string('a', 65);
		bool result =
			EmailUtilities.IsValidEmailAddress($"{longLocal}@example.com");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address total length exceeds maximum false test.
	/// </summary>
	[Test]
	public void IsValidEmailAddressExceedsTotalMaxLengthFalse()
	{
		string longLocal = new string('a', 250);
		bool result =
			EmailUtilities.IsValidEmailAddress($"{longLocal}@example.com");
		Assert.That(result, Is.False);
	}

	/// <summary>
	/// Is valid email address valid long local part true test.
	/// </summary>
	[Test]
	public void NormalizeMixedCaseEmailReturnsLowerCase()
	{
		string expected = "user@example.com";
		string email = EmailUtilities.Normalize("User@Example.COM");

		Assert.That(email, Is.EqualTo(expected));
	}

	/// <summary>
	/// Is valid email address leading and trailing whitespace returns trimmed
	/// test.
	/// </summary>
	[Test]
	public void NormalizeLeadingTrailingWhitespaceReturnsTrimmed()
	{
		string expected = "user@example.com";
		string email = EmailUtilities.Normalize("  user@example.com  ");

		Assert.That(email, Is.EqualTo(expected));
	}

	/// <summary>
	/// Is valid email address normalize googlemail domain returns gmail test.
	/// </summary>
	[Test]
	public void NormalizeGooglemailDomainReturnsGmail()
	{
		string expected = "user@gmail.com";
		string email = EmailUtilities.Normalize("user@googlemail.com");

		Assert.That(email, Is.EqualTo(expected));
	}

	/// <summary>
	/// Normalize plus tag strip tag enabled returns stripped test.
	/// </summary>
	[Test]
	public void NormalizePlusTagStripTagEnabledReturnsStripped()
	{
		string expected = "user@example.com";
		string email = EmailUtilities.Normalize(
			"user+newsletter@example.com", stripPlusTag: true);

		Assert.That(email, Is.EqualTo(expected));
	}

	/// <summary>
	/// Normalize plus tag strip tag disabled returns full test.
	/// </summary>
	[Test]
	public void NormalizePlusTagStripTagDisabledReturnsFull()
	{
		string expected = "user+newsletter@example.com";
		string email = EmailUtilities.Normalize(
			"user+newsletter@example.com", stripPlusTag: false);

		Assert.That(email, Is.EqualTo(expected));
	}

	/// <summary>
	/// Normalize Gmail dots strip dots enabled returns stripped test.
	/// </summary>
	[Test]
	public void NormalizeGmailDotsStripDotsEnabledReturnsStripped()
	{
		string expected = "user@gmail.com";
		string email = EmailUtilities.Normalize(
			"u.s.e.r@gmail.com", stripGmailDots: true);

		Assert.That(email, Is.EqualTo(expected));
	}

	/// <summary>
	/// Normalize Gmail dots strip dots disabled returns full test.
	/// </summary>
	[Test]
	public void NormalizeGmailDotsStripDotsDisabledReturnsFull()
	{
		string expected = "u.s.e.r@gmail.com";
		string email = EmailUtilities.Normalize(
			"u.s.e.r@gmail.com", stripGmailDots: false);

		Assert.That(email, Is.EqualTo(expected));
	}

	/// <summary>
	/// Normalize non-Gmail dots strip dots enabled does not strip test.
	/// </summary>
	[Test]
	public void NormalizeNonGmailDotsStripDotsEnabledDoesNotStrip()
	{
		// Dot stripping is Gmail-specific — should not affect other domains
		string expected = "u.s.e.r@example.com";
		string email = EmailUtilities.Normalize(
			"u.s.e.r@example.com", stripGmailDots: true);

		Assert.That(email, Is.EqualTo(expected));
	}

	/// <summary>
	/// Normalize Gmail plus and dots both enabled returns fully stripped test.
	/// </summary>
	[Test]
	public void NormalizeGmailPlusAndDotsBothEnabledReturnsFullyStripped()
	{
		string expected = "user@gmail.com";
		string email = EmailUtilities.Normalize(
			"U.S.E.R+tag@Gmail.com",
			stripPlusTag: true,
			stripGmailDots: true);

		Assert.That(email, Is.EqualTo(expected));
	}

	/// <summary>
	/// Normalize null input returns empty test.
	/// </summary>
	[Test]
	public void NormalizeNullInputReturnsEmpty()
	{
		string expected = string.Empty;

		// Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8625
		string email = EmailUtilities.Normalize(null);
#pragma warning restore CS8625

		Assert.That(email, Is.EqualTo(expected));
	}

	/// <summary>
	/// Normalize empty input returns empty test.
	/// </summary>
	[Test]
	public void NormalizeEmptyInputReturnsEmpty()
	{
		string expected = string.Empty;
		string email = EmailUtilities.Normalize(string.Empty);

		Assert.That(email, Is.EqualTo(expected));
	}

	/// <summary>
	/// Get domain base from email simple domain returns domain base test.
	/// </summary>
	[Test]
	public void GetDomainBaseFromEmailSimpleDomainReturnsDomainBase()
	{
		string expected = "example";
		string domanBase =
			EmailUtilities.GetDomainBaseFromEmail("user@example.com");

		Assert.That(domanBase, Is.EqualTo(expected));
	}

	/// <summary>
	/// Get domain base from email with subdomain returns domain base test.
	/// </summary>
	[Test]
	public void GetDomainBaseFromEmailWithSubdomainReturnsDomainBase()
	{
		string expected = "example";
		string domanBase =
			EmailUtilities.GetDomainBaseFromEmail("user@mail.example.com");

		Assert.That(domanBase, Is.EqualTo(expected));
	}

	/// <summary>
	/// Get domain base from email with multi-part TLD returns domain base test.
	/// </summary>
	[Test]
	public void GetDomainBaseFromEmailMultiPartTldReturnsDomainBase()
	{
		string expected = "example";
		string domanBase =
			EmailUtilities.GetDomainBaseFromEmail("user@mail.example.co.jp");

		Assert.That(domanBase, Is.EqualTo(expected));
	}

	/// <summary>
	/// Get domain base from email mixed case returns lower case test.
	/// </summary>
	[Test]
	public void GetDomainBaseFromEmailMixedCaseReturnsLowerCase()
	{
		string expected = "example";
		string domainBase =
			EmailUtilities.GetDomainBaseFromEmail("user@Example.COM");

		Assert.That(domainBase, Is.EqualTo(expected));
	}

	/// <summary>
	/// Get domain base from email null input returns empty test.
	/// </summary>
	[Test]
	public void GetDomainBaseFromEmailNullInputReturnsEmpty()
	{
		string expected = string.Empty;

		// Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8625
		string domainBase = EmailUtilities.GetDomainBaseFromEmail(null);
#pragma warning restore CS8625

		Assert.That(domainBase, Is.EqualTo(expected));
	}

	/// <summary>
	/// Get domain base from email no @ sign returns empty test.
	/// </summary>
	[Test]
	public void GetDomainBaseFromEmailNoAtSignReturnsEmpty()
	{
		string expected = string.Empty;
		string domainBase = EmailUtilities.GetDomainBaseFromEmail("notanemail");

		Assert.That(domainBase, Is.EqualTo(expected));
	}

	/// <summary>
	/// Get TLD from email simple TLD returns TLD test.
	/// </summary>
	[Test]
	public void GetTldFromEmailSimpleTldReturnsTld()
	{
		string expected = "com";
		string tld = EmailUtilities.GetTldFromEmail("user@example.com");

		Assert.That(tld, Is.EqualTo(expected));
	}

	/// <summary>
	/// Get TLD from email multi-part TLD returns full TLD test.
	/// </summary>
	[Test]
	public void GetTldFromEmailMultiPartTldReturnsFullTld()
	{
		string expected = "co.jp";
		string tld = EmailUtilities.GetTldFromEmail("user@example.co.jp");

		Assert.That(tld, Is.EqualTo(expected));
	}

	/// <summary>
	/// Get TLD from email multi-part TLD with known multi-part returns full
	/// TLD test.
	/// </summary>
	[Test]
	public void GetTldFromEmailComAuTldReturnsFullTld()
	{
		string expected = "com.au";
		string tld = EmailUtilities.GetTldFromEmail("user@example.com.au");

		Assert.That(tld, Is.EqualTo(expected));
	}

	/// <summary>
	/// Get TLD from email mixed case returns lower case test.
	/// </summary>
	[Test]
	public void GetTldFromEmailMixedCaseReturnsLowerCase()
	{
		string expected = "co.jp";
		string tld = EmailUtilities.GetTldFromEmail("user@Example.CO.JP");

		Assert.That(tld, Is.EqualTo(expected));
	}

	/// <summary>
	/// Get TLD from email null input returns empty test.
	/// </summary>
	[Test]
	public void GetTldFromEmailNullInputReturnsEmpty()
	{
		string expected = string.Empty;

		// Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8625
		string tld = EmailUtilities.GetTldFromEmail(null);
#pragma warning restore CS8625

		Assert.That(tld, Is.EqualTo(expected));
	}

	/// <summary>
	/// Parse email parts simple email returns correct parts test.
	/// </summary>
	[Test]
	public void ParseEmailPartsSimpleEmailReturnsCorrectParts()
	{
		var (local, subdomain, domainBase, tld) =
			EmailUtilities.ParseEmailParts("user@example.com");

		Assert.That(local, Is.EqualTo("user"));
		Assert.That(subdomain, Is.EqualTo(string.Empty));
		Assert.That(domainBase, Is.EqualTo("example"));
		Assert.That(tld, Is.EqualTo("com"));
	}

	/// <summary>
	/// Parse email parts with subdomain returns correct parts test.
	/// </summary>
	[Test]
	public void ParseEmailPartsWithSubdomainReturnsCorrectParts()
	{
		var (local, subdomain, domainBase, tld) =
			EmailUtilities.ParseEmailParts("user@mail.example.com");

		Assert.That(local, Is.EqualTo("user"));
		Assert.That(subdomain, Is.EqualTo("mail"));
		Assert.That(domainBase, Is.EqualTo("example"));
		Assert.That(tld, Is.EqualTo("com"));
	}

	/// <summary>
	/// Parse email parts with multi-part TLD returns correct parts test.
	/// </summary>
	[Test]
	public void ParseEmailPartsMultiPartTldReturnsCorrectParts()
	{
		var (local, subdomain, domainBase, tld) =
			EmailUtilities.ParseEmailParts("user@mail.example.co.jp");

		Assert.That(local, Is.EqualTo("user"));
		Assert.That(subdomain, Is.EqualTo("mail"));
		Assert.That(domainBase, Is.EqualTo("example"));
		Assert.That(tld, Is.EqualTo("co.jp"));
	}

	/// <summary>
	/// Parse email parts mixed case returns lower case test.
	/// </summary>
	[Test]
	public void ParseEmailPartsMixedCaseReturnsLowerCase()
	{
		var (local, subdomain, domainBase, tld) =
			EmailUtilities.ParseEmailParts("User@Mail.Example.CO.JP");

		Assert.That(local, Is.EqualTo("user"));
		Assert.That(subdomain, Is.EqualTo("mail"));
		Assert.That(domainBase, Is.EqualTo("example"));
		Assert.That(tld, Is.EqualTo("co.jp"));
	}

	/// <summary>
	/// Parse email parts null input returns all empty test.
	/// </summary>
	[Test]
	public void ParseEmailPartsNullInputReturnsAllEmpty()
	{
		// Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8625
		var (local, subdomain, domainBase, tld) =
			EmailUtilities.ParseEmailParts(null);
#pragma warning restore CS8625

		Assert.That(local, Is.EqualTo(string.Empty));
		Assert.That(subdomain, Is.EqualTo(string.Empty));
		Assert.That(domainBase, Is.EqualTo(string.Empty));
		Assert.That(tld, Is.EqualTo(string.Empty));
	}

	/// <summary>
	/// Parse email parts no @ sign returns all empty test.
	/// </summary>
	[Test]
	public void ParseEmailPartsNoAtSignReturnsAllEmpty()
	{
		var (local, subdomain, domainBase, tld) =
			EmailUtilities.ParseEmailParts("notanemail");

		Assert.That(local, Is.EqualTo(string.Empty));
		Assert.That(subdomain, Is.EqualTo(string.Empty));
		Assert.That(domainBase, Is.EqualTo(string.Empty));
		Assert.That(tld, Is.EqualTo(string.Empty));
	}
}
