/////////////////////////////////////////////////////////////////////////////
// <copyright file="EmailUtilities.cs" company="Digital Zen Works">
// Copyright © 2006 - 2026 Digital Zen Works.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class EmailUtilities
{
	/// <summary>
	/// Checks to see if the given email address is valid.
	/// </summary>
	/// <param name="emailAddress">The email address to check.</param>
	/// <returns>Returns true if the email address is valid,
	/// otherwise false.</returns>
	public static bool IsValidEmailAddress(string emailAddress)
	{
		bool valid = false;

#if REGEX_SIMPLE
		string validEmailRegEx = "/^([a-zA-Z0-9])+([a-zA-Z0-9\._-])" +
			"*@([a-zA-Z0-9_-])+([a-zA-Z0-9\._-]+)+$/";
#elif REGEX_MODERATE
		string validEmailRegEx = @"/^([a-z0-9])(([-a-z0-9._])" +
			"*([a-z0-9]))*\@([a-z0-9])(([a-z0-9-])*([a-z0-9]))" +
			"+(\.([a-z0-9])([-a-z0-9_-])?([a-z0-9])+)+$/i";
#else
		string validEmailRegEx =
			@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" +
			@"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?" +
			@"[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." +
			@"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?" +
			@"[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" +
			@"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
#endif

		// checks proper syntax
		Match match = Regex.Match(emailAddress, validEmailRegEx);

		if (match.Success == true)
		{
			valid = true;
		}

		return valid;
	}
}
