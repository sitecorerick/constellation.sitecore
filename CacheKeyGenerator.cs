namespace Spark.Sitecore
{
	using System;
	using System.Globalization;

	using global::Sitecore.Data;

	using global::Sitecore.Globalization;

	/// <summary>
	/// Utility to provide a very unique key for caching any object based on a Sitecore Item.
	/// </summary>
	public static class CacheKeyGenerator
	{
		/// <summary>
		/// Provides a string value to use as a cache key.
		/// </summary>
		/// <param name="repositoryType">The System.Type of repository that is requesting the cache key.
		/// - Ensures that the key is unique to that repository.
		/// </param>
		/// <param name="db">The name of the Sitecore Database related to the cacheable object.</param>
		/// <param name="lang">The Sitecore Language related to the cacheable object.</param>
		/// <returns>A string value to use as a cache key.</returns>
		public static string GetCacheKey(Type repositoryType, Database db, Language lang)
		{
			return GetCacheKey(string.Empty, repositoryType, db, lang);
		}

		/// <summary>
		/// Provides a string value to use as a cache key.
		/// </summary>
		/// <param name="identifier">A string-based unique identifier for the cacheable object.
		/// - for controls, this might be the control's UniqueID.
		/// - for Items, this might be the Guid or Path of the Item.
		/// - for collections, this might be the Datasource or Query string used to retrieve results.
		/// </param>
		/// <param name="repositoryType">The System.Type of repository that is requesting the cache key.
		/// - Ensures that the key is unique to that repository.
		/// </param>
		/// <param name="db">The name of the Sitecore Database related to the cacheable object.</param>
		/// <param name="lang">The Sitecore Language related to the cacheable object.</param>
		/// <returns>A string value to use as a cache key.</returns>
		public static string GetCacheKey(string identifier, Type repositoryType, Database db, Language lang)
		{
			string databaseName = string.Empty;
			if (db != null)
			{
				databaseName = db.Name;
			}

			return string.Format(CultureInfo.InvariantCulture, "{0}-{1}-{2}-{3}", identifier, repositoryType.ToString(), databaseName, Language.GetDisplayName(lang.CultureInfo));
		}
	}
}
