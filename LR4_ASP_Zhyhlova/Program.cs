using System.Text;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile(@"./books.json").AddJsonFile(@"./profiles.json");
var app = builder.Build();

app.MapGet("/Library", LibraryRequest);
app.MapGet("/Library/Books", BooksRequest);
app.MapGet("/Library/Profile", ProfilesRequest);
app.MapGet("/Library/Profile/{id:range(0,5)?}", ProfilesIdRequest);

async Task LibraryRequest(HttpContext context)
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync("Welcome to the Library!");
}

async Task BooksRequest(HttpContext context, IConfiguration appConfig)
{
    context.Response.ContentType = "text/html; charset=utf-8";
    StringBuilder sb = new StringBuilder("Available books:");
    for (int i = 0; i < 5; i++)
    {
        IConfiguration book = appConfig.GetSection($"books:{i}");
        sb.Append($"<li>{book["title"]} | {book["year"]} | {book["author"]} | {book["genre"]}  | {book["rating"]}</li>");
    }
    await context.Response.WriteAsync(sb.ToString());
}

async Task ProfilesRequest(HttpContext context)
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync("Information about current user" +
        $"<div>Id: Administrator</div>" +
        $"<div>Name: Zhyhlova Kateryna</div>" +
        $"<div>Age: 20 years old</div>");
}

async Task ProfilesIdRequest(HttpContext context, IConfiguration appConfig, int id)
{
    context.Response.ContentType = "text/html; charset=utf-8";
    StringBuilder sb = new StringBuilder("User profile with id: " + $"{id}");
    IConfiguration user = appConfig.GetSection($"profiles:{id}");
    sb.Append($"<div>Id: {user["id"]}</div>");
    sb.Append($"<div>Name: {user["full-name"]}</div>");
    sb.Append($"<div>Age: {user["age"]} y.o.</div>");
    await context.Response.WriteAsync(sb.ToString());
}
app.Run();