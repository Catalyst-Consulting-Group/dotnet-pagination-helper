namespace PaginationHelper
{
    /// <summary>
    /// This class is use to bind [FromQuery] QueryParameters and construct paginate options
    /// </summary>
    /// <example>
    /// public IActionResult GetPaginated([FromQuery] PaginateOptionsBuilder paginateOptionsBuilder)
    /// {
    ///     var options = paginateOptionsBuilder.ExcludeColumns("foo", "foo2").Build()
    /// }
    /// </example>
    public class PaginateOptionsBuilder : Dictionary<string, ICollection<string>>, IPaginateOptionsBuilder
    {

        private readonly ISet<string> _excludingSet = new HashSet<string>();
        private readonly ISet<string> _includingSet = new HashSet<string>();

        /// <summary>
        /// Generate paginate options
        /// </summary>
        /// <returns></returns>
        public PaginateOptions Build()
        {
            return new PaginateOptions(this, _excludingSet, _includingSet);
        }

        /// <summary>
        /// Add values 
        /// </summary>
        /// <param name="key">value key</param>
        /// <param name="values">values</param>
        /// <returns></returns>
        public IPaginateOptionsBuilder Add(string key, params string[] values)
        {
            if (!TryGetValue(key, out var list))
            {
                list = this[key] = new List<string>();
            }
            foreach (var val in values)
            {
                list.Add(val);
            }
            return this;
        }

        /// <summary>
        /// Including column keys
        /// </summary>
        /// <param name="columns">including keys</param>
        /// <returns></returns>
        public IPaginateOptionsBuilder IncludeColumns(params string[] columns)
        {
            foreach (var column in columns)
            {
                _includingSet.Add(column);
            }
            return this;
        }

        /// <summary>
        /// Excluding column keys.
        /// </summary>
        /// <param name="columns">excluding keys</param>
        /// <returns></returns>
        public IPaginateOptionsBuilder ExcludeColumns(params string[] columns)
        {
            foreach (var column in columns)
            {
                _excludingSet.Add(column);
            }
            return this;
        }
    }
}
