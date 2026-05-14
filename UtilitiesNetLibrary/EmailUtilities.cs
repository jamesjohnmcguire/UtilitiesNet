/////////////////////////////////////////////////////////////////////////////
// <copyright file="EmailUtilities.cs" company="Digital Zen Works">
// Copyright © 2006 - 2026 Digital Zen Works.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

/// <summary>
/// Provides utility methods for validating email addresses according to common
/// formatting rules and RFC 5321 constraints.
/// </summary>
/// <remarks>This class is intended for use in scenarios where email address
/// validation is required before processing or storing user input. The
/// validation performed aims to catch common formatting errors and adheres to
/// standard email address specifications. All members are static and
/// thread-safe.</remarks>
public static class EmailUtilities
{
	/// <summary>
	/// Represents the regular expression pattern used to validate email addresses according to common formatting rules and
	/// RFC 5321 constraints.
	/// </summary>
	/// <remarks>The pattern enforces several restrictions, including
	/// prohibiting consecutive dots, ensuring the local part does not start
	/// with a dot, limiting the local part to 64 characters, and requiring
	/// valid domain label formatting. This pattern is intended for use in
	/// email validation scenarios where compliance with standard email
	/// address formats is required.</remarks>
	private const string RegexPattern =

		// no consecutive dots anywhere
		@"^(?!.*\.\.)" +

		// local part cannot start with a dot
		@"(?!\.)" +

		// local part can't end with dot
		@"(?!.*\.@)" +

		// local part, max 64 chars (RFC 5321)
		@"[a-zA-Z0-9._%+\-]{1,64}" +
		@"@" +

		// domain label can't start with hyphen
		@"(?!-)" +

		// domain label can't end with hyphen before a dot
		@"(?!.*-\.)" +

		// domain label can't end with hyphen at end of string
		@"(?!.*-$)" +

		// domain must start with alphanumeric
		@"[a-zA-Z0-9]" +

		// middle of first label (max 63 total)
		@"[a-zA-Z0-9\-]{0,61}" +

		// domain label can't end with hyphen
		@"[a-zA-Z0-9]?" +

		// further labels
		@"(\.[a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?)*" +

		// TLD, no upper cap
		@"\.[a-zA-Z]{2,}$";

	private static readonly Regex EmailRegex = new Regex(
		RegexPattern,
		RegexOptions.Compiled | RegexOptions.IgnoreCase);

	// Partial: has @ and at least a domain, but no/incomplete TLD
	// e.g. "user@domain" or "user@domain.".
	private static readonly Regex PartialEmailRegex = new Regex(
		@"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+$",
		RegexOptions.Compiled | RegexOptions.IgnoreCase);

	// Known multi-part TLDs — extend as needed
	private static readonly HashSet<string> KnownMultiPartTlds = new(
		StringComparer.OrdinalIgnoreCase)
	{
		"co.uk", "co.jp", "co.nz", "co.za", "co.in",
		"com.au", "com.br", "com.mx", "com.sg", "com.hk",
		"org.uk", "net.au", "gov.uk", "ac.uk", "me.uk"
	};

	/// <summary>
	/// Determines whether the specified string is a valid email address
	/// according to standard email address formatting rules.
	/// </summary>
	/// <remarks>The validation checks for compliance with common email
	/// address formatting rules, including length and structure. The method
	/// does not verify that the email address exists or is deliverable.
	/// </remarks>
	/// <param name="emailAddress">The email address to validate. May not be
	/// null, empty, or consist only of white-space characters.</param>
	/// <param name="allowPartialMatch">
	/// When true, strings like "user@domain" (missing TLD) return
	/// PartialMatch instead of Invalid.
	/// </param>
	/// <returns>true if the specified string is a valid email address;
	/// otherwise, false.</returns>
	public static bool IsValidEmailAddress(
		string emailAddress,
		bool allowPartialMatch = false)
	{
		bool valid = false;

		if (!string.IsNullOrWhiteSpace(emailAddress))
		{
			emailAddress = emailAddress.Trim();

			// RFC 5321 max total length
			if (emailAddress.Length < 255)
			{
				try
				{
					// MailAddress as first gate —
					// catches structural issues cheaply.
					MailAddress addressObject = new(emailAddress);

					if (addressObject.Address == emailAddress)
					{
						bool fullValid = EmailRegex.IsMatch(emailAddress);

						if (fullValid == true)
						{
							valid = true;
						}
						else if (allowPartialMatch == true)
						{
							valid = PartialEmailRegex.IsMatch(emailAddress);
						}
					}
				}
				catch (Exception exception) when
					(exception is ArgumentException ||
					exception is FormatException)
				{
				}
			}
		}

		return valid;
	}

