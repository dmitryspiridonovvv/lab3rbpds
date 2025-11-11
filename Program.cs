using FitnessCenterLab3.Data;
using FitnessCenterLab3.Models;
using FitnessCenterLab3.Services;
using FitnessCenterLab3.Session;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Настройка сервисов
// -------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<CachedDataService>();

var app = builder.Build();

// -------------------------
// ⚡ Пересоздание базы при запуске (для загрузки тестовых данных)
// -------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

// -------------------------
// Включаем Session и предварительное кэширование
// -------------------------
app.UseSession();

using (var scope = app.Services.CreateScope())
{
    var cache = scope.ServiceProvider.GetRequiredService<CachedDataService>();
    cache.GetClients();
}

// -------------------------
// Маршрут по умолчанию
// -------------------------
app.MapGet("/", async ctx =>
{
    ctx.Response.Redirect("/info");
});

// -------------------------
// Главная страница /info
// -------------------------
app.MapGet("/info", async ctx =>
{
    var html = """
    <html>
    <head>
        <meta charset='utf-8'>
        <title>ЛР 3 - Fitness Center</title>
        <style>
            body {
                font-family: 'Segoe UI', Arial;
                background: linear-gradient(135deg, #f0f4f8, #d9e6f2);
                margin: 0;
                height: 100vh;
                display: flex;
                justify-content: center;
                align-items: center;
            }
            .card {
                background: white;
                padding: 40px 60px;
                border-radius: 12px;
                box-shadow: 0 6px 20px rgba(0,0,0,0.15);
                max-width: 700px;
                width: 80%;
                text-align: center;
            }
            h2 {
                color: #2c3e50;
                font-size: 28px;
                margin-bottom: 10px;
            }
            p {
                font-size: 18px;
                color: #555;
                margin-bottom: 30px;
            }
            a {
                display: inline-block;
                margin: 10px;
                padding: 10px 18px;
                background-color: #007bff;
                color: white;
                border-radius: 6px;
                text-decoration: none;
                transition: 0.2s;
            }
            a:hover {
                background-color: #0056b3;
                transform: translateY(-2px);
            }
        </style>
    </head>
    <body>
        <div class='card'>
            <h2>Лабораторная работа №3: Fitness Center</h2>
            <p><b>Вариант №22</b><br>Студент: Спиридонов Д.А.</p>
            <a href='/table/clients'>📋 Таблица клиентов</a>
            <a href='/searchform1'>🔍 Поиск (Cookies)</a>
            <a href='/searchform2'>🔍 Поиск (Session)</a>
        </div>
    </body>
    </html>
    """;
    ctx.Response.ContentType = "text/html; charset=utf-8";
    await ctx.Response.WriteAsync(html, Encoding.UTF8);
});

