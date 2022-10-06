namespace CatConsult.PaginationHelper
{
    public interface IPaginateOptionsBuilder
    {
        /// <summary>
        /// Generate paginate options
        /// </summary>
        /// <returns></returns>
        PaginateOptions Build();

        /// <summary>
        /// Add values 
        /// </summary>
        /// <param name="key">value key</param>
        /// <param name="values">values</param>
        /// <returns></returns>
        IPaginateOptionsBuilder Add(string key, params string[] values);

        /// <summary>
        /// Remove keys if exist
        /// </summary>
        /// <param name="keys">keys</param>
        /// <returns></returns>
        IPaginateOptionsBuilder Remove(params string[] keys);

        /// <summary>
        /// Including column keys
        /// </summary>
        /// <param name="columns">including keys</param>
        /// <returns></returns>
        IPaginateOptionsBuilder IncludeColumns(params string[] columns);

        /// <summary>
        /// Excluding column keys.
        /// </summary>
        /// <param name="columns">excluding keys</param>
        /// <returns></returns>
        IPaginateOptionsBuilder ExcludeColumns(params string[] columns);
    }
}
