﻿using SuperHeroes.WebSite.Controllers;
using SuperHeroes.WebSite.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static SuperHeroes.WebSite.Controllers.ProductsController;

namespace UnitTests.Controllers
{
    /// <summary>
    /// Unit tests for productcontroller class
    /// </summary>
    public class ProductControllerTests
    {
        /// <summary>
        /// Creating an instance
        /// </summary>
        public static ProductsController testProductController;

        /// <summary>
        /// Test initialize
        /// </summary>
        #region TestSetup

        [SetUp]
        public void Testinitialize()
        {
            testProductController = new ProductsController(TestHelper.ProductService);
        }

        #endregion TestSetup

        // Testing the ProductController.cs file
        #region ControllerTests

        /// <summary>
        /// Testing if get is valid should return products
        /// </summary>
        [Test]
        public void Get_Valid_Should_Return_List_Of_Products()
        {
            //Arrange
            var data = testProductController.Get().ToList();

            //Act

            //Assert
            Assert.AreEqual(typeof(List<ProductModel>), data.GetType());
        }

        /// <summary>
        /// Testing get valid toString should return string
        /// </summary>
        [Test]
        public void Get_Valid_ToString_Should_Return_String()
        {
            //Arrange
            var data = testProductController.Get().FirstOrDefault().ToString();

            //Act

            //Assert
            Assert.AreEqual(typeof(string), data.GetType());
        }

        /// <summary>
        /// Testing patch valid should return ok
        /// </summary>
        [Test]
        public void Patch_Valid_Should_Return_Ok()
        {
            //Arrange
            //A new variable of type RatingRequest
            var data = new RatingRequest
            {
                ProductId = "",
                Rating = 5
            };

            //A variable to hold the request
            var result = testProductController.Patch(data);

            //Act
            var okResult = result as OkResult;

            //Assert
            Assert.AreEqual(200, okResult.StatusCode);
        }

        #endregion ControllerTests
        // Ending Testing for ProductController.cs file
    }
}