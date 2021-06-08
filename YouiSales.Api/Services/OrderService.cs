using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouiSales.Api.Models;
using YouiSales.Api.Services.Interfaces;
using YouiSales.Common.Models;

namespace YouiSales.Api.Services
{
    /// <summary>
    /// Order Service.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IList<Line> _lines = null;
        private readonly string lineCacheKey = "YouiSales_GetLineData";
        private IMemoryCache _cacheSystem;

        /// <summary>
        /// Order Service Constructor.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="memoryCache">Memory Cache</param>
        public OrderService(ILogger<OrderService> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _cacheSystem = memoryCache;
            _lines = CacheLine();
        }

        /// <summary>
        /// Test propose only
        /// </summary>
        /// <param name="lineModel">Content on line</param>
        /// <returns></returns>
        private IList<Line> CacheLine(IList<Line> lineModel = null)
        {
            if (lineModel == null)
            {
                // Look for cache key.
                if (!_cacheSystem.TryGetValue(lineCacheKey, out lineModel))
                {
                    // Key not in cache, so get data.
                    lineModel = new List<Line>();                    
                }
            }
            // Save data in cache and set the relative expiration time to one day
            _cacheSystem.Set(lineCacheKey, lineModel, TimeSpan.FromDays(1));

            return lineModel;
        }

        /// <summary>
        /// Add line
        /// </summary>
        /// <param name="line">Content on the line object</param>
        public async Task<OrderResponse> AddLine(dynamic line)
        {
            _logger?.LogInformation("Add Line - Start");

            try
            {
                var lineObject = JsonConvert.DeserializeObject<Line>(line.ToString());

                _lines.Add(lineObject);

                CacheLine(_lines);

                _logger?.LogInformation("Add Line - Finish");

                return await Task.Run(() => new OrderResponse()
                {
                    Result = null,
                    Success = true
                });

            }
            catch (Exception ex)
            {
                _logger?.LogCritical($"There was an error on 'AddLine' invocation: {ex.Message}");

                return await Task.Run(() => new OrderResponse()
                {
                    Result = null,
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        /// <summary>
        /// Receipt for the order.
        /// </summary>
        /// <returns>Return result with Receipt</returns>
        public async Task<OrderResponse> Receipt(string company)
        {
            _logger?.LogInformation("Printing receipt (text version) - Start");

            try
            {
                Order order = new Order(company);
                double totalAmount = 0d;
                StringBuilder result = new StringBuilder($"Order Receipt for {order.Company}{Environment.NewLine}");
                for (int index = 0; index < _lines.Count; index++)
                {
                    Line line = _lines[index];
                    double amount = 0d;
                    if (line.Policy.Price == Policy.Car)
                    {
                        if (line.Quantity >= 1)
                        {
                            amount += line.Quantity * line.Policy.Price * .9d;
                        }
                        else
                        {
                            amount += line.Quantity * line.Policy.Price;
                        }
                    }
                    else if (line.Policy.Price == Policy.Motorcycle)
                    {
                        if (line.Quantity >= 2)
                        {
                            amount += line.Quantity * line.Policy.Price * .8d;
                        }
                        else
                        {
                            amount += line.Quantity * line.Policy.Price;
                        }
                    }
                    else if (line.Policy.Price == Policy.Home)
                    {
                        if (line.Quantity >= 1)
                        {
                            amount += line.Quantity * line.Policy.Price * .8d;
                        }
                        else
                        {
                            amount += line.Quantity * line.Policy.Price;
                        }
                    }

                    result.AppendLine($"\t{line.Quantity} x {line.Policy.PolicyHolderName} {line.Policy.Description} = {amount.ToString("C")}");
                    totalAmount += amount;
                }

                result.AppendLine($"Sub-Total: {totalAmount.ToString("C")}");
                double tax = totalAmount * order.TaxRate;
                result.AppendLine($"Tax: {tax.ToString("C")}");
                result.AppendLine($"Total: {(totalAmount + tax).ToString("C")}");
                result.Append($"Date: {DateTime.Now.ToString("F")}");

                _logger?.LogInformation("Printing receipt (text version) - Finish");

                return await Task.Run(() => new OrderResponse()
                {
                    Result = result.ToString(),
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger?.LogCritical($"There was an error on 'Receipt' invocation: {ex.Message}");

                return await Task.Run(() => new OrderResponse()
                {
                    Result = null,
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }

        /// <summary>
        /// HTML Receipt for the order.
        /// </summary>
        /// <returns>Return result with HTML Receipt</returns>
        public async Task<OrderResponse> HtmlReceipt(string company)
        {
            _logger?.LogInformation("Printing receipt (HTML version) - Start");

            try
            {
                Order order = new Order(company);
                double totalAmount = 0d;
                StringBuilder result = new StringBuilder($"<html><body><h1>Order Receipt for {order.Company}</h1>");
                if (_lines.Any())
                {
                    result.Append("<ul>");
                    for (var index = 0; index < _lines.Count; index++)
                    {
                        Line line = _lines[index];
                        double thisAmount = 0d;
                        if (line.Policy.Price == Policy.Car)
                        {
                            if (line.Quantity >= 2)
                            {
                                thisAmount += line.Quantity * line.Policy.Price * .9d;
                            }
                            else
                            {
                                thisAmount += line.Quantity * line.Policy.Price;
                            }
                        }
                        else if (line.Policy.Price == Policy.Motorcycle)
                        {
                            if (line.Quantity >= 2)
                            {
                                thisAmount += line.Quantity * line.Policy.Price * .8d;
                            }
                            else
                            {
                                thisAmount += line.Quantity * line.Policy.Price;
                            }
                        }
                        else if (line.Policy.Price == Policy.Home)
                        {
                            if (line.Quantity >= 2)
                            {
                                thisAmount += line.Quantity * line.Policy.Price * .8d;
                            }
                            else
                            {
                                thisAmount += line.Quantity * line.Policy.Price;
                            }
                        }

                        result.Append($"<li>{line.Quantity} x {line.Policy.PolicyHolderName} {line.Policy.Description} = {thisAmount.ToString("C")}</li>");
                        totalAmount += thisAmount;
                    }

                    result.Append("</ul>");
                }
                result.Append($"<h3>Sub-Total: {totalAmount.ToString("C")}</h3>");
                double tax = totalAmount * order.TaxRate;
                result.Append($"<h3>Tax: {tax.ToString("C")}</h3>");
                result.Append($"<h2>Total: {(totalAmount + tax).ToString("C")}</h2>");
                result.Append($"<h3>Date: {DateTime.Now.ToString("F")}</h3>");
                result.Append("</body></html>");

                _logger?.LogInformation("Printing receipt (HTML version) - Finish");

                return await Task.Run(() => new OrderResponse()
                {
                    Result = result.ToString(),
                    Success = true
                });
            }
            catch (Exception ex)
            {
                _logger?.LogCritical($"There was an error on 'HtmlReceipt' invocation: {ex.Message}");

                return await Task.Run(() => new OrderResponse()
                {
                    Result = null,
                    Success = false,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
