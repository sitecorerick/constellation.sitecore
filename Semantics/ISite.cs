namespace Spark.Sitecore.Semantics
{

	/// <summary>
	/// Designates an object (typically Item based) as representing a 
	/// site definition in Sitecore. This Item is usually the parent
	/// of the site's Home Page.
	/// </summary>
	public interface ISite
	{
		/// <summary>
		/// Gets the home page of the site.
		/// </summary>
		IPage HomePage { get; }
	}
}
