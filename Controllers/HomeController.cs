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
        // List<Product> productList = new() {
        //     new() { Name = "Smartphone XYZ", Description = "Ultimo modello di smartphone", Price = 799.99 },
        //     new() { Name = "Laptop ABC", Description = "Potente laptop per professionisti", Price = 1299.99 },
        //     new() { Name = "Fotocamera123", Description = "Fotocamera digitale ad alta risoluzione", Price = 499.99 },
        //     new() { Name = "Smart TV 4K", Description = "TV con risoluzione Ultra HD", Price = 899.99 },
        //     new() { Name = "Cuffie Bluetooth", Description = "Cuffie wireless con cancellazione del rumore", Price = 149.99 },
        //     new() { Name = "Console Gaming Pro", Description = "Console di gioco ad alte prestazioni", Price = 499.99 },
        //     new() { Name = "Fitness Tracker", Description = "Dispositivo per il monitoraggio dell'attività fisica", Price = 79.99 },
        //     new() { Name = "Aspirapolvere Robot", Description = "Aspirapolvere autonomo con tecnologia avanzata", Price = 349.99 },
        //     new() { Name = "Drone Explorer", Description = "Drone con telecamera per esplorazioni aeree", Price = 599.99 },
        //     new() { Name = "Stampante 3D Pro", Description = "Stampante tridimensionale ad alta precisione", Price = 899.99 },
        // };

        // foreach (var item in productList) {
        //     Context.Product.Add(item);
        // }

        // // Delete
        // var lastN = Context.Product.OrderByDescending(g => g.ProductId).Take(10);
        // Context.Product.RemoveRange(lastN);

        // Context.SaveChanges();
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
        System.Console.WriteLine(cart.UserId + " --- " + cart.ProductId);
        var toDelete = Context.Cart.Where(c => c.UserId == cart.UserId && c.ProductId == cart.ProductId);
        // System.Console.WriteLine(toDelete.ElementAt(0).Product.Name);
        System.Console.WriteLine(toDelete.Count());
        foreach (var item in toDelete) {
            Context.Cart.Remove(item);
        }
        Context.SaveChanges();
        System.Console.WriteLine("aaaaaaaaaaaaa");
        
        // return View("Index");
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
