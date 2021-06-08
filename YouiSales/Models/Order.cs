namespace YouiSales.Common.Models
{
    /// <summary>
    /// Order class.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Intance the class Order
        /// </summary>
        /// <param name="company">company provided</param>
        public Order(string company)
        {
            Company = company;
            TaxRate = .1d;
        }

        /// <summary>
        /// Get/Set company string
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Get/Set taxRate double
        /// </summary>
        public double TaxRate { get; set; }
    }
}