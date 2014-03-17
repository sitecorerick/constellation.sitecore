namespace Spark.Sitecore.Links
{
	using System.Configuration;
	using System.Diagnostics.CodeAnalysis;


	/// <summary>
	/// The configuration settings for the LinkProviderSwitcher.
	/// </summary>
	public class LinkProviderSwitchingConfiguration : ConfigurationSection
	{
		#region Fields
		/// <summary>
		/// The configuration settings for the LinkProviderSwitcher.
		/// </summary>
		private static LinkProviderSwitchingConfiguration config = null;

		/// <summary>
		/// If true, at least one rule has a site name defined.
		/// </summary>
		private static bool siteNameIsRelevant;

		/// <summary>
		/// If true, at least one rule has an item path defined.
		/// </summary>
		private static bool itemPathIsRelevant;

		/// <summary>
		/// If true, at least one rule has a language defined.
		/// </summary>
		private static bool languageIsRelevant;

		/// <summary>
		/// Flag to determine if the rules have been parsed to populate the
		/// relevance properties.
		/// </summary>
		private static bool rulesParsed;
		#endregion

		#region Properties

		/// <summary>
		/// Gets the configuration settings for the LinkProviderSwitcher.
		/// </summary>
		public static LinkProviderSwitchingConfiguration Settings
		{
			get
			{
				if (config == null)
				{
					config = ConfigurationManager.GetSection("spark/linkProviderSwitching") as LinkProviderSwitchingConfiguration;
				}
				return config;
			}
		}

		/// <summary>
		/// Gets the rules for selecting a Link Provider.
		/// </summary>
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public LinkProviderSwitchingRuleCollection Rules
		{
			[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1122:UseStringEmptyForEmptyStrings", Justification = "Reviewed. Suppression is OK here.")]
			get
			{
				return (LinkProviderSwitchingRuleCollection)base[""];
			}
		}

		/// <summary>
		/// Gets a value indicating whether site name is relevant when
		/// selecting a rule.
		/// </summary>
		[SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Reviewed. Suppression is OK here.")]
		public static bool SiteNameIsRelevant
		{
			get
			{
				if (!rulesParsed)
				{
					ParseRulesForRelevance();
				}

				return siteNameIsRelevant;
			}
		}

		/// <summary>
		/// Gets a value indicating whether item path is relevant when
		/// selecting a rule.
		/// </summary>
		[SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Reviewed. Suppression is OK here.")]
		public static bool ItemPathIsRelevant
		{
			get
			{
				if (!rulesParsed)
				{
					ParseRulesForRelevance();
				}

				return itemPathIsRelevant;
			}
		}

		/// <summary>
		/// Gets a value indicating whether language is relevant
		/// when selecting a rule.
		/// </summary>
		[SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Reviewed. Suppression is OK here.")]
		public static bool LanguageIsRelevant
		{
			get
			{
				if (!rulesParsed)
				{
					ParseRulesForRelevance();
				}

				return languageIsRelevant;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Reviews all rules to determine the most efficient way to find the
		/// applicable rule.
		/// </summary>
		private static void ParseRulesForRelevance()
		{
			var rules = Settings.Rules;

			foreach (LinkProviderSwitchingRuleElement rule in rules)
			{
				if (!string.IsNullOrEmpty(rule.SiteName))
				{
					siteNameIsRelevant = true;
				}

				if (!string.IsNullOrEmpty(rule.ItemPath))
				{
					itemPathIsRelevant = true;
				}

				if (!string.IsNullOrEmpty(rule.Language))
				{
					languageIsRelevant = true;
				}

				if (siteNameIsRelevant && itemPathIsRelevant && languageIsRelevant)
				{
					break;
				}
			}

			rulesParsed = true;
		}
		#endregion
	}
}
