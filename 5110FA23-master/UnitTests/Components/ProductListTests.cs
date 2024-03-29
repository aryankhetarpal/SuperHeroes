﻿using Bunit;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using SuperHeroes.WebSite.Components;
using System.Linq;
using SuperHeroes.WebSite.Models;
using SuperHeroes.WebSite.Services;
using Moq;
using System.Threading.Tasks;
using System;

namespace UnitTests.Components.Tests
{ 
     /// <summary>
     /// This class contains unit tests for the ProductModel
     /// </summary>
    public class ProductListTests : Bunit.TestContext
    {
    public ProductListTests()
        {
            Services.AddSingleton(TestHelper.ProductService);
        }

        /// <summary>
        /// Test for getting the product list
        /// </summary>
        #region Rating

        

       

        #endregion Rating

        #region ProductList

        [Test]
        public void ProductList_Default_Should_Return_Content()
        {
            // Act
            var page = RenderComponent<ProductList>();
            var result = page.Markup;

            // Assert
            Assert.AreEqual(true, result.Contains("t-challa"));
        }

        #endregion ProductList

        #region FilterData

        /// <summary>
        /// Test for testing the enabling the filter function
        /// </summary>

        [Test]
        public void Enable_Filter_Data_Set_to_True_Should_Return_True()
        {
            // Act
            var page = RenderComponent<ProductList>();
            var buttonList = page.FindAll("Button");
            var button = buttonList.First(m => m.OuterHtml.Contains("Filter"));
            button.Click();
            var pageMarkup = page.Markup;

            // Assert
            Assert.AreEqual(true, page.Instance.FilterData);
        }

        /// <summary>
        /// Test for updating the filter
        /// </summary>
        [Test]
        public void UpdateFilterType_ShouldUpdateFilterDataType()
        {
            // Act
            var page = RenderComponent<ProductList>();
            var inputField = page.Find("input[type='text']");
            inputField.Change("NewFilterText");

            // Assert
            Assert.AreEqual("NewFilterText", page.Instance.FilterDataString);
        }

        /// <summary>
        /// Test for clearing out the filter text
        /// </summary>
        [Test]
        public void Clear_Filter_Data_Set_to_False_Should_Return_False()
        {
            // Act
            var clearButton = "Clear";
            var page = RenderComponent<ProductList>();
            var buttonList = page.FindAll("Button");
            var button = buttonList.First(m => m.OuterHtml.Contains(clearButton));
            button.Click();
            var pageMarkup = page.Markup;

            // Assert
            Assert.AreEqual(false, page.Instance.FilterData);
        }

        [Test]
        public void ClearFilterData_ShouldResetFilterData()
        {
            // Arrange
            var page = RenderComponent<ProductList>();
            page.Instance.FilterData = true;
            page.Instance.FilterDataString = "SomeFilterText";

            // Act
            page.Instance.ClearFilterData();

            // Assert
            Assert.IsFalse(page.Instance.FilterData);
            Assert.IsNull(page.Instance.FilterDataString);
        }

        #endregion FilterData

        #region SelectProduct

        /// <summary>
        /// Test for getting content of selected product
        /// </summary>
        [Test]
        public void SelectProduct_Valid_ID_Should_Return_Content()
        {
            // Act
            var id = "1";
            var page = RenderComponent<ProductList>();
            var buttonList = page.FindAll("Button");
            var button = buttonList.First(m => m.OuterHtml.Contains(id));
            button.Click();
            var pageMarkup = page.Markup;

            // Assert
            Assert.AreEqual(true, pageMarkup.Contains("t-challa"));
        }

        #endregion SelectProduct

        #region Comment
        /// <summary>
        /// Test for checking if comment is present
        /// </summary>

        [Test]
        public void Select_comment_should_return_true()
        {
            // Act
            var page = RenderComponent<ProductList>();
            var button = page.Find("#AddComment");
            button.Click();
            var pageMarkup = page.Markup;
            var temp = page.Instance.NewComment;


            page.Instance.NewComment = false;
            page.SetParametersAndRender();
            // Assert
            Assert.AreEqual(true, temp);

        }

        [Test]
        public void ShowNewCommentInput_ShouldSetNewCommentToTrue()
        {
            // Arrange
            var page = RenderComponent<ProductList>();

            // Act
            page.Instance.ShowNewCommentInput();

            // Assert
            Assert.IsTrue(page.Instance.NewComment);
        }

