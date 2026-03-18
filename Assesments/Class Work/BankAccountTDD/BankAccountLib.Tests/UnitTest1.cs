using Xunit;
using BankAccountLib;

public class BankAccountTests
{
    [Fact]
    public void Transfer_Should_Move_Money_From_One_Account_To_Another()
    {
        var source = new BankAccount(1000);
        var target = new BankAccount(500);

        source.TransferTo(target, 300);

        Assert.Equal(700, source.Balance);
        Assert.Equal(800, target.Balance);
    }
    [Fact]
    public void Transfer_Should_Throw_When_Insufficient_Funds()
    {
        var source = new BankAccount(100);
        var target = new BankAccount(500);

        Assert.Throws<InvalidOperationException>(() =>
            source.TransferTo(target, 200));
    } 
}