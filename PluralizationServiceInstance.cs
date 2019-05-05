using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PluralizationService;
using PluralizationService.Core;
using PluralizationService.English;

namespace DigitalZenWorks.Common.Utilities
{
	public static class PluralizationServiceInstance
	{
		private static readonly IPluralizationApi Api = BuildApi();
		private static readonly CultureInfo CultureInfo =
			new CultureInfo("en-US");

		static IPluralizationApi BuildApi()
		{
			PluralizationApiBuilder builder = new PluralizationApiBuilder();
			builder.AddEnglishProvider();

			IPluralizationApi api = builder.Build();

			return api;
		}

		public static string Pluralize(string name)
		{
			return Api.Pluralize(name, CultureInfo) ?? name;
		}

		public static string Singularize(string name)
		{
			return Api.Singularize(name, CultureInfo) ?? name;
		}
	}
}
