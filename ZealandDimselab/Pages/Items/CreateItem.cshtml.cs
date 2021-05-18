using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZealandDimselab.Models;
using ZealandDimselab.Services;

namespace ZealandDimselab.Pages.Items
{
    public class CreateItemModel : PageModel
    {
        private ItemService itemService;
        private CategoryService categoryService;
        [BindProperty] public Item Item { get; set; }
        public List<Item> Items { get; set; }
        public List<Category> Categories { get; set; }
        [BindProperty] public int CategoryId { get; set; }


        public CreateItemModel(ItemService itemService, CategoryService categoryService)
        {
            this.itemService = itemService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Items = await itemService.GetAllItemsWithCategoriesAsync();
            Categories = categoryService.GetAllCategories();
            CategoryId = 0;

            return Page();
        }

        public async Task<IActionResult> OnGetFilterByCategoryAsync(int category)
        {
            if (category == 0) return RedirectToPage("CreateItem");

            Items = await itemService.GetItemsWithCategoryIdAsync(category);
            Categories = categoryService.GetAllCategories();
            CategoryId = category;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                if (CategoryId == 0) return await OnGetAsync();
                return await OnGetFilterByCategoryAsync(CategoryId);
            }

            await itemService.AddItemAsync(Item);

            if (CategoryId == 0) return RedirectToPage("AllItems");
            if (Item.CategoryId != CategoryId)
                return RedirectToPage("AllItems", "FilterByCategory", new {category = Item.CategoryId});
            return RedirectToPage("AllItems", "FilterByCategory", new { category = CategoryId });
        }
    }
}
