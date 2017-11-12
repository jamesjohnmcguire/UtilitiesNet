using Common.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DigitalZenWorks.Common.Utils
{
	public static class XmlUtilities
	{
		private static readonly ILog log = LogManager.GetLogger
			(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly ResourceManager stringTable = new
			ResourceManager("DigitalZenWorks.Common.Utils.Resources",
			Assembly.GetExecutingAssembly());

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public static object LoadWithValidation(string schemaFile,
			string xmlFile, Type type)
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
				log.Error(CultureInfo.InvariantCulture, m => m(
					stringTable.GetString("EXCEPTION") + exception));
			}
			catch
			{
				throw;
			}

			return obj;
		}
	}
}
