namespace Spark.Sitecore.Rules.Actions
{
	using global::Sitecore.Data;
	using global::Sitecore.Data.Items;
	using global::Sitecore.Rules;
	using global::Sitecore.SecurityModel;
	using System.Globalization;

	/// <summary>
	/// Rules action to create Alphabetical hierarchy.
	/// </summary>
	/// <typeparam name="T">Rules Context.</typeparam>
	public class MoveToAlphabeticalFolder<T> : RuleAction<T>
		where T : RuleContext
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="MoveToAlphabeticalFolder{T}"/> class.
		/// </summary>
		public MoveToAlphabeticalFolder()
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
		/// Applies Rule.
		/// </summary>
		/// <param name="ruleContext">
		/// The rule Context.
		/// </param>
		protected override void Execute(T ruleContext)
		{
			if (ruleContext.Item.TemplateID == new ID(this.FolderTemplate))
			{
				return;
			}

			var item = ruleContext.Item;
			var name = item.Name.ToUpper(CultureInfo.InvariantCulture);
			var firstLetter = name[0].ToString(CultureInfo.InvariantCulture);

			using (new SecurityDisabler())
			{
				var rootFolder = this.GetRootItem(item);
				var alphaFolder = rootFolder.FindOrCreateChildItem(firstLetter, new ID(this.FolderTemplate));
				item.MoveTo(alphaFolder);
			}
		}

		/// <summary>
		/// Gets the root item for a given item.
		/// </summary>
		/// <param name="context">
		/// The context item.
		/// </param>
		/// <returns>
		/// The root item<see cref="Item"/>.
		/// </returns>
		private Item GetRootItem(Item context)
		{
			if (context.Parent.TemplateID != new ID(this.FolderTemplate))
			{
				return context.Parent;
			}

			return this.GetRootItem(context.Parent);
		}
		#endregion
	}
}
