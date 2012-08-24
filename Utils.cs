/////////////////////////////////////////////////////////////////////////////
// $Id:$
//
// Copyright (c) 2006-2012 by James John McGuire
// All rights reserved.
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Common.Logging;

namespace Zenware.Common.UtilsNet
{
	public static class Utils
	{
		private static readonly ILog log = LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static string CallingMethod()
		{
			StackFrame stackFrame = new StackFrame();
			MethodBase methodBase = stackFrame.GetMethod();

			return methodBase.Name;
		}

		public static DateTime DateFromString(string stringDate)
		{
			DateTime date = DateTime.MinValue;

			try
			{
				DateTime.TryParse(stringDate, out date);
			}
			catch
			{
			}

			return date;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Reads a text file contents into a string
		/// </summary>
		/// <param name="PathOfFileToRead"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static string GetFileContents(
			string PathOfFileToRead)
		{
			string FileContents = null;

			if (File.Exists(PathOfFileToRead))
			{
				StreamReader StreamReaderObject = new StreamReader(PathOfFileToRead);

				if (null != StreamReaderObject)
				{
					FileContents = StreamReaderObject.ReadToEnd();
					StreamReaderObject.Close();
				}
			}

			return FileContents;
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns a writable stream object.
		/// </summary>
		/// <param name="FilePath"></param>
		/// <returns></returns>
		/////////////////////////////////////////////////////////////////////
		public static StreamWriter GetWriteStreamObject(
			string FilePath)
		{
			StreamWriter StreamWriterObject = null;
			if (File.Exists(FilePath))
			{
				//set up a filestream
				FileStream FileStreamObject = new FileStream(FilePath,
															FileMode.Open,
															FileAccess.ReadWrite);

				if (null != FileStreamObject)
				{
					//set up a streamwriter for adding text
					StreamWriterObject = new StreamWriter(FileStreamObject);
				}
			}

			return StreamWriterObject;
		}

		/////////////////////////////////////////////////////////////////////
		/// IsValidEmailAddress
		/////////////////////////////////////////////////////////////////////
		public static bool IsValidEmailAddress(string EmailAddress)
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
			@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
	 + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
	 + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
	 + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

			// checks proper syntax
			Match Match = Regex.Match(EmailAddress, ValidEmailRegEx);

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
		private static bool CheckCommandLineParameters(
			string[] Parameters)
		{
			bool IsValid = false;

			// Ensure we have a valid file name
			if (Parameters.Length < 1)
			{
				Console.WriteLine("usage: ");
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
		public static bool IsDate(Object obj)
		{
			string strDate = obj.ToString();
			try
			{
				DateTime dt;
				DateTime.TryParse(strDate, out dt);
				if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
				{
					return true;
				}

				return false;
			}
			catch
			{
				return false;
			}
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
				log.Error(CultureInfo.InvariantCulture, m => m("Error: {0}", ex.Message));
			}
			finally
			{
				fileStreamObject.Close();
			}

			return true;
		}
	} // End Class
} // End Namespace