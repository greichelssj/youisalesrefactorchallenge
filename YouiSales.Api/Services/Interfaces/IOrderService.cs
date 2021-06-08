using System.Threading.Tasks;
using YouiSales.Api.Models;

namespace YouiSales.Api.Services.Interfaces
{
    /// <summary>
    /// Interface for Order Service.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Add line
        /// </summary>
        /// <param name="line">Content on the line object</param>
        /// <returns>Return success true/false</returns>
        Task<OrderResponse> AddLine(dynamic line);

        /// <summary>
        /// Receipt for the order.
        /// </summary>
        /// <param name="company">Company provided</param>
        /// <returns>Return result with Receipt</returns>
        Task<OrderResponse> Receipt(string company);

        /// <summary>
        /// HTML Receipt for the order.
        /// </summary>
        /// <param name="company">Company provided</param>
        /// <returns>Return result with HTML Receipt</returns>
        Task<OrderResponse> HtmlReceipt(string company);
    }
}
