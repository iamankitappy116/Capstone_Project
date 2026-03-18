using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesDemo.Models;
using System.Collections.Generic;

namespace RazorPagesDemo.Pages
{
    public class IndexModel : PageModel
    {
        public static List<Item> ItemsList = new List<Item>
        {
            new Item { Name = "Item 1" },
            new Item { Name = "Item 2" }
        };

        public void OnGet()
        {
        }
    }
}