// -------------------------
// Таблица клиентов с пагинацией
// -------------------------
app.MapGet("/table/clients", async ctx =>
{
    var db = ctx.RequestServices.GetRequiredService<ApplicationDbContext>();

    // Определяем текущую страницу
    int page = 1;
    int.TryParse(ctx.Request.Query["page"], out page);
    if (page < 1) page = 1;

    int pageSize = 50;
    int totalClients = db.Clients.Count();
    int totalPages = (int)Math.Ceiling(totalClients / (double)pageSize);

    var clients = db.Clients
        .OrderBy(c => c.ClientID)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    var html = new StringBuilder($@"
    <html><head><meta charset='utf-8'>
    <title>Клиенты</title>
    <style>
        body {{
            font-family: Arial;
            background-color: #f9fafc;
            margin: 0;
            padding: 40px;
        }}
        h2 {{
            color: #2c3e50;
            text-align: center;
        }}
        table {{
            border-collapse: collapse;
            margin: 0 auto;
            width: 80%;
            background: white;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }}
        th, td {{
            border: 1px solid #ccc;
            padding: 8px 12px;
            text-align: left;
        }}
        th {{
            background-color: #f2f2f2;
        }}
        tr:hover {{
            background-color: #f9f9f9;
        }}
        .pagination {{
            margin-top: 20px;
            text-align: center;
        }}
        .pagination a {{
            margin: 0 5px;
            padding: 6px 12px;
            border-radius: 4px;
            background: #007bff;
            color: white;
            text-decoration: none;
        }}
        .pagination a:hover {{
            background: #0056b3;
        }}
        .back {{
            display: block;
            margin-top: 30px;
            text-align: center;
        }}
    </style></head><body>
    <h2>Таблица: clients (страница {page}/{totalPages})</h2>
    <table>
    <tr><th>ID</th><th>Фамилия</th><th>Имя</th><th>Телефон</th><th>Пол</th></tr>");

    foreach (var c in clients)
    {
        html.Append($"<tr><td>{c.ClientID}</td><td>{c.LastName}</td><td>{c.FirstName}</td><td>{c.Phone}</td><td>{c.Gender}</td></tr>");
    }

    html.Append("</table><div class='pagination'>");

    if (page > 1)
        html.Append($"<a href='/table/clients?page={page - 1}'>⬅ Назад</a>");
    if (page < totalPages)
        html.Append($"<a href='/table/clients?page={page + 1}'>Вперёд ➡</a>");

    html.Append("</div><div class='back'><a href='/info'>🏠 Вернуться на главную</a></div></body></html>");

    ctx.Response.ContentType = "text/html; charset=utf-8";
    await ctx.Response.WriteAsync(html.ToString(), Encoding.UTF8);
});

// -------------------------
// Поиск (Cookies)
// -------------------------
app.MapGet("/searchform1", async ctx =>
{
    ctx.Response.ContentType = "text/html; charset=utf-8";
    string lastName = ctx.Request.Cookies["LastName"] ?? "";
    string phone = ctx.Request.Cookies["Phone"] ?? "";
    string gender = ctx.Request.Cookies["Gender"] ?? "";

    string form = $@"
        <html><head><meta charset='utf-8'>
        <style>
            body {{ font-family: Arial; background-color: #f9fafc; margin: 40px; }}
            input {{ margin: 5px; padding: 5px; }}
            button {{ padding: 6px 12px; background-color: #007bff; color: white; border: none; border-radius: 4px; cursor: pointer; }}
            button:hover {{ background-color: #0056b3; }}
        </style></head><body>
        <h2>Поиск клиентов (Cookies)</h2>
        <form method='post' action='/searchform1'>
            Фамилия: <input name='LastName' value='{lastName}' /><br>
            Телефон: <input name='Phone' value='{phone}' /><br>
            Пол (M/F): <input name='Gender' value='{gender}' /><br>
            <button type='submit'>Найти</button>
        </form>
        <a href='/info'>⬅ Назад</a>
        </body></html>";

    await ctx.Response.WriteAsync(form);
});

app.MapPost("/searchform1", async ctx =>
{
    var form = ctx.Request.Form;
    string lastName = form["LastName"];
    string phone = form["Phone"];
    string gender = form["Gender"];

    ctx.Response.Cookies.Append("LastName", lastName);
    ctx.Response.Cookies.Append("Phone", phone);
    ctx.Response.Cookies.Append("Gender", gender);

    var db = ctx.RequestServices.GetRequiredService<ApplicationDbContext>();
    var query = db.Clients.AsQueryable();

    if (!string.IsNullOrEmpty(lastName))
        query = query.Where(c => c.LastName.Contains(lastName));
    if (!string.IsNullOrEmpty(phone))
        query = query.Where(c => c.Phone.Contains(phone));
    if (!string.IsNullOrEmpty(gender))
        query = query.Where(c => c.Gender == gender);

    var results = query.Take(30).ToList();

    var html = new StringBuilder("<h3>Результаты поиска (Cookies):</h3>");
    foreach (var c in results)
        html.Append($"<p>{c.LastName} {c.FirstName} — {c.Phone} ({c.Gender})</p>");

    html.Append("<br><a href='/searchform1'>⬅ Назад</a>");
    ctx.Response.ContentType = "text/html; charset=utf-8";
    await ctx.Response.WriteAsync(html.ToString());
});

// -------------------------
// Поиск (Session)
// -------------------------
app.MapGet("/searchform2", async ctx =>
{
    ctx.Response.ContentType = "text/html; charset=utf-8";
    var state = ctx.Session.GetObject<SearchFormState>("SearchForm") ?? new SearchFormState();

    string form = $@"
        <html><head><meta charset='utf-8'>
        <style>
            body {{ font-family: Arial; background-color: #f9fafc; margin: 40px; }}
            input {{ margin: 5px; padding: 5px; }}
            button {{ padding: 6px 12px; background-color: #007bff; color: white; border: none; border-radius: 4px; cursor: pointer; }}
            button:hover {{ background-color: #0056b3; }}
        </style></head><body>
        <h2>Поиск клиентов (Session)</h2>
        <form method='post' action='/searchform2'>
            Фамилия: <input name='LastName' value='{state.LastName}' /><br>
            Телефон: <input name='Phone' value='{state.Phone}' /><br>
            Пол (M/F): <input name='Gender' value='{state.Gender}' /><br>
            <button type='submit'>Найти</button>
        </form>
        <a href='/info'>⬅ Назад</a>
        </body></html>";

    await ctx.Response.WriteAsync(form);
});

app.MapPost("/searchform2", async ctx =>
{
    var form = ctx.Request.Form;
    var state = new SearchFormState
    {
        LastName = form["LastName"],
        Phone = form["Phone"],
        Gender = form["Gender"]
    };

    ctx.Session.SetObject("SearchForm", state);

    var db = ctx.RequestServices.GetRequiredService<ApplicationDbContext>();
    var query = db.Clients.AsQueryable();

    if (!string.IsNullOrEmpty(state.LastName))
        query = query.Where(c => c.LastName.Contains(state.LastName));
    if (!string.IsNullOrEmpty(state.Phone))
        query = query.Where(c => c.Phone.Contains(state.Phone));
    if (!string.IsNullOrEmpty(state.Gender))
        query = query.Where(c => c.Gender == state.Gender);

    var results = query.Take(30).ToList();

    var html = new StringBuilder("<h3>Результаты поиска (Session):</h3>");
    foreach (var c in results)
        html.Append($"<p>{c.LastName} {c.FirstName} — {c.Phone} ({c.Gender})</p>");

    html.Append("<br><a href='/searchform2'>⬅ Назад</a>");
    ctx.Response.ContentType = "text/html; charset=utf-8";
    await ctx.Response.WriteAsync(html.ToString());
});

app.Run();
