using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossSolar.Controllers;
using CrossSolar.Domain;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CrossSolar.Tests.Controller
{
    public class AnalyticsControllerTests
    {

        private readonly AnalyticsController _analyticsController;

        private readonly Mock<IAnalyticsRepository> _analyticsRepositoryMock = new Mock<IAnalyticsRepository>();
        private readonly Mock<IPanelRepository> _panelRepositoryMock = new Mock<IPanelRepository>();

        public AnalyticsControllerTests()
        {
            _analyticsController = new AnalyticsController(_analyticsRepositoryMock.Object, _panelRepositoryMock.Object);
        }

        [Fact]
        public async Task Retrieve_GetAnalystics()
        {
            string panelId = "AAAA1111BBBB2222";

            var mockPanels = new List<Panel>()
            {
                new Panel
                {
                    Brand = "Areva",
                    Latitude = 12.345678,
                    Longitude = 98.765543,
                    Serial = panelId
                }
            }.AsQueryable().BuildMock();

            var mockOneHourElectricities = new List<OneHourElectricity>() {
            new OneHourElectricity()
            {
                DateTime = new DateTime(2018, 9, 8),
                Id = 1,
                KiloWatt = 100,
                PanelId = panelId
            }
                }.AsQueryable().BuildMock();

            _panelRepositoryMock.Setup(m => m.Query()).Returns(mockPanels.Object);
            _analyticsRepositoryMock.Setup(m => m.Query()).Returns(mockOneHourElectricities.Object);

            // Act
            var result = await _analyticsController.Get(panelId);

            // Assert
            Assert.NotNull(result);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task Retrieve_DayResultsAnalystics()
        {
            string panelId = "AAAA1111BBBB2222";

            var mockPanels = new List<Panel>()
            {
                new Panel
                {
                    Brand = "Areva",
                    Latitude = 12.345678,
                    Longitude = 98.765543,
                    Serial = panelId
                }
            }.AsQueryable().BuildMock();

            _panelRepositoryMock.Setup(m => m.Query()).Returns(mockPanels.Object);

            var mockOneDayElectricities = new List<OneHourElectricity>() {
                new OneHourElectricity()
                {
                    Id = 3,
                    PanelId = panelId,
                    DateTime = new DateTime(2018, 9, 8),
                    KiloWatt = 100
                },
                new OneHourElectricity()
                {
                    Id = 4,
                    PanelId = panelId,
                    DateTime = new DateTime(2018, 9, 7),
                    KiloWatt = 200
                },
                new OneHourElectricity()
                {
                    Id = 5,
                    PanelId = panelId,
                    DateTime = new DateTime(2018, 9, 7),
                    KiloWatt = 300
                }
            }.AsQueryable().BuildMock();

            _analyticsRepositoryMock.Setup(m => m.Query()).Returns(mockOneDayElectricities.Object);

            // Act
            var result = await _analyticsController.DayResults(panelId);

            // Assert
            Assert.NotNull(result);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }


        [Fact]
        public async Task Create_PostAnalystics()
        {
            string panelId = "1234567890987654";

            var mockPanels = new List<Panel>()
            {
                new Panel
                {
                    Brand = "Areva",
                    Latitude = 12.345678,
                    Longitude = 98.765543,
                    Serial = panelId
                }
            }.AsQueryable().BuildMock();

            _panelRepositoryMock.Setup(m => m.Query()).Returns(mockPanels.Object);

            var oneHourElectricityModel = new OneHourElectricityModel
            {
                Id = 1,
                DateTime = DateTime.Now,
                KiloWatt = 100
            };
            // Act
            var result = await _analyticsController.Post(panelId, oneHourElectricityModel);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task Create_PostAnalystics_V2_UnitTypeWatt_KiloWatt()
        {
            string panelId = "1234567890987654";

            var mockPanels = new List<Panel>()
            {
                new Panel
                {
                    Brand = "Areva",
                    Latitude = 12.345678,
                    Longitude = 98.765543,
                    Serial = panelId
                }
            }.AsQueryable().BuildMock();

            _panelRepositoryMock.Setup(m => m.Query()).Returns(mockPanels.Object);

            var oneHourElectricityByTypeModel = new OneHourElectricityByTypeModel
            {
                Id = 1,
                DateTime = DateTime.Now,
                ElectricAmount = 1000
            };
            // Act
            var result = await _analyticsController.Post(panelId, oneHourElectricityByTypeModel);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);

            var oneHourElectricityByTypeModelReturn = createdResult.Value as OneHourElectricityByTypeModel;
            Assert.NotNull(oneHourElectricityByTypeModelReturn);

            // Both KiloWatt
            Assert.Equal(oneHourElectricityByTypeModel.ElectricAmount, oneHourElectricityByTypeModelReturn.ElectricAmount);
        }

        [Fact]
        public async Task Create_PostAnalystics_V2_UnitTypeWatt_Watt()
        {
            string panelId = "1234567890987654";

            var mockPanels = new List<Panel>()
            {
                new Panel
                {
                    Brand = "Areva",
                    Latitude = 12.345678,
                    Longitude = 98.765543,
                    Serial = panelId
                }
            }.AsQueryable().BuildMock();

            _panelRepositoryMock.Setup(m => m.Query()).Returns(mockPanels.Object);

            var oneHourElectricityByTypeModel = new OneHourElectricityByTypeModel
            {
                Id = 1,
                DateTime = DateTime.Now,
                ElectricAmount = 100,
                UnitType = ElectricUnitType.Watt
            };
            // Act
            var result = await _analyticsController.Post(panelId, oneHourElectricityByTypeModel);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);

            var oneHourElectricityByTypeModelReturn = createdResult.Value as OneHourElectricityByTypeModel;
            Assert.NotNull(oneHourElectricityByTypeModelReturn);

            // Sent Watt, Return KiloWatt
            double amountKiloWatt = oneHourElectricityByTypeModel.ElectricAmount / 1000;
            Assert.Equal(amountKiloWatt, oneHourElectricityByTypeModelReturn.ElectricAmount);
        }
    }
}