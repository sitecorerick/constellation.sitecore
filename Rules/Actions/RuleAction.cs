namespace Constellation.Sitecore.Rules.Actions
{
	using global::Sitecore.Diagnostics;
	using System.Collections.Generic;
	using System.Web;

	/// <summary>
	/// A basic RuleAction framework that prevents rule recursion.
	/// </summary>
	/// <typeparam name="T">The ruleContext.</typeparam>
	public abstract class RuleAction<T> : global::Sitecore.Rules.Actions.RuleAction<T>, IContextSensitive
		where T : global::Sitecore.Rules.RuleContext
	{
		#region Locals
		/// <summary>
		/// Lists IDs that are still being acted upon. Prevents recursion.
		/// </summary>
		// ReSharper disable StaticFieldInGenericType
		private static readonly List<string> InProgress = new List<string>();
		// ReSharper restore StaticFieldInGenericType
		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RuleAction{T}"/> class.
		/// </summary>
		// ReSharper disable PublicConstructorInAbstractClass
		public RuleAction()
		// ReSharper restore PublicConstructorInAbstractClass
		{
			this.ContextValidator = new ContextValidator(this);
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the Context Validator associated with the instance.
		/// </summary>
		public ContextValidator ContextValidator { get; private set; }

		/// <summary>
		/// Gets the Sitecore database name for the current request.
		/// </summary>
		public string ContextDatabaseName
		{
			get { return global::Sitecore.Context.Database != null ? global::Sitecore.Context.Database.Name : string.Empty; }
		}

		/// <summary>
		/// Gets the Host name of the current request.
		/// </summary>
		public string ContextHostName
		{
			get { return HttpContext.Current != null ? HttpContext.Current.Request.Url.Host : string.Empty; }
		}

		/// <summary>
		/// Gets the file path for the current request.
		/// </summary>
		public string ContextLocalPath
		{
			get { return HttpContext.Current != null ? HttpContext.Current.Request.Url.LocalPath : string.Empty; }
		}

		/// <summary>
		/// Gets the Sitecore site name for the current request.
		/// </summary>
		public string ContextSiteName
		{
			get { return global::Sitecore.Context.Site != null ? global::Sitecore.Context.Site.Name : string.Empty; }
		}

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of Database names that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string DatabasesToProcess { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of Database names that should cause the Processor to ignore
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string DatabasesToIgnore { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of host names that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string HostnamesToProcess { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of host names that should cause the Processor to ignore
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string HostnamesToIgnore { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of file paths that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string PathsToProcess { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of file paths that should cause the Processor to ignore 
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string PathsToIgnore { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of Site names that should match the current Request before
		/// the Processor executes.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string SitesToProcess { get; set; }

		/// <summary>
		/// <para>
		/// Gets or sets a comma-delimited list of Site names that should cause the Processor to ignore
		/// the request.
		/// </para>
		/// <para>
		/// When implementing, list the members in alphabetical order to optimize search routines.
		/// </para>
		/// </summary>
		public string SitesToIgnore { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// The method called by the Rule Engine.
		/// </summary>
		/// <param name="ruleContext">The context.</param>
		public override void Apply(T ruleContext)
		{
			if (!this.ContextValidator.ContextIsValidForExecution())
			{
				return;
			}

			if (InProgress.Contains(ruleContext.Item.ID.ToString()))
			{
				return;
			}

			Log.Debug("RuleAction " + this.GetType().Name + " started for " + ruleContext.Item.Name, this);
			InProgress.Add(ruleContext.Item.ID.ToString());

			try
			{
				this.Execute(ruleContext);
			}
			finally
			{
				InProgress.Remove(ruleContext.Item.ID.ToString());
			}

			Log.Debug("RuleAction " + this.GetType().Name + " ended for " + ruleContext.Item.Name, this);
		}

		/// <summary>
		/// Implement the Action details.
		/// </summary>
		/// <param name="ruleContext">The context.</param>
		protected abstract void Execute(T ruleContext);
		#endregion
	}
}