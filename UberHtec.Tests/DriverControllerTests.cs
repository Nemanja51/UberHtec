using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using UberAPI.Controllers;
using UberAPI.Models;
using UberAPI.Repository.IRepository;
using Xunit;

namespace UberHtec.Tests
{
    public class DriverControllerTests
    {
        [Fact]
        public void GetAllDrivers_Returns_1000_Users_With_Role_Driver()
        {
            ////arrange
            //var fakeDrivers = A.CollectionOfDummy<User>(1000).AsEnumerable();
            //var dataStore = A.Fake<IDriversRepository>();
            //A.CallTo(() => dataStore.GetAllDrivers()).ReturnsNextFromSequence();
            //var controller = new DriverController(dataStore);
            ////act
            //var actionResult = controller.GetAllDrivers();
            ////assert
            //var result = 
        }
    }
}
