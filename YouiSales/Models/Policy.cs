namespace YouiSales.Common.Models
{
    /// <summary>
    /// Policy class.
    /// </summary>
    public class Policy
    {
        public const int Car = 105;
        public const int Motorcycle = 56;
        public const int Home = 235;

        /// <summary>
        /// Intance the class Policy
        /// </summary>
        /// <param name="policyHolderName">Policy Holder Name provided</param>
        /// <param name="description">Description provided</param>
        /// <param name="price">price provided</param>
        public Policy(string policyHolderName, string description, int price)
        {
            PolicyHolderName = policyHolderName;
            Description = description;
            Price = price;
        }

        /// <summary>
        /// Get/Set Policy Holder Name string
        /// </summary>
        public string PolicyHolderName { get; set; }

        /// <summary>
        /// Get/Set Description string
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get/Set Price int
        /// </summary>
        public int Price { get; set; }

    }
}
