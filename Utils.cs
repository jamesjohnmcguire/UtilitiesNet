/////////////////////////////////////////////////////////////////////////////
// $Id:$
//
// Copyright (c) 2006-2015 by James John McGuire
// All rights reserved.
/////////////////////////////////////////////////////////////////////////////

using Common.Logging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Zenware.Common.UtilsNet.Extensions;

namespace Zenware.Common.UtilsNet
{
	public static class Utils
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
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
		/// Reads a text file contents into a string
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static string GetFileContents(string filePath)
		{
			string contents = null;

			if (File.Exists(filePath))
			{
				StreamReader StreamReaderObject = new StreamReader(filePath);

				if (null != StreamReaderObject)
				{
					contents = StreamReaderObject.ReadToEnd();
					StreamReaderObject.Close();
				}
			}

			return contents;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns a writable stream object.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static StreamWriter GetWriteStreamObject(
			string filePath)
		{
			StreamWriter streamWriterObject = null;
			if (File.Exists(filePath))
			{
				//set up a filestream
				FileStream fileStreamObject = new FileStream(filePath,
															FileMode.Open,
															FileAccess.ReadWrite);

				if (null != fileStreamObject)
				{
					//set up a streamwriter for adding text
					streamWriterObject = new StreamWriter(fileStreamObject);
				}
			}

			return streamWriterObject;
		}

		/////////////////////////////////////////////////////////////////////
		/// IsValidEmailAddress
		/////////////////////////////////////////////////////////////////////
		public static bool IsValidEmailAddress(string emailAddress)
		{
			bool ValidEmailAddress = false;

			//ValidEmailRegEx	= "/^([a-zA-Z0-9])+([a-zA-Z0-9\._-])*@([a-zA-Z0-9_-])+([a-zA-Z0-9\._-]+)+$/";
			//ValidEmailRegEx	= "/^([a-zA-Z0-9])+([a-zA-Z0-9\._-])*@([a-zA-Z0-9_-])+([a-zA-Z0-9\._-]+)+$/";
			//ValidEmailRegEx	= "/^[\w]+(\.[\w]+)*@([\w\-]+\.)+[a-zA-Z]{2,7}$/";
			//ValidEmailRegEx	= "/^([a-zA-Z0-9])+([a-zA-Z0-9\.\\+=_-])*@([a-zA-Z0-9_-])+([a-zA-Z0-9\._-]+)+$/";

			//string ValidEmailRegEx = @"/^([a-z0-9])(([-a-z0-9._])*([a-z0-9]))*\@([a-z0-9])(([a-z0-9-])*([a-z0-9]))+(\.([a-z0-9])([-a-z0-9_-])?([a-z0-9])+)+$/i";
			//string ValidEmailRegEx = @"/^([a-z0-9])(([-a-z0-9._])*([a-z0-9]))*\@([a-z0-9])(([a-z0-9-])*([a-z0-9]))+(\.([a-z0-9])([-a-z0-9_-])?([a-z0-9])+)+$/";
			//string ValidEmailRegEx = @"/^([a-z0-9])(([-a-z0-9._])*([a-z0-9]))*\@([a-z0-9])(([a-z0-9-])*([a-z0-9]))+(\.([a-z0-9])([-a-z0-9_-])?([a-z0-9])+)+$/";
			string ValidEmailRegEx =
			@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@" +
				@"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\." +
				@"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|" +
				@"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

			// checks proper syntax
			Match Match = Regex.Match(emailAddress, ValidEmailRegEx);

			if (true == Match.Success)
			{
				ValidEmailAddress = true;
			}

			return ValidEmailAddress;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Validates command line parameters
		/// </summary>
		/// <param name="FilePath"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static bool CheckCommandLineParameters(	string[] parameters)
		{
			bool IsValid = false;

			// Ensure we have a valid file name
			if (parameters.Length < 1)
			{
				//Console.WriteLine("usage: ");
			}
			else
			{
				IsValid = true;
			}

			return IsValid;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// </summary>
		/// <param name="obj"></param>
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
						DateTime.TryParse(strDate, out date);

						if (date != DateTime.MinValue && date != DateTime.MaxValue)
						{
							successCode = true;
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
		/// SaveFile
		/// <summary>
		/// Saves the file.
		/// </summary>
		/// <param name="FileContents"></param>
		/// <param name="FilePathName"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public static bool SaveFile(
			string fileContents,
			string filePathName)
		{
			FileStream fileStreamObject = null;
			try
			{
				fileStreamObject = new FileStream(filePathName,
														 FileMode.Create,
														 FileAccess.ReadWrite);

				StreamWriter streamWriterObject =
					new StreamWriter(fileStreamObject);
				streamWriterObject.Write(fileContents);
				streamWriterObject.Close();
			}
			catch (Exception ex)
			{
				log.Error(CultureInfo.InvariantCulture, m =>
					m("Error: {0}", ex.Message));
			}
			finally
			{
				fileStreamObject.Close();
			}

			return true;
		}
	} // End Class
} // End Namespace