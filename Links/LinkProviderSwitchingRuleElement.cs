namespace Spark.Sitecore.Links
{
	using System.Configuration;
	using System.Text;

	/// <summary>
	/// The link provider rule element.
	/// </summary>
	public class LinkProviderSwitchingRuleElement : ConfigurationElement
	{
		/// <summary>
		/// Gets or sets the rule name.
		/// </summary>
		[ConfigurationProperty("name", DefaultValue = "", IsRequired = true)]
		public string Name
		{
			get
			{
				return (string)this["name"];
			}
			set
			{
				this["name"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the provider name.
		/// </summary>
		[ConfigurationProperty("providerName", DefaultValue = "", IsRequired = true)]
		public string ProviderName
		{
			get
			{
				return (string)this["providerName"];
			}
			set
			{
				this["providerName"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the site name.
		/// </summary>
		[ConfigurationProperty("siteName", DefaultValue = "", IsRequired = false)]
		public string SiteName
		{
			get
			{
				return (string)this["siteName"];
			}
			set
			{
				this["siteName"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the item path.
		/// </summary>
		[ConfigurationProperty("itemPath", DefaultValue = "", IsRequired = false)]
		public string ItemPath
		{
			get
			{
				return (string)this["itemPath"];
			}
			set
			{
				this["itemPath"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the item language.
		/// </summary>
		[ConfigurationProperty("language", DefaultValue = "", IsRequired = false)]
		public string Language
		{
			get
			{
				return (string)this["language"];
			}
			set
			{
				this["language"] = value;
			}
		}

		/// <summary>
		/// The get unique key.
		/// </summary>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
		public string GetUniqueKey()
		{
			var builder = new StringBuilder();

			if (!string.IsNullOrEmpty(this.SiteName))
			{
				builder.Append(this.SiteName);
			}

			if (!string.IsNullOrEmpty(this.ItemPath))
			{
				builder.Append(this.ItemPath);
			}

			if (!string.IsNullOrEmpty(this.Language))
			{
				builder.Append(this.Language);
			}

			if (builder.Length == 0)
			{
				builder.Append(this.Name); // So we have a non-null key
			}

			return builder.ToString();
		}
	}
}
