namespace Constellation.Sitecore.Pipelines.HttpRequest
{
	using global::Sitecore;
	using global::Sitecore.Diagnostics;
	using global::Sitecore.IO;
	using global::Sitecore.Pipelines.HttpRequest;
	using System.Diagnostics.CodeAnalysis;
	using System.Web.Hosting;

	/// <summary>
	/// Resolves files. This processor replaces the default Sitecore File Resolver and offers
	/// the benefits of the Constellation HttpRequestProcessor class, which allow the defiinition of
	/// explicit execution conditions.
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
	public class FileResolver : Constellation.Sitecore.Pipelines.HttpRequest.HttpRequestProcessor
	{
		/// <summary>
		/// Method that is executed if the conditions defined in the properties
		/// determine that the Processor should run for this request.
		/// Implement the behavior of the processor in this method.
		/// </summary>
		/// <param name="args">The details of the current HttpRequest.</param>
		protected override void Execute(HttpRequestArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			if (Context.Page.FilePath.Length > 0)
			{
				return;
			}

			var path = args.Url.FilePath;
			if (string.IsNullOrEmpty(path))
			{
				path = "/";
			}

			if (HostingEnvironment.VirtualPathProvider.DirectoryExists(path))
			{
				path = FileUtil.MakePath(path, "default.aspx");
			}

			if (string.CompareOrdinal(path, "/default.aspx") == 0 || !HostingEnvironment.VirtualPathProvider.FileExists(path))
			{
				return;
			}

			Tracer.Info("Using virtual file \"" + path + "\" instead of Sitecore layout.");
			Context.Page.FilePath = path;
		}

		/// <summary>
		/// Method that is executed if the conditions defined in the properties
		/// determine that the Processor should NOT run fro this request.
		/// Use this method to allow stock Sitecore processes to run when the
		/// custom processor is not a good idea.
		/// </summary>
		/// <param name="args">The details of the current HttpRequest.</param>
		protected override void Defer(HttpRequestArgs args)
		{
			var resolver = new global::Sitecore.Pipelines.HttpRequest.FileResolver();
			resolver.Process(args);
		}
	}
}