        [Test]
        public void UpdateCommentText_ShouldUpdateNewCommentText()
        {
            // Arrange
            var page = RenderComponent<ProductList>();
            var newCommentText = "New Comment Text";

            // Act
            page.Instance.UpdateCommentText(newCommentText);

            // Assert
            Assert.AreEqual(newCommentText, page.Instance.NewCommentText);
        }

        [Test]
        public void AddComment_ShouldAddCommentToSelectedProduct()
        {
            // Arrange
            var page = RenderComponent<ProductList>();
            page.Instance.selectedProduct = new ProductModel();
            page.Instance.NewCommentText = "New Comment";

            // Act
            page.Instance.AddComment();

            // Assert
            Assert.AreEqual(1, page.Instance.selectedProduct.CommentList.Count);
            Assert.AreEqual("New Comment", page.Instance.selectedProduct.CommentList.First().Comment);
            Assert.IsFalse(page.Instance.NewComment); // Ensure NewComment is set to false after adding a comment
        }

        [Test]
        public void AddComment_SelectedProductIsNull_ShouldNotAddComment()
        {
            // Arrange
            var page = RenderComponent<ProductList>();
            page.Instance.selectedProduct = null;
            page.Instance.NewCommentText = "New Comment";

            // Act
            page.Instance.AddComment();

            // Assert
            Assert.IsNull(page.Instance.selectedProduct); // Ensure selectedProduct is still null
            Assert.AreEqual(1, page.Instance.selectedProduct?.CommentList.Count ?? 1); // Ensure no comment is added
            Assert.IsFalse(page.Instance.NewComment); // Ensure NewComment is not changed
        }
        #endregion Comment

        #region Toggle Products
        [Test]
        public async Task ToggleProductSelection_AddProductToComparisonList()
        {
            // Arrange
            var page = RenderComponent<ProductList>();
            var productIdToAdd = "scarlet-witch";

            // Act
            page.Instance.ToggleProductSelection(productIdToAdd);


            // Wait for the expected state
            await Task.Run(() => page.WaitForAssertion(() => Assert.AreEqual(1, page.Instance.SelectedProductsForComparison.Count)));

            // Assert
            Assert.AreEqual(1, page.Instance.SelectedProductsForComparison.Count);

            Assert.AreEqual(productIdToAdd, page.Instance.AddedToComparisonProductId);

            // Wait for the notification to reset
            await Task.Delay(2000);

            // Assert notification reset
            Assert.IsNull(page.Instance.AddedToComparisonProductId);
        }
        [Test]
        public void ToggleProductSelection_RemoveProductFromComparisonList()
        {
            // Arrange
            var page = RenderComponent<ProductList>();
            var productIdToRemove = "scarlet-witch";
            page.Instance.SelectedProductsForComparison.Add(new ProductModel { Id = productIdToRemove });

            // Debugging output before calling ToggleProductSelection
            Console.WriteLine($"Before ToggleProductSelection - Count: {page.Instance.SelectedProductsForComparison.Count}");

            // Act
            page.Instance.ToggleProductSelection(productIdToRemove);

            // Debugging output after calling ToggleProductSelection
            Console.WriteLine($"After ToggleProductSelection - Count: {page.Instance.SelectedProductsForComparison.Count}");

            // Assert
            Assert.AreEqual(0, page.Instance.SelectedProductsForComparison.Count);
            Assert.IsNull(page.Instance.AddedToComparisonProductId);
        }

        [Test]
        public void ToggleProductSelection_InvalidProductId_NoChangeToComparisonList()
        {
            // Arrange
            var page = RenderComponent<ProductList>();
            var invalidProductId = "invalidId";
            page.Instance.SelectedProductsForComparison.Add(new ProductModel { Id = "1" });

            // Act
            page.Instance.ToggleProductSelection(invalidProductId);

            // Assert
            Assert.AreEqual(1, page.Instance.SelectedProductsForComparison.Count);
            Assert.IsNull(page.Instance.AddedToComparisonProductId);
        }
        #endregion Toggle Product

        #region Rating
        [Test]
        public void GetCurrentRating_NullRatings_ShouldSetCurrentRatingToZero()
        {
            // Arrange
            var page = RenderComponent<ProductList>();
            page.Instance.selectedProduct = new ProductModel { Id = "thunder-god", Ratings = null };

            // Act
            page.Instance.GetCurrentRating();

            // Assert
            Assert.AreEqual(0, page.Instance.currentRating);
            Assert.AreEqual(0, page.Instance.voteCount);
            Assert.AreEqual(null, page.Instance.voteLabel); // Assuming default label is "Vote"
        }
        #endregion Rating
    }
}