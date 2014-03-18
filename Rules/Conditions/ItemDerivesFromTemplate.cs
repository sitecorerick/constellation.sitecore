namespace Constellation.Sitecore.Rules.Conditions
{
	using global::Sitecore.Data;
	using global::Sitecore.Diagnostics;
	using global::Sitecore.Rules;
	using global::Sitecore.Rules.Conditions;

	/// <summary>
	/// Rule that walks the supplied Item's template inheritance hierarchy looking
	/// for a match to the TemplateID provided by the rule context.
	/// </summary>
	/// <typeparam name="T">Instance of Sitecore.Rules.Conditions.RuleContext.</typeparam>
	public class ItemDerivesFromTemplate<T> : WhenCondition<T>
		where T : RuleContext
	{
		/// <summary>
		/// The template Id.
		/// </summary>
		private ID templateId;

		/// <summary>
		/// Gets or sets the template id.
		/// </summary>
		public ID TemplateId
		{
			get
			{
				return this.templateId;
			}

			set
			{
				Assert.ArgumentNotNull(value, "value");
				this.templateId = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the ItemDerivesFromTemplate class.
		/// </summary>
		public ItemDerivesFromTemplate()
		{
			this.templateId = ID.Null;
		}

		/// <summary>
		/// Initializes a new instance of the ItemDerivesFromTemplate class.
		/// </summary>
		/// <param name="templateId">The template id.</param>
		public ItemDerivesFromTemplate(ID templateId)
		{
			Assert.ArgumentNotNull(templateId, "templateId");
			this.templateId = templateId;
		}

		/// <summary>
		/// Executes the specified rule context.
		/// </summary>
		/// <param name="ruleContext">The rule context.</param>
		/// <returns>
		/// <c>True</c>, if the condition succeeds, otherwise <c>false</c>.
		/// </returns>
		protected override bool Execute(T ruleContext)
		{
			Assert.ArgumentNotNull(ruleContext, "ruleContext");
			var item = ruleContext.Item;
			return item != null && item.IsDerived(this.TemplateId);
		}
	}
}
