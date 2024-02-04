using Microsoft.AspNetCore.Mvc;

public class SessionInfoViewComponent : ViewComponent {
    public AppDbContext Context { get; private set; }

    public SessionInfoViewComponent(AppDbContext context) {
        Context = context;
    }
    
    public async Task<IViewComponentResult> InvokeAsync() {
        return await Task.FromResult((IViewComponentResult) View("SessionInfo", new SessionInfo() { Context = Context }));
    }

    // public IViewComponentResult Invoke(int id) {
    //     System.Console.WriteLine("Page User Id: " + id);
    //     return View("SessionInfo", new SessionInfo() { UserId = id, Context = Context });
    // }
}