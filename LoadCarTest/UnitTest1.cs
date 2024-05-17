using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;
using WpfApp2;
namespace LoadCarTest
{
    [TestClass]
    public class UnitTest1 : PageTest
    {
        [TestMethod]
        public void LoadCarData_LoadsDataSuccessfully()
        {
            // Arrange
            var adminWindow = new AdminWindow();

            // Act
            adminWindow.LoadCarData(1); // Assuming you have data at index 1 for testing

            // Assert
            // Verify the expected values based on your test data
            Assert.AreEqual("ExpectedCarBrand", adminWindow.carBrandLabel.Content);
            Assert.AreEqual("ExpectedCarName", adminWindow.carNameLabel.Content);
            Assert.AreEqual("ExpectedCarPrice", adminWindow.carPriceLabel.Content);
            // Add more assertions based on your data
        }

        [TestMethod]
        public void LoadCarData_HandlesException()
        {
            // Arrange
            var adminWindow = new AdminWindow();

            // Act
            // Simulate an exception by passing an invalid index
            adminWindow.LoadCarData(-1);

            // Assert
            // Verify that an exception is handled, you can check for a specific MessageBox content or log messages
            // Assert.AreEqual("ExpectedErrorMessage", someAssertion);
        }
    }
}
