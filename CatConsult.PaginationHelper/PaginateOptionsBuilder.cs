namespace CatConsult.PaginationHelper
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
            var loweredKey = key.ToLower();
            if (!TryGetValue(loweredKey, out var list))
            {
                list = this[loweredKey] = new List<string>();
            }
            foreach (var val in values)
            {
                list.Add(val);
            }
            return this;
        }

        /// <summary>
        /// Remove keys if exist
        /// </summary>
        /// <param name="keys">keys</param>
        /// <returns></returns>
        public IPaginateOptionsBuilder Remove(params string[] keys)
        {
            foreach(var key in keys)
            {
                var loweredKey = key.ToLower();
                if (this.ContainsKey(loweredKey))
                {
                    base.Remove(loweredKey);
                }
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
                _includingSet.Add(column.ToLower());
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
                _excludingSet.Add(column.ToLower());
            }
            return this;
        }
    }
}
