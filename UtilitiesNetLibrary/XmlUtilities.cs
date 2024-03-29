﻿/////////////////////////////////////////////////////////////////////////////
// <copyright file="XmlUtilities.cs" company="James John McGuire">
// Copyright © 2006 - 2024 James John McGuire. All Rights Reserved.
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

#pragma warning disable CA1823
		private static readonly ResourceManager StringTable = new (
			"DigitalZenWorks.Common.Utilities.Resources",
			Assembly.GetExecutingAssembly());
#pragma warning restore CA1823

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
			object deserializedObject = null;

			try
			{
				if (null != type)
				{
					XmlSerializer serializer = new (type);

					if (File.Exists(schemaFile))
					{
						XmlReaderSettings settings = new ();
						settings.ValidationType = ValidationType.Schema;
						settings.Schemas.Add(null, schemaFile);

						if (File.Exists(xmlFile))
						{
							using XmlReader validatingReader =
								XmlReader.Create(xmlFile, settings);
							deserializedObject =
								serializer.Deserialize(validatingReader);
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
				Log.Error(exception.ToString());
			}
			catch
			{
				throw;
			}

			return deserializedObject;
		}
	}
}