	/// <summary>
	/// Normalizes an email address for storage and comparison.
	/// Does NOT modify for delivery — use the original address for that.
	/// </summary>
	/// <param name="emailAddress">Raw input email address.</param>
	/// <param name="stripPlusTag">
	/// If true, removes plus-addressing tags (user+tag@domain → user@domain).
	/// Useful for deduplication; do NOT use for actual mail delivery.
	/// </param>
	/// <param name="stripGmailDots">
	/// If true, removes dots from Gmail/Googlemail local parts.
	/// Useful for deduplication only.
	/// </param>
	/// <returns>A normalized version of the email address, with consistent
	/// casing and optional deduplication adjustments.</returns>
	public static string Normalize(
		string emailAddress,
		bool stripPlusTag = false,
		bool stripGmailDots = false)
	{
		string normalizedEmail = string.Empty;

		if (!string.IsNullOrWhiteSpace(emailAddress))
		{
			emailAddress = emailAddress.Trim();

			// Split into local and domain parts
			int atIndex = emailAddress.LastIndexOf('@');

			if (atIndex < 0)
			{
				// not a valid email, return as-is
				normalizedEmail = emailAddress;
			}
			else
			{
#if NET5_0_OR_GREATER
				string local = emailAddress[..atIndex];
				string domain = emailAddress[(atIndex + 1)..];
#else
				string local = emailAddress.Substring(0, atIndex);
				string domain = emailAddress.Substring(atIndex + 1);
#endif

				// Lowercase the domain — always safe per RFC
#pragma warning disable CA1308
				domain = domain.ToLowerInvariant();
#pragma warning restore CA1308

				// Normalize googlemail.com → gmail.com
				if (domain == "googlemail.com")
				{
					domain = "gmail.com";
				}

				// Lowercase the local part — technically optional but safe for
				// virtually all real-world mail servers
#pragma warning disable CA1308
				local = local.ToLowerInvariant();
#pragma warning restore CA1308

				// Strip plus tag if requested (deduplication only)
				if (stripPlusTag == true)
				{
#if NET5_0_OR_GREATER
					int plusIndex =
						local.IndexOf('+', StringComparison.Ordinal);

					if (plusIndex >= 0)
					{
						local = local[..plusIndex];
					}
#else
					int plusIndex = local.IndexOf('+');

					if (plusIndex >= 0)
					{
						local = local.Substring(0, plusIndex);
					}
#endif
				}

				// Strip Gmail dots if requested (deduplication only)
				if (stripGmailDots == true && domain == "gmail.com")
				{
#if NET5_0_OR_GREATER
					local = local.Replace(
						".", string.Empty, StringComparison.Ordinal);
#else
					local = local.Replace(".", string.Empty);
#endif
				}

				normalizedEmail = $"{local}@{domain}";
			}
		}

		return normalizedEmail;
	}

	/// <summary>
	/// Extracts the second-level domain (SLD) from an email address.
	/// e.g. "user@mail.example.co.jp" → "example"
	///      "user@example.com"        → "example".
	/// </summary>
	/// <param name="emailAddress">The email address to extract the domain base
	/// from.</param>
	/// <returns>The second-level domain (SLD) if extraction is successful;
	/// otherwise, an empty string.</returns>
	public static string GetDomainBaseFromEmail(string emailAddress)
	{
		string domainBase = string.Empty;

		if (!string.IsNullOrWhiteSpace(emailAddress))
		{
			string normalized = Normalize(emailAddress);

			int atIndex = normalized.LastIndexOf('@');

			if (atIndex > 0 && atIndex < normalized.Length - 1)
			{
				// "mail.example.co.jp"
#if NET5_0_OR_GREATER
				string domain = normalized[(atIndex + 1)..];
#else
				string domain = normalized.Substring(atIndex + 1);
#endif

				string tld = GetTldFromEmail(emailAddress);
				string[] parts = tld.Split('.');
				int tldLabelCount = parts.Length;

				// Labels: ["mail", "example", "co", "jp"]
				// SLD is always second from the right,
				// regardless of subdomain count
				string[] labels = domain.Split('.');

				// Minimum 2 labels needed: domain.tld
				if (labels.Length > 1)
				{
					int sldIndex = labels.Length - tldLabelCount - 1;

					if (sldIndex >= 0)
					{
#if NET5_0_OR_GREATER
						domainBase = labels[sldIndex];
#else
						domainBase = labels[sldIndex];
#endif
					}
				}
			}
		}

		return domainBase;
	}

