namespace Spark.Sitecore.Rules.Actions
{
	using global::Sitecore.Data;
	using global::Sitecore.Data.Items;
	using global::Sitecore.Rules;
	using global::Sitecore.SecurityModel;
	using System.Diagnostics.CodeAnalysis;

	/// <summary>
	/// Ensures that a Page has a child named _subcontent based upon the Subcontent Folder
	/// template.
	/// </summary>
	/// <typeparam name="T">An instance of RuleContext</typeparam>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
	public class AddSubcontentFolder<T> : RuleAction<T>
		where T : RuleContext
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="AddSubcontentFolder{T}"/> class.
		/// </summary>
		public AddSubcontentFolder()
		{
			this.DatabasesToProcess = "master";
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the Folder template.
		/// </summary>
		public string FolderTemplate { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Called by Sitecore as part of the rule execution pipeline.
		/// </summary>
		/// <param name="rulecontext">The rulecontext to use.</param>
		protected override void Execute(T rulecontext)
		{
			var item = rulecontext.Item;

			var folder = item.Axes.SelectSingleItem("./_subcontent");
			if (folder != null)
			{
				return;
			}

			var template = item.Database.GetTemplate(new ID(this.FolderTemplate));
			folder = item.Add("_subcontent", template);
			using (new SecurityDisabler())
			{
				using (new EditContext(folder))
				{
					folder.Appearance.ReadOnly = true;
				}
			}
		}
		#endregion
	}
}