using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using StoreBackend.Controllers;
using StoreBackend.DTOs;
using StoreBackend.Exceptions;
using StoreBackend.Interfaces;
using StoreBackend.Models;
using StoreBackend.Tests.Factories;
using Xunit;

namespace StoreBackend.Tests
{
    public class BasketControllerTests
    {
        private Mock<IBasketControllerFacade> BasketControllerFacadeStub;
        private Mock<IBasketRepository> BasketRepositoryStub;
        public const int Basket1Id = 1;
        private const int Basket2Id = 2;
        public BasketControllerTests()
        {
            InitMocks();
        }

        [Fact]
        public async void GetBasket_WithNoParameters_ReturnsAllBaskets()
        {
            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var result = await SUT.GetBasket();
            var basketList = result.Value;

            basketList.Should().BeAssignableTo<IEnumerable<BasketDTO>>();
            basketList.Should().HaveCount(2);
        }

        [Fact]
        public async void GetBasket_WithNonexistingBasketId_ReturnsNotFound()
        {
            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var nonexistentBasketId = -1;
            var result = await SUT.GetBasket(nonexistentBasketId);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void GetBasket_WithExistingBasketId_ReturnsCorrectBasket()
        {
            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var result = await SUT.GetBasket(Basket1Id);

            result.Value.Should().BeOfType<BasketDTO>();
            result.Value.Id.Should().Be(Basket1Id);
        }

        [Fact]
        public async void PutBasket_WithMismatchedBasketIds_ReturnsBadRequest()
        {
            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var basketId = 1;
            var updateBasketDTO = new UpdateBasketDTO { Basket = new Basket { Id = 2 } };
            var result = await SUT.PutBasket(basketId, updateBasketDTO);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void PutBasket_WithNonExistentBasketId_ReturnsNotFound()
        {
            BasketRepositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<Basket>())).ReturnsAsync(false);

            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var nonexistentBasketId = -1;
            var updateBasketDTO = new UpdateBasketDTO { Basket = new Basket { Id = -1 }, ProductIds = new[] { 1 } };
            var result = await SUT.PutBasket(nonexistentBasketId, updateBasketDTO);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void PutBasket_WithExistingBasketId_ReturnsNoContent()
        {
            BasketRepositoryStub.Setup(repo => repo.UpdateAsync(It.IsAny<Basket>())).ReturnsAsync(true);

            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var updateBasketDTO = new UpdateBasketDTO { Basket = new Basket { Id = Basket1Id }, ProductIds = new[] { 1 } };
            var result = await SUT.PutBasket(Basket1Id, updateBasketDTO);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async void PostBasket_WithBasketToPost_ReturnsPostedBasket()
        {
            var newBasketId = 3;
            var newBasket = new Basket { Id = newBasketId };

            BasketRepositoryStub.Setup(repo => repo.IncludeAsync(It.IsAny<Expression<Func<Basket, object>>>()))
            .ReturnsAsync(new[] { newBasket }.AsQueryable().BuildMock().Object.Include(p => p));

            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var result = await SUT.PostBasket(newBasket);
            var basket = (result.Result as CreatedAtActionResult).Value as BasketDTO;

            basket.Should().BeEquivalentTo(newBasket);
        }

        [Fact]
        public async void DeleteBasket_WithNonexistentBasketId_ReturnsNotFound()
        {
            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var nonexistentBasketId = -1;
            var result = await SUT.DeleteBasket(nonexistentBasketId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void DeleteBasket_WithExistentBasketId_ReturnsNoContent()
        {
            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var result = await SUT.DeleteBasket(Basket1Id);
            BasketRepositoryStub.Verify(repo => repo.DeleteAsync(It.IsAny<Basket>()), Times.Once);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async void SetDiscountCode_WithNonexistentBasketId_ReturnsNotFound()
        {
            var nonexistentBasketId = -1;
            BasketControllerFacadeStub.Setup(facade => facade.BasketDiscountService.SetDiscountOnBasketAsync(It.IsAny<int>(), It.IsAny<string>()))
            .Throws(new BasketNotFoundException(nonexistentBasketId));

            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var result = await SUT.SetDiscountCode(nonexistentBasketId, "SALE20");

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async void SetDiscountCode_WithNonexistentDiscountCode_ReturnsNotFound()
        {
            var nonexistentDiscountCode = "InvalidDiscountCode";
            BasketControllerFacadeStub.Setup(facade =>
                facade.BasketDiscountService.SetDiscountOnBasketAsync(It.IsAny<int>(), It.IsAny<string>()))

            .Throws(new DiscountCodeNotFoundException(nonexistentDiscountCode));

            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var result = await SUT.SetDiscountCode(Basket1Id, nonexistentDiscountCode);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async void SetDiscountCode_WithExistingBasketAndDiscount_ReturnsNoContent()
        {
            BasketControllerFacadeStub.Setup(facade =>
                facade.BasketDiscountService.SetDiscountOnBasketAsync(It.IsAny<int>(), It.IsAny<string>()));

            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var result = await SUT.SetDiscountCode(Basket1Id, "SALE10");

            BasketControllerFacadeStub.Verify(facade =>
                facade.BasketDiscountService.SetDiscountOnBasketAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async void GetBasketTotalCost_WithNonexistentBasketId_ReturnsNotFound()
        {
            var nonexistentBasketId = -1;
            BasketControllerFacadeStub.Setup(facade =>
                facade.QueryTotalCostOfBasketCommand.Execute(It.IsAny<int>()))
                .Throws(new BasketNotFoundException(nonexistentBasketId));

            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var result = await SUT.GetBasketTotalCost(nonexistentBasketId);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async void GetBasketTotalCost_WithExistentBasketId_ReturnsOk()
        {
            var testBasketTotalCost = 9.99M;
            BasketControllerFacadeStub.Setup(facade =>
                facade.QueryTotalCostOfBasketCommand.Execute(It.IsAny<int>())).ReturnsAsync(testBasketTotalCost);

            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var result = await SUT.GetBasketTotalCost(Basket1Id);

            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().Be(testBasketTotalCost);
        }

        [Fact]
        public async void GetBasketTotalCostAfterDiscount_WithExistentBasketId_ReturnsOk()
        {
            var testBasketTotalCostAfterDiscount = 7.49M;
            BasketControllerFacadeStub.Setup(facade => facade.BasketDiscountService.QueryBasketTotalCostAfterDiscountAsync(It.IsAny<int>()))
                .ReturnsAsync(testBasketTotalCostAfterDiscount);

            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var result = await SUT.GetBasketTotalCostAfterDiscount(Basket1Id);

            result.Should().BeOfType<OkObjectResult>();
            (result as OkObjectResult).Value.Should().Be(testBasketTotalCostAfterDiscount);
        }

        [Fact]
        public async void GetBasketTotalCostAfterDiscount_WithNonexistentBasketId_ReturnsNotFound()
        {
            var nonexistentBasketId = -1;
            BasketControllerFacadeStub.Setup(facade => facade.BasketDiscountService.QueryBasketTotalCostAfterDiscountAsync(It.IsAny<int>()))
                .Throws(new BasketNotFoundException(nonexistentBasketId));

            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var result = await SUT.GetBasketTotalCostAfterDiscount(Basket1Id);

            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async void GetBasketTotalCostAfterDiscount_WithNonexistentDiscountCode_ReturnsBadRequest()
        {
            BasketControllerFacadeStub.Setup(facade => facade.BasketDiscountService.QueryBasketTotalCostAfterDiscountAsync(It.IsAny<int>()))
                .Throws<InvalidOperationException>();

            var SUT = new BasketController(BasketRepositoryStub.Object, BasketControllerFacadeStub.Object);

            var result = await SUT.GetBasketTotalCostAfterDiscount(Basket1Id);

            result.Should().BeOfType<BadRequestObjectResult>();
        }


        private void InitMocks()
        {
            // Data
            var user1 = TestUserFactory.CreateRandomUser(1);
            var user2 = TestUserFactory.CreateRandomUser(2);

            var productsList = new[]
            {
                TestProductFactory.CreateProduct(1, 7.99M),
                TestProductFactory.CreateProduct(2, 5M),
            };

            var (basket1, basketProductsList1) = TestBasketFactory.CreateBasketWithExistingProducts(Basket1Id, user1.Id, productsList);

            var mockBasketQueryable = new List<Basket> {
            basket1,
            TestBasketFactory.CreateEmptyBasket(Basket2Id, user2.Id)
            }.AsQueryable().BuildMock().Object;

            var productsInBasketQueryable = new[] { productsList[0] }.AsQueryable();
            var productIdsInBasketQueryable = new[] { productsList[0].Id }.AsQueryable();

            // Mocks
            BasketRepositoryStub = new Mock<IBasketRepository>();
            BasketRepositoryStub.Setup(repo => repo.GetOneAsync(Basket1Id)).ReturnsAsync(basket1);
            BasketRepositoryStub.Setup(repo => repo.GetAllAsync()).ReturnsAsync(mockBasketQueryable);
            BasketRepositoryStub.Setup(repo => repo.CreateAsync(It.IsAny<Basket>()));
            BasketRepositoryStub.Setup(repo => repo.DeleteAsync(It.IsAny<Basket>()));

            BasketRepositoryStub.Setup(repo => repo.IncludeAsync(It.IsAny<Expression<Func<Basket, object>>>()))
            .ReturnsAsync(mockBasketQueryable.Include(p => p));

            BasketControllerFacadeStub = new Mock<IBasketControllerFacade>();
            BasketControllerFacadeStub.Setup(facade => facade.AddProductsToBasketCommand.Execute(It.IsAny<(int, IEnumerable<int>)>()));
            BasketControllerFacadeStub.Setup(facade => facade.RemoveProductsFromBasketCommand.Execute(It.IsAny<(int, IEnumerable<int>)>()));

            BasketControllerFacadeStub.Setup(facade => facade.QueryProductsInBasketCommand.Execute(It.IsAny<int>()))
            .ReturnsAsync(productsInBasketQueryable);

            BasketControllerFacadeStub.Setup(facade => facade.QueryProductIdsInBasketCommand.Execute(It.IsAny<int>()))
            .ReturnsAsync(productIdsInBasketQueryable);
        }
    }
}
