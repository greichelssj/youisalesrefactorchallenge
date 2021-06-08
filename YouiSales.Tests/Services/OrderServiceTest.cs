using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xunit;
using YouiSales.Api.Models;
using YouiSales.Api.Services;
using YouiSales.Common.Models;

namespace YouiSales.Tests.Services
{
    public class OrderServiceTest
    {
        private readonly OrderService _orderService;

        private readonly static Policy BMW = new Policy("Jane Doe", "BMW", Policy.Car);
        private readonly static Policy Harley = new Policy("John Doe", "Harley", Policy.Motorcycle);
        private readonly static Policy SunnyCoast = new Policy("John Doe", "Sunshine Coast", Policy.Home);

        public OrderServiceTest()
        {
            // Arrange
            Mock<ILogger<OrderService>> mockLogger = new Mock<ILogger<OrderService>>();
            MemoryCache mockCache = new MemoryCache(new MemoryCacheOptions());
            _orderService = new OrderService(mockLogger.Object, mockCache);
        }

        [Fact]
        public async Task AddLineAsync_ReturnsSuccess()
        {
            // Act
            OrderResponse actual = await _orderService.AddLine(JsonConvert.SerializeObject(new Line(BMW, 1)));

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
        }

        [Fact]
        public async Task ReceiptOneBMWAsync_ReturnsSuccess()
        {
            // Act
            OrderResponse addLine = await _orderService.AddLine(JsonConvert.SerializeObject(new Line(BMW, 1)));
            OrderResponse actual = await _orderService.Receipt("Youi");

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            string ResultStatementOneBMW = @"Order Receipt for Youi
	1 x Jane Doe BMW = $105.00
Sub-Total: $105.00
Tax: $10.50
Total: $115.50
Date: Friday, 25 October 2019 9:07:27 AM";
            Assert.Equal(ResultStatementOneBMW, actual.Result);
        }

        [Fact]
        public async Task ReceiptOneHarleyAsync_ReturnsSuccess()
        {
            // Act
            OrderResponse addLine = await _orderService.AddLine(JsonConvert.SerializeObject(new Line(Harley, 1)));
            OrderResponse actual = await _orderService.Receipt("Youi");

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            string ResultStatementOneHarley = @"Order Receipt for Youi
	1 x John Doe Harley = $56.00
Sub-Total: $56.00
Tax: $5.60
Total: $61.60
Date: Friday, 25 October 2019 9:07:27 AM";
            Assert.Equal(ResultStatementOneHarley, actual.Result);
        }

        [Fact]
        public async Task ReceiptOneSunnyCoastAsync_ReturnsSuccess()
        {
            // Act
            OrderResponse addLine = await _orderService.AddLine(JsonConvert.SerializeObject(new Line(SunnyCoast, 1)));
            OrderResponse actual = await _orderService.Receipt("Youi");

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            string ResultStatementOneSunnyCoast = @"Order Receipt for Youi
	1 x John Doe Sunshine Coast = $235.00
Sub-Total: $235.00
Tax: $23.50
Total: $258.50
Date: Friday, 25 October 2019 9:07:27 AM";
            Assert.Equal(ResultStatementOneSunnyCoast, actual.Result);
        }

        [Fact]
        public async Task HtmlReceiptOneBMWAsync_ReturnsSuccess()
        {
            // Act
            OrderResponse addLine = await _orderService.AddLine(JsonConvert.SerializeObject(new Line(BMW, 1)));
            OrderResponse actual = await _orderService.HtmlReceipt("Youi");

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            string HtmlResultStatementOneBMW = @"<html><body><h1>Order Receipt for Youi</h1><ul><li>1 x Jane Doe BMW = $105.00</li></ul><h3>Sub-Total: $105.00</h3><h3>Tax: $10.50</h3><h2>Total: $115.50</h2><h3>Date: Friday, 25 October 2019 9:07:27 AM</h3></body></html>";
            Assert.Equal(HtmlResultStatementOneBMW, actual.Result);
        }

        [Fact]
        public async Task HtmlReceiptOneHarleyAsync_ReturnsSuccess()
        {
            // Act
            OrderResponse addLine = await _orderService.AddLine(JsonConvert.SerializeObject(new Line(Harley, 1)));
            OrderResponse actual = await _orderService.HtmlReceipt("Youi");

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            string HtmlResultStatementOneHarley = @"<html><body><h1>Order Receipt for Youi</h1><ul><li>1 x John Doe Harley = $56.00</li></ul><h3>Sub-Total: $56.00</h3><h3>Tax: $5.60</h3><h2>Total: $61.60</h2><h3>Date: Friday, 25 October 2019 9:07:27 AM</h3></body></html>";
            Assert.Equal(HtmlResultStatementOneHarley, actual.Result);
        }

        [Fact]
        public async Task HtmlReceiptOneSunnyCoastAsync_ReturnsSuccess()
        {
            // Act
            OrderResponse addLine = await _orderService.AddLine(JsonConvert.SerializeObject(new Line(SunnyCoast, 1)));
            OrderResponse actual = await _orderService.HtmlReceipt("Youi");

            // Assert
            Assert.NotNull(actual);
            Assert.True(actual.Success);
            string HtmlResultStatementOneSunnyCoast = @"<html><body><h1>Order Receipt for Youi</h1><ul><li>1 x John Doe Sunshine Coast = $235.00</li></ul><h3>Sub-Total: $235.00</h3><h3>Tax: $23.50</h3><h2>Total: $258.50</h2><h3>Date: Friday, 25 October 2019 9:07:27 AM</h3></body></html>";
            Assert.Equal(HtmlResultStatementOneSunnyCoast, actual.Result);
        }
    }
}
