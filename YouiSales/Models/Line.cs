namespace YouiSales.Common.Models
{
    /// <summary>
    /// Line class.
    /// </summary>
    public class Line
    {
        /// <summary>
        /// Intance the class Policy
        /// </summary>
        /// <param name="policy">policy provided</param>
        /// <param name="quantity">quantity provided</param>
        public Line(Policy policy, int quantity)
        {
            Policy = policy;
            Quantity = quantity;
        }

        /// <summary>
        /// Get/Set Policy object
        /// </summary>
        public Policy Policy { get; set; }

        /// <summary>
        /// GET/Set Quantity int
        /// </summary>
        public int Quantity { get; set; }
    }
}