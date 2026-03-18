namespace SolidReportingSystem.Models
{
    public class SalesReport : Report
    {
        public override string GenerateContent()
        {
            return "Sales Report: Total Sales = $10,000";
        }
    }
}