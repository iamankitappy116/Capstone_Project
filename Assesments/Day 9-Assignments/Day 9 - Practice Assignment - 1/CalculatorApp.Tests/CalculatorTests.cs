using Xunit;
using CalculatorApp;

namespace CalculatorApp.Tests
{
    public class CalculatorTests
    {
        private readonly Calculator _calculator;

        public CalculatorTests()
        {
            _calculator = new Calculator();
        }

        [Fact]
        public void Add_ReturnsCorrectResult()
        {
            var result = _calculator.Add(5, 3);
            Assert.Equal(8, result);
        }

        [Fact]
        public void Subtract_ReturnsCorrectResult()
        {
            var result = _calculator.Subtract(10, 4);
            Assert.Equal(6, result);
        }

        [Fact]
        public void Multiply_ReturnsCorrectResult()
        {
            var result = _calculator.Multiply(3, 5);
            Assert.Equal(15, result);
        }

        [Fact]
        public void Divide_ReturnsCorrectResult()
        {
            var result = _calculator.Divide(10, 2);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Divide_ByZero_ThrowsException()
        {
            Assert.Throws<DivideByZeroException>(() => _calculator.Divide(10, 0));
        }

        [Fact]
        public void Add_WithZero_ReturnsSameNumber()
        {
            var result = _calculator.Add(5, 0);
            Assert.Equal(5, result);
        }
    }
}