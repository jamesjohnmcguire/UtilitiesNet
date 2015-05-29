/////////////////////////////////////////////////////////////////////////////
// $Id$
//
// Copyright (c) 2006-2015 by James John McGuire
// All rights reserved.
/////////////////////////////////////////////////////////////////////////////

using Common.Logging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DigitalZenWorks.Common.Utils
{
	public static class General
	{
		private static readonly ILog log = LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static string CallingMethod()
		{
			StackFrame stackFrame = new StackFrame(1);
			MethodBase methodBase = stackFrame.GetMethod();
			string methodName = methodBase.Name.Substring(1);
			int index = methodName.IndexOf('>');
			methodName = methodName.Substring(0, index);

			return methodName;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Validates command line parameters
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static bool CheckCommandLineParameters(string[] parameters)
		{
			bool IsValid = false;

			// Ensure we have a valid file name
			if ((null != parameters) && (parameters.Length < 1))
			{
				//Console.WriteLine("usage: ");
			}
			else
			{
				IsValid = true;
			}

			return IsValid;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes")]
		public static DateTime DateFromString(string dateText)
		{
			DateTime date = DateTime.MinValue;
			DateTime testDate = DateTime.MinValue;

			if (DateTime.TryParse(dateText, out testDate))
			{
				date = testDate;
			}

			return date;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Is object a date
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
			"CA1031:DoNotCatchGeneralExceptionTypes")]
		public static bool IsDate(Object value)
		{
			bool successCode = false;

			try
			{
				if (value != null)
				{
					DateTime date;
					string strDate = value.ToString();

					try
					{
						if (DateTime.TryParse(strDate, out date))
						{
							if (date != DateTime.MinValue && date != DateTime.MaxValue)
							{
								successCode = true;
							}
						}
					}
					catch
					{
					}
					finally
					{
					}
				}
			}
			catch (Exception ex)
			{
				log.Error(CultureInfo.InvariantCulture, m =>
					m("Error: {0}", ex.Message));
			}

			return successCode;
		}

		/////////////////////////////////////////////////////////////////////
		/// IsValidEmailAddress
		/////////////////////////////////////////////////////////////////////
		public static bool IsValidEmailAddress(string emailAddress)
		{
			bool valid = false;

			//validEmailRegEx	= "/^([a-zA-Z0-9])+([a-zA-Z0-9\._-])" +
			// "*@([a-zA-Z0-9_-])+([a-zA-Z0-9\._-]+)+$/";

			//string validEmailRegEx = @"/^([a-z0-9])(([-a-z0-9._])" +
			// "*([a-z0-9]))*\@([a-z0-9])(([a-z0-9-])*([a-z0-9]))" +
			// "+(\.([a-z0-9])([-a-z0-9_-])?([a-z0-9])+)+$/i";
			string validEmailRegEx =
			@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" +
				@"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." +
				@"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" +
				@"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

			// checks proper syntax
			Match Match = Regex.Match(emailAddress, validEmailRegEx);

			if (true == Match.Success)
			{
				valid = true;
			}

			return valid;
		}
	} // End Class
} // End Namespace