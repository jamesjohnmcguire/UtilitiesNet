/////////////////////////////////////////////////////////////////////////////
// <copyright file="XmlUtilities.cs" company="James John McGuire">
// Copyright © 2006 - 2026 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.Common.Utilities
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Xml;
	using System.Xml.Schema;
	using System.Xml.Serialization;

	using global::Common.Logging;

	/// <summary>
	/// XML utilities class.
	/// </summary>
	public static class XmlUtilities
	{
		private static readonly Type ClassType = typeof(XmlUtilities);
		private static readonly ILog Log =
			LogManager.GetLogger(ClassType);

		/// <summary>
		/// Converts the XML node to string.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns>The string represeentation of the node.</returns>
		public static string? ConvertXmlNodeToString(XmlNode node)
		{
			string? nodeString = null;

			if (node != null)
			{
				StringWriter stringWriter = new ();
				using XmlTextWriter xmlTextWriter = new (stringWriter);

				node.WriteTo(xmlTextWriter);
				nodeString = stringWriter.ToString();
			}

			return nodeString;
		}

		/// <summary>
		/// Load with validation.
		/// </summary>
		/// <param name="schemaFile">The schema file.</param>
		/// <param name="xmlFile">The XML file.</param>
		/// <param name="type">The serializer type.</param>
		/// <returns>The XML object.</returns>
		public static object? LoadWithValidation(
			string schemaFile, string xmlFile, Type? type)
		{
			object? deserializedObject = null;

			try
			{
				if (type != null)
				{
					XmlSerializer serializer = new(type);

					if (File.Exists(schemaFile))
					{
						XmlReaderSettings settings = new();
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

