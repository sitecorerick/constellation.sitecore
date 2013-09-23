
namespace Spark.Sitecore.Links
{
	using System.Configuration;

	/// <summary>
	/// A collection of rules to determine which LinkProvider to instantiate for a given context.
	/// </summary>
	[ConfigurationCollection(typeof(LinkProviderSwitchingRuleElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
	public class LinkProviderSwitchingRuleCollection : ConfigurationElementCollection
	{
		/// <summary>
		/// The <see cref="LinkProviderSwitchingRuleElement"/> at the specified index.
		/// </summary>
		/// <param name="index">
		/// The index.
		/// </param>
		/// <returns>
		/// The <see cref="LinkProviderSwitchingRuleElement"/>.
		/// </returns>
		public LinkProviderSwitchingRuleElement this[int index]
		{
			get
			{
				return (LinkProviderSwitchingRuleElement)BaseGet(index);
			}

			set
			{
				if (this.BaseGet(index) != null)
				{
					this.BaseRemoveAt(index);
				}

				this.BaseAdd(index, value);
			}
		}

		/// <summary>
		/// The <see cref="LinkProviderSwitchingRuleElement"/> at the specified Key.
		/// </summary>
		/// <param name="key">
		/// The name of the rule.
		/// </param>
		/// <returns>
		/// The <see cref="LinkProviderSwitchingRuleElement"/>.
		/// </returns>
		public new LinkProviderSwitchingRuleElement this[string key]
		{
			get
			{
				return (LinkProviderSwitchingRuleElement)BaseGet(key);
			}
		}

		#region Methods
		/// <summary>
		/// Creates a new <see cref="LinkProviderSwitchingRuleElement"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="ConfigurationElement"/>.
		/// </returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new LinkProviderSwitchingRuleElement();
		}

		/// <summary>
		/// The Name attribute value of the provided <see cref="LinkProviderSwitchingRuleElement"/>.
		/// </summary>
		/// <param name="element">
		/// The element.
		/// </param>
		/// <returns>
		/// The <see cref="object"/>.
		/// </returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((LinkProviderSwitchingRuleElement)element).Name;
		}
		#endregion
	}
}
