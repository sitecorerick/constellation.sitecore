namespace Spark.Sitecore.Links
{
	using global::Sitecore.Links;
	using System;

	/// <summary>
	/// Reads the configuration file and determines the best-match LinkProvider.
	/// </summary>
	public static class LinkProviderResolver
	{
		/// <summary>
		/// Resolves the correct link provider based on the system configuration.
		/// </summary>
		/// <param name="site">
		/// The site.
		/// </param>
		/// <param name="itemPath">
		/// The item path.
		/// </param>
		/// <param name="itemLanguage">
		/// The item language.
		/// </param>
		/// <returns>
		/// The <see cref="LinkProvider"/>.
		/// </returns>
		public static LinkProvider Resolve(string site, string itemPath, string itemLanguage)
		{
			LinkProvider provider = null;

			/*
			 * Search possibility matrix
			 * s p l
			 * s p x
			 * s x l
			 * s x x
			 * x p l
			 * x p x
			 * x x l
			 */

			var pathIsRelevant = LinkProviderSwitchingConfiguration.ItemPathIsRelevant;
			var langIsRelevant = LinkProviderSwitchingConfiguration.LanguageIsRelevant;
			var siteIsRelevant = LinkProviderSwitchingConfiguration.SiteNameIsRelevant;

			if (!pathIsRelevant && !langIsRelevant && !siteIsRelevant)
			{
				return new LinkProvider();
			}

			var rules = LinkProviderSwitchingConfiguration.Settings.Rules;

			foreach (LinkProviderSwitchingRuleElement rule in rules)
			{
				var siteMatches = rule.SiteName.Equals(site, StringComparison.InvariantCultureIgnoreCase);
				var pathMatches = rule.ItemPath.Equals(itemPath, StringComparison.InvariantCultureIgnoreCase);
				var langMatches = rule.Language.Equals(itemLanguage, StringComparison.InvariantCultureIgnoreCase);

				if (siteMatches && pathMatches && langMatches)
				{
					provider = LinkManager.Providers[rule.ProviderName];
					break;
				}

				if (siteMatches && pathMatches && !langIsRelevant)
				{
					provider = LinkManager.Providers[rule.ProviderName];
					break;
				}

				if (siteMatches && !pathIsRelevant && langMatches)
				{
					provider = LinkManager.Providers[rule.ProviderName];
					break;
				}

				if (siteMatches && !pathIsRelevant && !langIsRelevant)
				{
					provider = LinkManager.Providers[rule.ProviderName];
					break;
				}

				if (!siteIsRelevant && pathMatches && langMatches)
				{
					provider = LinkManager.Providers[rule.ProviderName];
					break;
				}

				if (!siteIsRelevant && pathMatches && !langIsRelevant)
				{
					provider = LinkManager.Providers[rule.ProviderName];
					break;
				}

				if (!siteIsRelevant && !pathIsRelevant && langMatches)
				{
					provider = LinkManager.Providers[rule.ProviderName];
					break;
				}
			}

			return provider;
		}
	}
}
