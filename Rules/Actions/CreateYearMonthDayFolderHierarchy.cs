namespace Spark.Sitecore.Rules.Actions
{
	using System.Globalization;

	using global::Sitecore;
	using global::Sitecore.Data;
	using global::Sitecore.Data.Fields;
	using global::Sitecore.Data.Items;

	/// <summary>
	/// Rules action to create Year and Month hierarchy.
	/// </summary>
	/// <typeparam name="T">Rules Context.</typeparam>
	[UsedImplicitly]
	public class CreateYearMonthDayFolderHierarchy<T> : RuleAction<T>
		where T : global::Sitecore.Rules.RuleContext
	{
		#region Properties
		/// <summary>
		/// Gets or sets the base template to determine the parent template type.
		/// </summary>
		public string AncestorTemplate
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the Date Folder template.
		/// </summary>
		public string DateFolderTemplate
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the name of the field to pull the date from.
		/// </summary>
		public string DateFieldName
		{
			get;
			set;
		}
		#endregion Properties

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateYearMonthDayFolderHierarchy{T}"/> class.
		/// </summary>
		public CreateYearMonthDayFolderHierarchy()
		{
			this.DatabasesToProcess = "master";
		}
		#endregion

		#region Methods
		/// <summary>
		/// Applies rule.
		/// </summary>
		/// <param name="ruleContext">The rule context.</param>
		protected override void Execute(T ruleContext)
		{
			var savedItem = ruleContext.Item;
			var landingItem = savedItem.FindAncestorByTemplateId(new ID(this.AncestorTemplate));
			if (landingItem == null)
			{
				return;
			}

			DateField postedDate = savedItem.Fields[this.DateFieldName];
			var year = postedDate.DateTime.Year;

			var yearFolder = landingItem.FindOrCreateChildItem(year.ToString(CultureInfo.InvariantCulture), new ID(this.DateFolderTemplate));
			var monthFolder = yearFolder.FindOrCreateChildItem(postedDate.DateTime.Month.ToString("00", CultureInfo.InvariantCulture), new ID(this.DateFolderTemplate));
			var dayFolder = monthFolder.FindOrCreateChildItem(postedDate.DateTime.Day.ToString("00", CultureInfo.InvariantCulture), new ID(this.DateFolderTemplate));

			if (savedItem.ParentID.Equals(dayFolder.ID))
			{
				return;
			}

			var oldDay = savedItem.Parent;
			savedItem.MoveTo(dayFolder);

			// Clean up the folder tree
			if (oldDay.TemplateID.ToString().Equals(this.DateFolderTemplate) && !oldDay.HasChildren)
			{
				Item oldMonth = oldDay.Parent;
				oldDay.Delete();
				if (oldMonth.TemplateID.ToString().Equals(this.DateFolderTemplate) && !oldMonth.HasChildren)
				{
					Item oldYear = oldMonth.Parent;
					oldMonth.Delete();
					if (oldYear.TemplateID.ToString().Equals(this.DateFolderTemplate) && !oldYear.HasChildren)
					{
						oldYear.Delete();
					}
				}
			}
		}
		#endregion
	}
}
