﻿using System.Linq;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
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

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            var MockLoggerDirect = Mock.Of<ILogger<IndexModel>>();
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            pageModel = new UpdateModel(productService)
            {
            };
        }

        #endregion TestSetup

        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange

            // Act
            pageModel.OnGet("sailorhg-bubblesortpic");

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual("Spider-Man", pageModel.Product.Title);
        }

        [Test]
        public void OnGet_InValid_Should_Not_Return_Products()
        {
            // Arrange

            // Act
            pageModel.OnGet("Test");

            // Assert
            Assert.IsNull(pageModel.Product);
        }

        [Test]
        public void OnGet_Valid_Id_Null_Title_Should_Return_False()
        {
            // Arrange

            // Act
            pageModel.OnGet("sailorhg-bubblesortpic");
            var result = pageModel.Product.Title == null;

            // Assert
            Assert.AreEqual(false, result);
        }

        #endregion OnGet

        #region OnPost
        [Test]
        public void OnPost_Valid_Should_Return_Products()
        {
            // Arrange
            pageModel.OnGet("sailorhg-corsage");

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.AreEqual(true, pageModel.ModelState.IsValid);
            Assert.AreEqual(true, result.PageName.Contains("Index"));
        }

        [Test]
        public void OnPost_InValid_Model_Not_Valid_Return_Page()
        {
            // Arrange
            pageModel.Product = new ContosoCrafts.WebSite.Models.ProductModel
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
        }

        #endregion OnPost

    }
}