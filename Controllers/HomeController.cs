using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrimaWeb.Models;

namespace PrimaWeb.Controllers;

public class HomeController : Controller {
    private readonly ILogger<HomeController> _logger;
    public readonly AppDbContext Context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context) {
        _logger = logger;
        Context = context;
    }
    
    public IActionResult Index(int? id) {
        TempData["user_id"] = id;
        return View();
    }
    
    public IActionResult Privacy() {
        return View();
    }

    [HttpGet]
    public IActionResult Products(int? id) {
        if (id == null) {
            return View(new SessionInfo() { Context = Context });
        } else {
            TempData["user_id"] = id;
            return View(new SessionInfo() { UserId = id, Context = Context });
        }
    }

    [HttpPost]
    public IActionResult AddToCart(Cart cart) {
        var user = Context.User.Where(u => u.UserId == cart.UserId).ElementAt(0);
        var product = Context.Product.Where(p => p.ProductId == cart.ProductId).ElementAt(0);

        cart.User = user;
        cart.Product = product;

        Context.Cart.Add(cart);
        Context.SaveChanges();

        return RedirectToAction("Products", new { id = cart.UserId });
    }

    public IActionResult Cart(int? id) {
        if (id == null) {
            return View(new SessionInfo() { Context = Context });
        } else {
            TempData["user_id"] = id;
            return View(new SessionInfo() { Cart = new Cart{ UserId = (int)id! }, Context = Context });
        }
    }

    [HttpPost]
    public IActionResult Order(Cart cart) {
        var toDelete = Context.Cart.Where(c => c.UserId == cart.UserId && c.ProductId == cart.ProductId);

        foreach (var item in toDelete) {
            Context.Cart.Remove(item);
        }
        Context.SaveChanges();

        return RedirectToAction("Cart", new { id = cart.UserId });
    }

    [HttpGet]
    public IActionResult SignUp() {
        return View();
    }

    [HttpPost]
    public IActionResult Confirm(User u) {
        bool recordAlreadyExists = Context.User.AsNoTracking().Any(entry => entry.Email == u.Email);
        if (!recordAlreadyExists) {
            Context.User.Add(u);
            Context.SaveChanges();

            return  View("Confirm", u);
        }
        
        return View("SignUp");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
