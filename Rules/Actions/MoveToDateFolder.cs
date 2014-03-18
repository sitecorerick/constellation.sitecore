namespace Constellation.Sitecore.Rules.Actions
{
	using global::Sitecore.Data;
	using global::Sitecore.Data.Fields;
	using global::Sitecore.Data.Items;
	using global::Sitecore.Rules;
	using global::Sitecore.SecurityModel;
	using System.Globalization;

	/// <summary>
	/// Rules action to create Alphabetical hierarchy.
	/// </summary>
	/// <typeparam name="T">Rules Context.</typeparam>
	public class MoveToDateFolder<T> : RuleAction<T>
		where T : RuleContext
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="MoveToDateFolder{T}"/> class.
		/// </summary>
		public MoveToDateFolder()
		{
			this.DatabasesToProcess = "master";
		}
		#endregion

		/// <summary>
		/// The date sort options.
		/// </summary>
		public enum DateSortOptions
		{
			/// <summary>
			/// The only year.
			/// </summary>
			OnlyYear,

			/// <summary>
			/// The year and month.
			/// </summary>
			YearAndMonth,

			/// <summary>
			/// The year and month and day.
			/// </summary>
			YearAndMonthAndDay
		}

		#region Properties
		/// <summary>
		/// Gets or sets the Folder template.
		/// </summary>
		public string FolderTemplate { get; set; }

		/// <summary>
		/// Gets or sets the folder depth.
		/// </summary>
		public DateSortOptions FolderDepth { get; set; }

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
			var item = ruleContext.Item;
			if (item.TemplateID == new ID(this.FolderTemplate))
			{
				return;
			}

			DateField field = item.Fields["Release Date"] ?? item.Fields["__Created"];
			if (field.DateTime == System.DateTime.MinValue)
			{
				return;
			}

			var theYear = field.DateTime.ToString("yyyy", CultureInfo.InvariantCulture);
			var theMonth = field.DateTime.ToString("MM", CultureInfo.InvariantCulture);
			var theDay = field.DateTime.ToString("dd", CultureInfo.InvariantCulture);

			var folderLevel = this.GetSiteFolder(item);
			var oldFilePath = item.Paths.FullPath;
			var datePath = "/" + theYear;
			if (!oldFilePath.Contains(datePath))
			{
				this.MoveItem(theYear, item, folderLevel);
				oldFilePath = item.Paths.FullPath;
			}

			if (this.FolderDepth != DateSortOptions.OnlyYear)
			{
				datePath = datePath + "/" + theMonth;
				if (!oldFilePath.Contains(datePath))
				{
					folderLevel = this.AdvanceFolderLevel(folderLevel, item);
					this.MoveItem(theMonth, item, folderLevel);
					oldFilePath = item.Paths.FullPath;
				}

				if (this.FolderDepth == DateSortOptions.YearAndMonthAndDay)
				{
					datePath = datePath + "/" + theDay;
					if (!oldFilePath.Contains(datePath))
					{
						folderLevel = this.AdvanceFolderLevel(folderLevel, item);
						this.MoveItem(theDay, item, folderLevel);
					}
				}
			}
		}

		/// <summary>
		/// Finds the child of a folder that is an ancestor of an item.
		/// </summary>
		/// <param name="folder">
		/// The folder.
		/// </param>
		/// <param name="item">
		/// The context item.
		/// </param>
		/// <returns>
		/// The correct folder<see cref="Item"/>.
		/// </returns>
		protected Item AdvanceFolderLevel(Item folder, Item item)
		{
			foreach (Item child in folder.GetChildren())
			{
				if (child.Axes.IsAncestorOf(item))
				{
					return child;
				}
			}

			return folder;
		}

		/// <summary>
		/// Gets the site folder for the item.
		/// </summary>
		/// <param name="item">
		/// The context item.
		/// </param>
		/// <returns>
		/// The site folder<see cref="Item"/> for the item.
		/// </returns>
		protected Item GetSiteFolder(Item item)
		{
			item = item.Parent;
			while (item.TemplateID.ToString() == this.FolderTemplate)
			{
				item = item.Parent;
			}

			return item;
		}

		/// <summary>
		/// Moves the item to a new folder.
		/// </summary>
		/// <param name="name">
		/// The name of the folder.
		/// </param>
		/// <param name="currentItem">
		/// The current item.
		/// </param>
		/// <param name="folderLevel">
		/// The folder Level.
		/// </param>
		protected void MoveItem(string name, Item currentItem, Item folderLevel)
		{
			using (new SecurityDisabler())
			{
				var newFolder = folderLevel.FindOrCreateChildItem(name, new ID(this.FolderTemplate));
				currentItem.MoveTo(newFolder);
			}
		}
		#endregion
	}
}
