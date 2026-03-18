using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EFExample;

namespace EFExample.Tests
{
    public class MathsHelperTests
    {
        [Fact]
        public void Sum_ReturnsCorrectResult()
        {
            // Arrange
            var helper = new MathsHelper();
            int a = 5;
            int b = 10;
            // Act
            int result = helper.Sum(a, b);
            // Assert
            Assert.Equal(15, result);
        }
    }
}