	/// <summary>
	/// Extracts the TLD from an email address.
	/// e.g. "user@example.co.jp"  → "co.jp"
	///      "user@example.com"    → "com".
	/// </summary>
	/// <param name="emailAddress">The email address to extract the domain base
	/// from.</param>
	/// <returns>The TLD if extraction is successful; otherwise, an empty
	/// string.</returns>
	public static string GetTldFromEmail(string emailAddress)
	{
		string tld = string.Empty;

		if (!string.IsNullOrWhiteSpace(emailAddress))
		{
			string normalized = Normalize(emailAddress);

			int atIndex = normalized.LastIndexOf('@');

			if (atIndex > 0 && atIndex < normalized.Length - 1)
			{
#if NET5_0_OR_GREATER
				string domain = normalized[(atIndex + 1)..];
#else
				string domain = normalized.Substring(atIndex + 1);
#endif
				string[] labels = domain.Split('.');

				if (labels.Length > 1)
				{
					// For known multi-part TLDs return the last two labels
					// For everything else return just the last label
#if NET5_0_OR_GREATER
					string[] lastTwoLabels = labels[^2..];
#else
					IEnumerable<string> items = labels.Skip(labels.Length - 2);
					string[] lastTwoLabels = items.ToArray();
#endif

					string lastTwo = string.Join(".", lastTwoLabels);

					bool multiPart = IsKnownMultiPartTld(lastTwo);

					if (multiPart == true)
					{
						// "co.jp", "com.au", "org.uk"
						tld = lastTwo;
					}
					else
					{
						// "com", "net", "org"
#if NET5_0_OR_GREATER
						tld = labels[^1];
#else
						tld = labels[labels.Length - 1];
#endif
					}
				}
			}
		}

		return tld;
	}

	/// <summary>
	/// Extracts all domain parts from an email as a structured record.
	/// </summary>
	/// <param name="emailAddress">The email address to extract the domain base
	/// from.</param>
	/// <returns>A tuple containing the local part, subdomain, domain base, and
	/// TLD.</returns>
	public static
		(string Local, string Subdomain, string DomainBase, string Tld)
		ParseEmailParts(string emailAddress)
	{
		string local = string.Empty;
		string subdomain = string.Empty;
		string domainBase = string.Empty;
		string tld = string.Empty;

		if (!string.IsNullOrWhiteSpace(emailAddress))
		{
			string normalized = Normalize(emailAddress);

			int atIndex = normalized.LastIndexOf('@');

			if (atIndex > -1)
			{
#if NET5_0_OR_GREATER
				local = normalized[..atIndex];
				string domain = normalized[(atIndex + 1)..];
#else
				local = normalized.Substring(0, atIndex);
				string domain = normalized.Substring(atIndex + 1);
#endif
				string[] labels = domain.Split('.');

				if (labels.Length > 1)
				{
					tld = GetTldFromEmail(emailAddress);

					string[] parts = tld.Split('.');
					int tldLabelCount = parts.Length;
					int sldIndex = labels.Length - tldLabelCount - 1;

					if (sldIndex >= 0)
					{
						domainBase = labels[sldIndex];

#if NET5_0_OR_GREATER
						string[] subdomainLabels = labels[..sldIndex];
#else
						string[] subdomainLabels = new string[sldIndex];
						Array.Copy(labels, 0, subdomainLabels, 0, sldIndex);
#endif
						subdomain = string.Join(".", subdomainLabels);
					}
				}
			}
		}

		return (local, subdomain, domainBase, tld);
	}

	private static bool IsKnownMultiPartTld(string candidate) =>
		KnownMultiPartTlds.Contains(candidate);
}
