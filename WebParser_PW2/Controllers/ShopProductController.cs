using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebParser_PW2.Data;

namespace WebParser_PW2.Controllers
{
    public class ShopProductController : Controller
    {
        private readonly PricesOfProductContext _context;

        public ShopProductController(PricesOfProductContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> FindPrice()
        {
            using var httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(10)};

            var response = await httpClient.GetAsync("https://shop.grohe.ru/tehnicheskie-aksessuary/aksessuary-dlja-kuhni/termobutylka-grohe-grohe-red-superstal.html");
            var responseData = await response.Content.ReadAsStringAsync();

            if(response.StatusCode == HttpStatusCode.OK)
            {
                var FindBlock = "\"price\":\"";
                var StartPos = responseData.IndexOf(FindBlock);
                var PriceAllText = responseData.Substring(StartPos + FindBlock.Length);

                var PriceText = PriceAllText.Substring(0, PriceAllText.IndexOf('"'));
                var Price = Convert.ToInt32(PriceText);
            }

            return Ok();
        }

        // GET: ShopProduct
        public async Task<IActionResult> Index()
        {
            var pricesOfProductContext = _context.ShopProducts.Include(s => s.Product).Include(s => s.Shop);
            return View(await pricesOfProductContext.ToListAsync());
        }

        // GET: ShopProduct/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShopProducts == null)
            {
                return NotFound();
            }

            var shopProduct = await _context.ShopProducts
                .Include(s => s.Product)
                .Include(s => s.Shop)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (shopProduct == null)
            {
                return NotFound();
            }

            return View(shopProduct);
        }

        // GET: ShopProduct/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "Name");
            return View();
        }

        // POST: ShopProduct/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ShopId,Link,Price")] ShopProduct shopProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", shopProduct.ProductId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "Name", shopProduct.ShopId);
            return View(shopProduct);
        }

        // GET: ShopProduct/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShopProducts == null)
            {
                return NotFound();
            }

            var shopProduct = await _context.ShopProducts.FindAsync(id);
            if (shopProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", shopProduct.ProductId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "ShopId", shopProduct.ShopId);
            return View(shopProduct);
        }

        // POST: ShopProduct/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ShopId,Link,Price")] ShopProduct shopProduct)
        {
            if (id != shopProduct.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopProductExists(shopProduct.ProductId))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId", shopProduct.ProductId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "ShopId", shopProduct.ShopId);
            return View(shopProduct);
        }

        // GET: ShopProduct/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShopProducts == null)
            {
                return NotFound();
            }

            var shopProduct = await _context.ShopProducts
                .Include(s => s.Product)
                .Include(s => s.Shop)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (shopProduct == null)
            {
                return NotFound();
            }

            return View(shopProduct);
        }

        // POST: ShopProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShopProducts == null)
            {
                return Problem("Entity set 'PricesOfProductContext.ShopProducts'  is null.");
            }
            var shopProduct = await _context.ShopProducts.FindAsync(id);
            if (shopProduct != null)
            {
                _context.ShopProducts.Remove(shopProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopProductExists(int id)
        {
          return _context.ShopProducts.Any(e => e.ProductId == id);
        }
    }
}
