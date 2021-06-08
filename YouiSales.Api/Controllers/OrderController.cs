using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using YouiSales.Api.Models;
using YouiSales.Api.Services.Interfaces;

namespace YouiSales.Api.Controllers
{
    /// <summary>
    /// Order Controller.
    /// </summary>
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        /// <summary>
        /// Order Controller Constructor
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="orderService">Order Service</param>
        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        /// <summary>
        /// Add line
        /// </summary>
        /// <param name="line">Content on the line object</param>
        /// <returns>Return success true/false</returns>
        [HttpPost("addline")]
        [AllowAnonymous]
        public async Task<IActionResult> AddLineAsync([FromBody] dynamic line)
        {
            _logger?.LogDebug("'AddLine' has been invoked.");

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {                
                OrderResponse response = await _orderService.AddLine(line);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical($"There was an error on 'AddLineAsync' invocation: {ex.InnerException.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, new OrderResponse { ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Receipt for the order.
        /// </summary>
        /// <returns>Return result with Receipt</returns>
        [HttpGet("receipt/{company}")]
        public async Task<IActionResult> ReceiptAsync(string company)
        {
            _logger?.LogDebug("'Receipt' has been invoked.");

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                OrderResponse response = await _orderService.Receipt(company);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical($"There was an error on 'ReceiptAsync' invocation: {ex.InnerException.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, new OrderResponse { ErrorMessage = ex.Message });
            }
        }

        /// <summary>
        /// HTML Receipt for the order.
        /// </summary>
        /// <returns>Return string with HTML Receipt</returns>
        [HttpGet("htmlreceipt/{company}")]
        public async Task<IActionResult> HtmlReceiptAsync(string company)
        {
            _logger?.LogDebug("'HtmlReceipt' has been invoked.");

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                OrderResponse response = await _orderService.HtmlReceipt(company);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical($"There was an error on 'HtmlReceiptAsync' invocation: {ex.InnerException.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, new OrderResponse { ErrorMessage = ex.Message });
            }
        }
    }
}
