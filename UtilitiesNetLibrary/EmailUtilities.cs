/////////////////////////////////////////////////////////////////////////////
// <copyright file="EmailUtilities.cs" company="Digital Zen Works">
// Copyright © 2006 - 2026 Digital Zen Works.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities;

using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

/// <summary>
/// Provides utility methods for validating email addresses according to common
/// formatting rules and RFC 5321 constraints.
/// </summary>
/// <remarks>This class is intended for use in scenarios where email address
/// validation is required before processing or storing user input. The
/// validation performed aims to catch common formatting errors and adheres to
/// standard email address specifications. All members are static and
/// thread-safe.</remarks>
public class EmailUtilities
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

		// local part, max 64 chars (RFC 5321)
		@"[a-zA-Z0-9._%+\-]{1,64}" +
		@"@" +

		// no consecutive dots in domain
		@"(?!.*\.\.)" +

		// domain label can't start with hyphen
		@"(?!-)" +

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
		RegexOptions.Compiled | RegexOptions.IgnoreCase
	);

	// Partial: has @ and at least a domain, but no/incomplete TLD
	// e.g. "user@domain" or "user@domain.".
	private static readonly Regex PartialEmailRegex = new Regex(
		@"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+$",
		RegexOptions.Compiled | RegexOptions.IgnoreCase
	);

	private static readonly Regex SimpleEmailRegex = new Regex(
		 @"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{1,63}$",
		 RegexOptions.Compiled | RegexOptions.IgnoreCase
	 );

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
}
