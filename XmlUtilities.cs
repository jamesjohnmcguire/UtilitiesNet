/////////////////////////////////////////////////////////////////////////////
// <copyright file="XmlUtilities.cs" company="James John McGuire">
// Copyright © 2006 - 2021 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using Common.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DigitalZenWorks.Common.Utilities
{
	/// <summary>
	/// XML utilities class.
	/// </summary>
	public static class XmlUtilities
	{
		private static readonly ILog Log = LogManager.GetLogger(
			MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly ResourceManager StringTable = new
			ResourceManager(
			"DigitalZenWorks.Common.Utilities.Resources",
			Assembly.GetExecutingAssembly());

		/// <summary>
		/// Load with validation.
		/// </summary>
		/// <param name="schemaFile">The schema file.</param>
		/// <param name="xmlFile">The XML file.</param>
		/// <param name="type">The serializer type.</param>
		/// <returns>The XML object.</returns>
		public static object LoadWithValidation(
			string schemaFile, string xmlFile, Type type)
		{
			object obj = null;

			try
			{
				if (null != type)
				{
					XmlSerializer serializer = new XmlSerializer(type);

					if (File.Exists(schemaFile))
					{
						XmlReaderSettings settings = new XmlReaderSettings();
						settings.ValidationType = ValidationType.Schema;
						settings.Schemas.Add(null, schemaFile);

						if (File.Exists(xmlFile))
						{
							using (XmlReader validatingReader =
								XmlReader.Create(xmlFile, settings))
							{
								obj = (OrderedItem)serializer.Deserialize(
									validatingReader);
							}
						}
					}
				}
			}
			catch (Exception exception) when
				(exception is ArgumentNullException ||
				exception is FileNotFoundException ||
				exception is InvalidOperationException ||
				exception is UriFormatException ||
				exception is XmlSchemaException)
			{
				Log.Error(CultureInfo.InvariantCulture, m => m(
					StringTable.GetString(
						"EXCEPTION",
						CultureInfo.InvariantCulture) + exception));
			}
			catch
			{
				throw;
			}

			return obj;
		}
	}
}
