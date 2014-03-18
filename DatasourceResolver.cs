namespace Constellation.Sitecore
{
	using System;
	using System.Text;

	using global::Sitecore.Data;

	using global::Sitecore.Data.Items;

	/// <summary>
	/// Encompasses most of the common developer tasks associated with resolving a datasource string to an Item.
	/// </summary>
	public static class DatasourceResolver
	{
		#region Resolution
		/// <summary>
		/// Resolves a specific Item from whatever string-based Item reference one can provide, typically the Datasource 
		/// property of a Rendering. If datasource is empty, the Context Item is assumed to be the correct resolution.
		/// </summary>
		/// <param name="datasource">An absolute XPath, relative XPath, query statement, fast statement, or Guid string that resolves to a single Item.</param>
		/// <param name="contextItem">Item providing the database and language to use in resolving the datasource.</param>
		/// <returns>The datasource Item, the Context Item (if datasource is empty) or null.</returns>
		public static Item Resolve(string datasource, Item contextItem)
		{
			if (contextItem == null)
			{
				return null;
			}

			if (string.IsNullOrEmpty(datasource))
			{
				return contextItem;
			}

			return Resolve(datasource, contextItem.Database);
		}

		/// <summary>
		/// Resolves a specific Item from whatever string-based Item reference one can provide, typically the Datasource
		/// property of a Rendering. If datasource is empty, the Context Item is assumed to be the correct resolution.
		/// </summary>
		/// <param name="datasource">An absolute XPath, relative XPath, query statement, fast statement, or Guid string that resolves to a single Item.</param>
		/// <param name="database">Database to use when resolving the datasource.</param>
		/// <returns>The datasource Item, the Context Item (if datasource is empty) or null.</returns>
		public static Item Resolve(string datasource, Database database)
		{
			// check for query/fast syntax
			if (datasource.StartsWith("query:", StringComparison.OrdinalIgnoreCase) || datasource.StartsWith("fast:", StringComparison.OrdinalIgnoreCase))
			{
				return database.SelectSingleItem(EncodeQuery(datasource));
			}

			return database.GetItem(datasource);
		}
		#endregion

		#region Utility
		/// <summary>
		/// Inspects a given datasource to determine if it contains Sitecore query syntax.
		/// </summary>
		/// <param name="datasource">The string to review.</param>
		/// <returns>True if the string starts with "query" or "fast" or contains "*".</returns>
		public static bool IsQuery(string datasource)
		{
			if (!datasource.Contains("/"))
			{
				return false; // it's a Guid
			}

			if (datasource.StartsWith("query:", StringComparison.OrdinalIgnoreCase) || datasource.StartsWith("fast:", StringComparison.OrdinalIgnoreCase) || datasource.Contains("*"))
			{
				return true; // it's a query and represents multiple items.
			}

			return false;
		}

		/// <summary>
		/// Given an absolute XPath, relative XPath, query statement, fast statement, or Guid string, provides appropriate escaping for the target Sitecore API.
		/// </summary>
		/// <param name="query">The query to encode.</param>
		/// <returns>A query safe for using in a Database.Select*** or Database.GetItem*** call - Note that the implementer must know which one is the right call.</returns>
		public static string EncodeQuery(string query)
		{
			if (!IsQuery(query))
			{
				return query; // it's a guid, no processing required
			}

			// We're dealing with a query or fast statement, and we need to add hashes
			var builder = new StringBuilder();
			var parts = query.Split(new[] { '/' });

			foreach (var part in parts)
			{
				if (string.IsNullOrEmpty(part))
				{
					continue;
				}

				if (!part.StartsWith("query:", StringComparison.OrdinalIgnoreCase) && !part.StartsWith("fast:", StringComparison.OrdinalIgnoreCase))
				{
					builder.Append("/");
				}

				if (!part.StartsWith("#", StringComparison.OrdinalIgnoreCase) && (part.Contains(" ") || part.Contains("-")))
				{
					builder.Append("#" + part + "#");
				}
				else
				{
					builder.Append(part);
				}
			}

			return builder.ToString();
		}
		#endregion
	}
}
