using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Sales.Domain.Entities;
using Sales.Domain.Interfaces.Repositories;
using Sales.Domain.Models;
using Sales.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class SaleServiceTests
{
    private readonly Mock<ISaleRepository> _saleRepositoryMock;
    private readonly Mock<ILogger<SaleService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly SaleService _saleService;

    public SaleServiceTests()
    {
        _saleRepositoryMock = new Mock<ISaleRepository>();
        _loggerMock = new Mock<ILogger<SaleService>>();
        _mapperMock = new Mock<IMapper>();
        _saleService = new SaleService(_saleRepositoryMock.Object, _loggerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyResponse_WhenNoSales()
    {
        // Arrange
        _saleRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Sale>());

        // Act
        var response = await _saleService.GetAllAsync();

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Nenhuma sale encontrada.", response.Message);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsSales_WhenSalesExist()
    {
        // Arrange
        var sales = new List<Sale> { new Sale { SaleId = 1 } };
        var salesModel = new List<SaleModel> { new SaleModel { IdSale = 1 } };

        _saleRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(sales);
        _mapperMock.Setup(mapper => mapper.Map<List<SaleModel>>(sales)).Returns(salesModel);

        // Act
        var response = await _saleService.GetAllAsync();

        // Assert
        Assert.True(response.Success);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNotFound_WhenSaleDoesNotExist()
    {
        // Arrange
        int saleId = 1;
        _saleRepositoryMock.Setup(repo => repo.GetByIdAsync(saleId)).ReturnsAsync((Sale)null);

        // Act
        var response = await _saleService.GetByIdAsync(saleId);

        // Assert
        Assert.False(response.Success);
        Assert.Equal($"Sale com ID {saleId} não encontrada.", response.Message);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsSale_WhenSaleExists()
    {
        // Arrange
        int saleId = 1;
        var sale = new Sale { SaleId = saleId };
        var saleModel = new SaleModel { IdSale = saleId };

        _saleRepositoryMock.Setup(repo => repo.GetByIdAsync(saleId)).ReturnsAsync(sale);
        _mapperMock.Setup(mapper => mapper.Map<SaleModel>(sale)).Returns(saleModel);

        // Act
        var response = await _saleService.GetByIdAsync(saleId);

        // Assert
        Assert.True(response.Success);
    }

    [Fact]
    public async Task AddAsync_CreatesSale_ReturnsCreatedSale()
    {
        // Arrange
        var saleModel = new SaleModel
        {
            Itens = new List<ItemSaleModel>
            {
                new ItemSaleModel { Quantity = 2, PriceUnit = 10, Discount = 1 }
            }
        };

        var sale = new Sale();
        var createdSale = new Sale { SaleId = 1 };
        var createdSaleModel = new SaleModel { IdSale = 1 };

        _mapperMock.Setup(mapper => mapper.Map<Sale>(saleModel)).Returns(sale);
        _saleRepositoryMock.Setup(repo => repo.AddAsync(sale)).ReturnsAsync(createdSale);
        _mapperMock.Setup(mapper => mapper.Map<SaleModel>(createdSale)).Returns(createdSaleModel);

        // Act
        var response = await _saleService.AddAsync(saleModel);

        // Assert
        Assert.True(response.Success);
    }

    // Adicione testes para UpdateAsync e DeleteAsync de forma similar.
}
