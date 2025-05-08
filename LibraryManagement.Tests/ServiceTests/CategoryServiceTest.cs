using AutoMapper;
using LibraryApp.Application.DTOs.Category;
using LibraryApp.Application.Services;
using LibraryApp.Domain.Entities;
using LibraryApp.Domain.Interfaces;
using Moq;

namespace LibraryApp.Tests.ServiceTests
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _repoMock;
        private Mock<IMapper> _mapperMock;
        private CategoryService _service;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new CategoryService(_repoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsMappedCategories()
        {
            var categories = new List<Category> { new(), new() };
            var dtos = new List<CategoryDto> { new(), new() };

            _repoMock.Setup(r => r.GetAllCategoriesAsync()).ReturnsAsync(categories);
            _mapperMock.Setup(m => m.Map<List<CategoryDto>>(categories)).Returns(dtos);

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.EqualTo(dtos));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsMappedCategory()
        {
            var id = Guid.NewGuid();
            var category = new Category { Id = id };
            var dto = new CategoryDto { Id = id };

            _repoMock.Setup(r => r.GetCategoryByIdAsync(id)).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map<CategoryDto>(category)).Returns(dto);

            var result = await _service.GetByIdAsync(id);

            Assert.That(result, Is.EqualTo(dto));
        }

        [Test]
        public async Task CreateAsync_CreatesAndReturnsCategory()
        {
            var createDto = new CreateCategoryDto { Name = "New" };
            var category = new Category { Name = "New" };
            var resultDto = new CategoryDto { Name = "New" };

            _mapperMock.Setup(m => m.Map<Category>(createDto)).Returns(category);
            _mapperMock.Setup(m => m.Map<CategoryDto>(category)).Returns(resultDto);

            var result = await _service.CreateAsync(createDto);

            _repoMock.Verify(r => r.AddCategoryAsync(category), Times.Once);
            Assert.That(result, Is.EqualTo(resultDto));
        }

        [Test]
        public async Task UpdateAsync_UpdatesAndReturnsCategory_WhenExists()
        {
            var updateDto = new UpdateCategoryDto { Id = Guid.NewGuid(), Name = "Updated" };
            var category = new Category { Id = updateDto.Id };
            var resultDto = new CategoryDto { Id = updateDto.Id, Name = "Updated" };

            _repoMock.Setup(r => r.GetCategoryByIdAsync(updateDto.Id)).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map(updateDto, category));
            _mapperMock.Setup(m => m.Map<CategoryDto>(category)).Returns(resultDto);

            var result = await _service.UpdateAsync(updateDto);

            _repoMock.Verify(r => r.UpdateCategoryAsync(category), Times.Once);
            Assert.That(result, Is.EqualTo(resultDto));
        }

        [Test]
        public async Task UpdateAsync_ReturnsNull_WhenCategoryNotFound()
        {
            var updateDto = new UpdateCategoryDto { Id = Guid.NewGuid() };

            _repoMock.Setup(r => r.GetCategoryByIdAsync(updateDto.Id)).ReturnsAsync((Category?)null);

            var result = await _service.UpdateAsync(updateDto);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteAsync_DeletesAndReturnsTrue_WhenCategoryExists()
        {
            var id = Guid.NewGuid();
            var category = new Category { Id = id };

            _repoMock.Setup(r => r.GetCategoryByIdAsync(id)).ReturnsAsync(category);

            var result = await _service.DeleteAsync(id);

            _repoMock.Verify(r => r.DeleteCategoryAsync(category), Times.Once);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalse_WhenCategoryNotFound()
        {
            var id = Guid.NewGuid();

            _repoMock.Setup(r => r.GetCategoryByIdAsync(id)).ReturnsAsync((Category?)null);

            var result = await _service.DeleteAsync(id);

            Assert.That(result, Is.False);
        }
    }
}