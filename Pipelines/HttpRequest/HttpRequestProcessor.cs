namespace Spark.Sitecore.Pipelines.HttpRequest
{
	using System.Diagnostics.CodeAnalysis;
	using System.Web;

	using global::Sitecore.Pipelines.HttpRequest;

	/// <summary>
	/// Basic implementation of IContextSensitive for use with Http Pipeline processors.
	/// </summary>
	public abstract class HttpRequestProcessor : global::Sitecore.Pipelines.HttpRequest.HttpRequestProcessor, IContextSensitive
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestProcessor"/> class.
		/// </summary>
		// ReSharper disable PublicConstructorInAbstractClass
		public HttpRequestProcessor()
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
		/// <para>
		/// Recommended Values:
		/// "core"
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
		/// <para>
		/// Recommended Values:
		/// "/~", "/maintenance", "/sitecore", "/sitecore modules", "/_DEV/TdsService.asmx"
		/// </para>
		/// </summary>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
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
		/// <para>
		/// Recommended Values:
		/// "admin, login, modules_shell, modules_website, publisher, scheduler, service, shell, system, testing"
		/// </para>
		/// </summary>
		public string SitesToIgnore { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Sitecore's contract for Pipeline Processors.
		/// </summary>
		/// <param name="args">The details of the current HttpRequest.</param>
		public override void Process(HttpRequestArgs args)
		{
			if (this.ContextValidator.ContextIsValidForExecution())
			{
				this.Execute(args);
			}
			else
			{
				this.Defer(args);
			}
		}

		/// <summary>
		/// Method that is executed if the conditions defined in the properties
		/// determine that the Processor should run for this request.
		/// Implement the behavior of the processor in this method.
		/// </summary>
		/// <param name="args">The details of the current HttpRequest.</param>
		protected abstract void Execute(HttpRequestArgs args);

		/// <summary>
		/// Method that is executed if the conditions defined in the properties
		/// determine that the Processor should NOT run fro this request.
		/// Use this method to allow stock Sitecore processes to run when the
		/// custom processor is not a good idea.
		/// </summary>
		/// <param name="args">The details of the current HttpRequest.</param>
		protected abstract void Defer(HttpRequestArgs args);
		#endregion
	}
}