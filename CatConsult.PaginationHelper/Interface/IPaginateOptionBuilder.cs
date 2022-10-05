namespace CatConsult.PaginationHelper
{
    public interface IPaginateOptionsBuilder
    {
        /// <summary>
        /// Generate paginate options
        /// </summary>
        /// <returns></returns>
        public PaginateOptions Build();

        /// <summary>
        /// Add values 
        /// </summary>
        /// <param name="key">value key</param>
        /// <param name="values">values</param>
        /// <returns></returns>
        public IPaginateOptionsBuilder Add(string key, params string[] values);

        /// <summary>
        /// Including column keys
        /// </summary>
        /// <param name="columns">including keys</param>
        /// <returns></returns>
        public IPaginateOptionsBuilder IncludeColumns(params string[] columns);

        /// <summary>
        /// Excluding column keys.
        /// </summary>
        /// <param name="columns">excluding keys</param>
        /// <returns></returns>
        public IPaginateOptionsBuilder ExcludeColumns(params string[] columns);
    }
}
