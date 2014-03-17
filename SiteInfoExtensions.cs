
namespace Spark.Sitecore
{
	using global::Sitecore.Web;

	/// <summary>
	/// The site info extensions.
	/// </summary>
	public static class SiteInfoExtensions
	{
		/// <summary>
		/// The link provider name for the provided site.
		/// </summary>
		/// <param name="info">
		/// The info.
		/// </param>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
		public static string LinkProvider(this SiteInfo info)
		{
			return info.Properties["linkProvider"];
		}
	}
}
