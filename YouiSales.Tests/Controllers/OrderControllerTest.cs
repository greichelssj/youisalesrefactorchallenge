using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YouiSales.Api.Controllers;
using YouiSales.Api.Models;
using YouiSales.Api.Services.Interfaces;
using YouiSales.Common.Models;

namespace YouiSales.Tests.Controllers
{
    public class OrderControllerTest
    {
        [Fact]
        public async Task OrderReceiptAsync_ReturnsSuccess()
        {
            // Arrange
            Mock<ILogger<OrderController>> mockLogger = new Mock<ILogger<OrderController>>();
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();

            Policy BMW = new Policy("Jane Doe", "BMW", Policy.Car);

            const string ResultStatementOneBMW = @"Order Receipt for Youi
	1 x Jane Doe BMW = $105.00
Sub-Total: $105.00
Tax: $10.50
Total: $115.50
Date: Friday, 25 October 2019 9:07:27 AM";

            OrderResponse response = new OrderResponse
            {                
                Result = ResultStatementOneBMW,
                Success = true
            };

            mockOrderService.Setup(q => q.AddLine(new Line(BMW, 1)));
            mockOrderService.Setup(q => q.Receipt("Youi")).Returns(Task.FromResult(response));

            OrderController controller = new OrderController(mockLogger.Object, mockOrderService.Object);

            // Act
            IActionResult result = await controller.ReceiptAsync("Youi");

            ObjectResult okObjectResult = result as ObjectResult;

            // Assert
            OrderResponse actual = (OrderResponse)okObjectResult.Value;

            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult is OkObjectResult);
            Assert.IsType<OrderResponse>(okObjectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.True(actual.Success);
        }

        [Fact]
        public async Task OrderHtmlReceiptAsync_ReturnsSuccess()
        {
            // Arrange
            Mock<ILogger<OrderController>> mockLogger = new Mock<ILogger<OrderController>>();
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();

            Policy BMW = new Policy("Jane Doe", "BMW", Policy.Car);

            const string HtmlResultStatementOneBMW = @"<html><body><h1>Order Receipt for Youi</h1><ul><li>1 x Jane Doe BMW = $105.00</li></ul><h3>Sub-Total: $105.00</h3><h3>Tax: $10.50</h3><h2>Total: $115.50</h2><h3>Date: Friday, 25 October 2019 9:07:27 AM</h3></body></html>";

            OrderResponse response = new OrderResponse
            {
                Result = HtmlResultStatementOneBMW,
                Success = true
            };

            mockOrderService.Setup(q => q.AddLine(new Line(BMW, 1)));
            mockOrderService.Setup(q => q.HtmlReceipt("Youi")).Returns(Task.FromResult(response));

            OrderController controller = new OrderController(mockLogger.Object, mockOrderService.Object);

            // Act
            IActionResult result = await controller.HtmlReceiptAsync("Youi");

            ObjectResult okObjectResult = result as ObjectResult;

            // Assert
            OrderResponse actual = (OrderResponse)okObjectResult.Value;

            Assert.NotNull(okObjectResult);
            Assert.True(okObjectResult is OkObjectResult);
            Assert.IsType<OrderResponse>(okObjectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.True(actual.Success);
        }
    }
}
