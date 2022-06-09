using Microsoft.VisualStudio.TestTools.UnitTesting;
using UberAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using UberAPI.Repository.IRepository;

namespace UberAPI.Controllers.Tests
{
    [TestClass()]
    public class DriverControllerTests
    {
        private readonly DriverController controller;
        private readonly IDriversRepository _driversInterface;
        public DriverControllerTests()
        {

        }
        [TestMethod()]
        public void GetAllDriversTest()
        {
            var drivers = new DriverController();

            Assert.Fail();
        }
    }
}