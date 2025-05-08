using LibraryApp.Api.Controllers;
using LibraryApp.Application.DTOs.Category;
using LibraryApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryApp.Tests.ControllerTests
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> _categoryServiceMock;
        private CategoryController _controller;

        [SetUp]
        public void SetUp()
        {
            _categoryServiceMock = new Mock<ICategoryService>();
            _controller = new CategoryController(_categoryServiceMock.Object);
        }

        [Test]
        public async Task GetAllCategories_ReturnsOk_WhenCategoriesExist()
        {
            var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = Guid.NewGuid(), Name = "Category 1" },
                new CategoryDto { Id = Guid.NewGuid(), Name = "Category 2" }
            };

            _categoryServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

            var result = await _controller.GetAllCategories();

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(categories));
            });
        }

        [Test]
        public async Task GetCategoryById_ReturnsOk_WhenCategoryExists()
        {
            var categoryId = Guid.NewGuid();
            var category = new CategoryDto { Id = categoryId, Name = "Category 1" };

            _categoryServiceMock.Setup(s => s.GetByIdAsync(categoryId)).ReturnsAsync(category);

            var result = await _controller.GetCategoryById(categoryId);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(category));
            });
        }

        [Test]
        public async Task GetCategoryById_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            var categoryId = Guid.NewGuid();
            _categoryServiceMock.Setup(s => s.GetByIdAsync(categoryId)).ReturnsAsync((CategoryDto)null!);

            var result = await _controller.GetCategoryById(categoryId);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateCategory_ReturnsCreatedAtAction_WhenModelIsValid()
        {
            var dto = new CreateCategoryDto { Name = "New Category" };
            var createdCategory = new CategoryDto { Id = Guid.NewGuid(), Name = dto.Name };

            _categoryServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(createdCategory);

            var result = await _controller.CreateCategory(dto);

            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(createdAtActionResult.StatusCode, Is.EqualTo(201));
                Assert.That(createdAtActionResult.RouteValues?["id"], Is.EqualTo(createdCategory.Id));
                Assert.That(createdAtActionResult.Value, Is.EqualTo(createdCategory));
            });
        }

        [Test]
        public async Task CreateCategory_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.CreateCategory(new CreateCategoryDto());

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task UpdateCategory_ReturnsOk_WhenCategoryIsUpdated()
        {
            var categoryId = Guid.NewGuid();
            var dto = new UpdateCategoryDto { Id = categoryId, Name = "Updated Category" };
            var updatedCategory = new CategoryDto { Id = categoryId, Name = dto.Name };

            _categoryServiceMock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(updatedCategory);

            var result = await _controller.UpdateCategory(categoryId, dto);

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(okResult.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(updatedCategory));
            });
        }

        [Test]
        public async Task UpdateCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            var categoryId = Guid.NewGuid();
            var dto = new UpdateCategoryDto { Id = categoryId, Name = "Updated Category" };

            _categoryServiceMock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync((CategoryDto?)null);

            var result = await _controller.UpdateCategory(categoryId, dto);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeleteCategory_ReturnsNoContent_WhenCategoryIsDeleted()
        {
            var categoryId = Guid.NewGuid();
            _categoryServiceMock.Setup(s => s.DeleteAsync(categoryId)).ReturnsAsync(true);

            var result = await _controller.DeleteCategory(categoryId);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            var categoryId = Guid.NewGuid();
            _categoryServiceMock.Setup(s => s.DeleteAsync(categoryId)).ReturnsAsync(false);

            var result = await _controller.DeleteCategory(categoryId);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
