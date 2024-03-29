﻿using SuperHeroes.WebSite.Pages.Product;
using SuperHeroes.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests.Pages.Product.Update
{
    public class UpdateTests
    {
        /// <summary>
        /// Test initialize
        /// </summary>
        #region TestSetup
        public static IUrlHelperFactory urlHelperFactory;
        public static DefaultHttpContext httpContextDefault;
        public static IWebHostEnvironment webHostEnvironment;
        public static ModelStateDictionary modelState;
        public static ActionContext actionContext;
        public static EmptyModelMetadataProvider modelMetadataProvider;
        public static ViewDataDictionary viewData;
        public static TempDataDictionary tempData;
        public static PageContext pageContext;

        public static UpdateModel pageModel;

        [SetUp]
        public void TestInitialize()
        {
            httpContextDefault = new DefaultHttpContext()
            {
                //RequestServices = serviceProviderMock.Object,
            };

            modelState = new ModelStateDictionary();

            actionContext = new ActionContext(httpContextDefault, httpContextDefault.GetRouteData(), new PageActionDescriptor(), modelState);

            modelMetadataProvider = new EmptyModelMetadataProvider();
            viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            tempData = new TempDataDictionary(httpContextDefault, Mock.Of<ITempDataProvider>());

            pageContext = new PageContext(actionContext)
            {
                ViewData = viewData,
            };

            ///creating a mockWebHostEnvironment
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            ///creating a MockLoggerDirect
            var MockLoggerDirect = Mock.Of<ILogger<IndexModel>>();
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            pageModel = new UpdateModel(productService)
            {
            };
        }

        #endregion TestSetup

        /// OnGet Method in Update.cshtml.cs file
        #region OnGet

        /// <summary>
        /// Test that's loading the update page returns a non-empty list of products
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Product()
        {
            // Arrange

            // Act
            pageModel.OnGet("bruce-banner");

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);

            // Reset
            // This should remove the error we added
            pageModel.ModelState.Clear();

        }

        /// <summary>
        /// Unit test if getting invalid product gives null
        /// </summary>
        [Test]
        public void OnGet_InValid_Should_Not_Return_Products()
        {
            // Arrange

            // Act
            pageModel.OnGet("Test");

            // Assert
            Assert.IsNull(pageModel.Product);

            // Reset
            pageModel.ModelState.Clear();
        }

        #endregion OnGet
        /// Ending OnGet Method in Update.cshtml.cs file

        /// OnPost Method in Update.cshtml.cs file
        #region OnPost

        /// <summary>
        /// Unit test if valid will return products
        /// </summary>
        [Test]
        public void OnPost_Valid_Should_Return_Products()
        {
            // Arrange
            pageModel.OnGet("t-challa");

            // Act
            ///creating variable result
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));

            // Reset
            pageModel.ModelState.Clear();
        }

        /// <summary>
        /// Unit test if invalid model should return false
        /// </summary>
        [Test]
        public void OnPost_InValid_Model_Not_Valid_Return_Page()
        {
            // Arrange
            pageModel.Product = new SuperHeroes.WebSite.Models.ProductModel
            {
                Id = "testId",
                Title = "Title",
                Description = "Description",
                Url = "url",
                Image = "image"
            };

            // Force an invalid error state
            pageModel.ModelState.AddModelError("Test", "Test error");

            // Act
            var result = pageModel.OnPost() as ActionResult;

            // Assert
            Assert.AreEqual(false, pageModel.ModelState.IsValid);

            // Reset
            pageModel.ModelState.Clear();
        }

        #endregion OnPost
        /// Ending OnPost Method in Update.cshtml.cs file
    }
}