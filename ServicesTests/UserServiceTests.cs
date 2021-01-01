using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumables;
using Models;
using Moq;
using NUnit.Framework;
using Sartain_Studios_Common.Cryptography;
using Sartain_Studios_Common.Logging;
using Services;

namespace ServicesTests
{
    public class UserServiceTests
    {
        private const string SampleId1 = "5eba08740bdc1e00945702411";
        private const string SampleId2 = "8aty08740bdc2e00945706666";

        private static readonly UserModel UserModel1 = new UserModel
        {
            Id = SampleId1
        };

        private static readonly UserModel UserModel2 = new UserModel
        {
            Id = SampleId2
        };

        private static readonly List<UserModel> UserModels1 = new List<UserModel>
        {
            UserModel1,
            UserModel2
        };

        private static readonly List<UserModel> UserModels2 = new List<UserModel>
        {
            new UserModel
            {
                Username = "Jim"
            },
            new UserModel
            {
                Username = "John",
                Password = "Hashed JohnsPassword"
            }
        };

        private Mock<IHasher> _hasherMock;
        private Mock<ILoggerWrapper> _loggerWrapperMock;
        private Mock<IUserConsumable> _userConsumableMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _hasherMock = new Mock<IHasher>();
            _loggerWrapperMock = new Mock<ILoggerWrapper>();
            _userConsumableMock = new Mock<IUserConsumable>();

            _userService = new UserService(_loggerWrapperMock.Object, _userConsumableMock.Object, _hasherMock.Object);
        }

        [Test]
        public async Task GetAllAsync_CallsGetAllAsyncOnceAsync()
        {
            await _userService.GetAllAsync();

            _userConsumableMock.Verify(x => x.GetAllAsync(), Times.Once());
        }

        [Test]
        public async Task GetByIdAsync_CallsGetByIdAsyncOnce()
        {
            await _userService.GetByIdAsync(SampleId1);

            _userConsumableMock.Verify(x => x.GetByIdAsync(SampleId1), Times.Once());
        }

        [Test]
        public async Task GetByIdAsync_ReturnsModelWithSpecifiedId()
        {
            _userConsumableMock.Setup(x => x.GetByIdAsync(SampleId1)).Returns(Task.FromResult(UserModel1));

            var result = await _userService.GetByIdAsync(SampleId1);

            Assert.AreEqual(SampleId1, result.Id);
        }

        [Test]
        public async Task UpdateAsync()
        {
            await _userService.UpdateAsync(SampleId1, UserModel1);

            _userConsumableMock.Verify(x => x.UpdateAsync(SampleId1, UserModel1), Times.Once());
        }

        [Test]
        public async Task CreateAsync()
        {
            await _userService.CreateAsync(UserModel1);

            _userConsumableMock.Verify(x => x.CreateAsync(UserModel1), Times.Once());
        }

        [Test]
        public async Task DeleteAsync()
        {
            await _userService.DeleteAsync(SampleId1);

            _userConsumableMock.Verify(x => x.DeleteAsync(SampleId1), Times.Once());
        }

        [Test]
        public async Task UsernameExistsAsync_ReturnsFalseIfNoModelExistsWithUsername()
        {
            _userConsumableMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(UserModels1.AsEnumerable()));

            var result = await _userService.UsernameExistsAsync(SampleId1);

            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task UsernameExistsAsync_ReturnsTrueIfModelExistsWithUsername()
        {
            _userConsumableMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(UserModels2.AsEnumerable()));

            var result = await _userService.UsernameExistsAsync("John");

            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task AreCredentialsValidAsync_ReturnsFalseIfNoModelExistsForProvidedUsernameAndPassword()
        {
            _userConsumableMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(UserModels1.AsEnumerable()));

            _hasherMock.Setup(x => x.GenerateHash("JohnsPassword")).Returns("Hashed JohnsPassword");

            var result = await _userService.AreCredentialsValidAsync(new UserModel
                {Username = "John", Password = "Hashed JohnsPassword"});

            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task AreCredentialsValidAsync_ReturnsTrueIfModelExistsForProvidedUsernameAndPassword()
        {
            _userConsumableMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(UserModels2.AsEnumerable()));

            _hasherMock.Setup(x => x.GenerateHash("JohnsPassword")).Returns("Hashed JohnsPassword");

            var result = await _userService.AreCredentialsValidAsync(new UserModel
                {Username = "John", Password = "JohnsPassword"});

            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task GetQuantityOfUsersAsync_ReturnsQuantityOfUserModels()
        {
            _userConsumableMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(UserModels1.AsEnumerable()));

            var result = await _userService.GetQuantityOfUsersAsync();

            Assert.AreEqual(2, result);
        }
    }
}