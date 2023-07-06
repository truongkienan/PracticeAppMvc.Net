﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PracticeAppMvc.Net.Models;

namespace PracticeAppMvc.Net.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard/Category
        public async Task<IActionResult> Index()
        {
            //var appDbContext = _context.Categories.Include(c => c.ParentCategory);
            //return View(await appDbContext.ToListAsync());
            var qr = (from c in _context.Categories select c)
                .Include(c => c.ParentCategory)
                .Include(c => c.CategoryChildren);

            var categories=(await qr.ToListAsync())
                .Where(c=>c.ParentCategory==null)
                .ToList();

            return View(categories);
        }

        // GET: Dashboard/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        private void CreateSelectItems(List<Category> source, List<Category> des, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("----", level));
            foreach (var category in source)
            {
                //category.Title = prefix + " " + category.Title;
                //des.Add(category);

                des.Add(new Category()
                {
                    Id=category.Id,
                    Title=prefix+" "+category.Title
                });

                if (category.CategoryChildren?.Count>0)
                {
                    CreateSelectItems(category.CategoryChildren.ToList(), des, level+1);
                }
            }
        }
        // GET: Dashboard/Category/Create
        public async Task<IActionResult> Create()
        {
            var qr = (from c in _context.Categories select c)
                        .Include(c => c.ParentCategory)
                        .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync())
                .Where(c => c.ParentCategory == null)
                .ToList();

            categories.Insert(0, new Category()
            {
                Id=-1,
                Title="Không có danh mục cha"
            });

            var items = new List<Category>();
            CreateSelectItems(categories, items, 0);
            var selectList = new SelectList(items, "Id", "Title");
            ViewData["ParentCategoryId"] = selectList;
            //ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }

        // POST: Dashboard/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Slug,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentCategoryId == -1) category.ParentCategoryId = null;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var qr = (from c in _context.Categories select c)
            .Include(c => c.ParentCategory)
            .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync())
                .Where(c => c.ParentCategory == null)
                .ToList();

            categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });

            var items = new List<Category>();
            CreateSelectItems(categories, items, 0);
            var selectList = new SelectList(items, "Id", "Title");
            ViewData["ParentCategoryId"] = selectList;
            //ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Slug", category.ParentCategoryId);
            return View(category);
        }

        // GET: Dashboard/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var qr = (from c in _context.Categories select c)
            .Include(c => c.ParentCategory)
            .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync())
                .Where(c => c.ParentCategory == null)
                .ToList();

            categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });

            var items = new List<Category>();
            CreateSelectItems(categories, items, 0);
            var selectList = new SelectList(items, "Id", "Title");
            ViewData["ParentCategoryId"] = selectList;

            //ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Slug", category.ParentCategoryId);
            return View(category);
        }

        // POST: Dashboard/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Slug,ParentCategoryId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (category.ParentCategoryId==category.Id)
            {
                ModelState.AddModelError(string.Empty, "Phải chọn danh mục cha khác");
            }

            if (ModelState.IsValid && category.ParentCategoryId != category.Id)
            {
                try
                {
                    if (category.ParentCategoryId == -1)
                        category.ParentCategoryId = null;

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var qr = (from c in _context.Categories select c)
            .Include(c => c.ParentCategory)
            .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync())
                .Where(c => c.ParentCategory == null)
                .ToList();

            categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });

            var items = new List<Category>();
            CreateSelectItems(categories, items, 0);
            var selectList = new SelectList(items, "Id", "Title");
            ViewData["ParentCategoryId"] = selectList;
            //ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Slug", category.ParentCategoryId);
            return View(category);
        }

        // GET: Dashboard/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Dashboard/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'AppDbContext.Categories'  is null.");
            }
            //var category = await _context.Categories.FindAsync(id);
            var category = await _context.Categories
                .Include(c => c.CategoryChildren)
                .FirstOrDefaultAsync(c=>c.Id==id);

            if (category==null)
            {
                return NotFound();
            }

            foreach (var cCategory in category.CategoryChildren)
            {
                cCategory.ParentCategoryId = category.ParentCategoryId;
            }

            //if (category != null)
            //{
            //    _context.Categories.Remove(category);
            //}
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
