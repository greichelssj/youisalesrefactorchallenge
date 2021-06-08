using Newtonsoft.Json;
using YouiSales.Api.Models.Base;

namespace YouiSales.Api.Models
{
    /// <summary>
    /// Order Response.
    /// </summary>
    public class OrderResponse : BaseResponse
    {
        /// <summary>
        /// Result.
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
