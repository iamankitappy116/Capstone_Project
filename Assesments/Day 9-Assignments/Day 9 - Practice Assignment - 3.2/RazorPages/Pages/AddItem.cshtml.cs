using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesDemo.Models;

namespace RazorPagesDemo.Pages
{
    public class AddItemModel : PageModel
    {
        [BindProperty]
        public Item NewItem { get; set; }

        public IActionResult OnPost()
        {
            IndexModel.ItemsList.Add(NewItem);
            return RedirectToPage("Index");
        }
    }
}