using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using qcs_product.API.BusinessProviders;
using qcs_product.API.Controllers;
using Moq;
using qcs_product.API.ViewModels;
using Xunit;

namespace qcs_product.UnitTests
{
    public class QcRequestUnitTest
    {   
        private QcRequestController _controller;
        private Mock<IQcRequestBusinessProvider> _businessProviderMock;

        public QcRequestUnitTest()
        {
            _businessProviderMock = new Mock<IQcRequestBusinessProvider>();
            // _controller = new QcRequestController(_businessProviderMock.Object);
        }

        [Fact]
        public async Task List_ReturnsStatusCode200_WhenSuccessful()
        {
            // // Arrange
            // string search = "example";
            // int limit = 10;
            // int page = 1;
            // DateTime? startDate = DateTime.Now;
            // DateTime? endDate = DateTime.Now.AddDays(7);
            // string status = "active";

            // var expectedResult = new ResponseViewModel<ListShortQcRequestViewModel>
            // {
            //     StatusCode = 200,
            //     Message = "Success",
            //     Data = new List<ListShortQcRequestViewModel>
            //     {
            //         new ListShortQcRequestViewModel
            //         {
            //             Id = 1,
            //             Status = 1,
            //             DateRequest = new DateOnly(),
            //             ItemName = "test",
            //             NoBatch = "1234",
            //             NoRequest = "1234",
            //             StatusName = "draft",
            //         }
            //     },
            //     Meta = new MetaViewModel
            //     {
            //         TotalItem = 1,
            //         TotalPages = 1
            //     }
            // };
            
            
            // _businessProviderMock.Setup(b => b.ListShort(search, limit, page, startDate, endDate, status))
            //     .ReturnsAsync(expectedResult);

            // // Act
            // var result = await _controller.List(search, limit, page, startDate, endDate, status) as ObjectResult;
            // var actualResult = result.Value as ResponseViewModel<ListShortQcRequestViewModel>;

            // // Assert
            // Assert.NotNull(result);
            // Assert.Equal(200, result.StatusCode);
            // Assert.Equal(expectedResult, actualResult);
        }

       
    }
}