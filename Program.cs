using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repo + Health + CORS
builder.Services.AddSingleton<IMoviesRepository, InMemoryMoviesRepository>();
builder.Services.AddHealthChecks();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

app.MapGet("/", () => Results.Text("Movies API – Mini POC"));
app.MapHealthChecks("/health");

var group = app.MapGroup("/api/movies");

// Liste + søk
group.MapGet("/", (IMoviesRepository repo, string? q) =>
{
    var items = repo.GetAll();
    if (!string.IsNullOrWhiteSpace(q))
    {
        var s = q.Trim().ToLowerInvariant();
        items = items.Where(m => m.Title.ToLowerInvariant().Contains(s) ||
                                 (m.Genres?.Any(g => g.ToLowerInvariant().Contains(s)) ?? false));
    }
    return Results.Ok(items);
});

// Hent én
group.MapGet("/{id:guid}", (IMoviesRepository repo, Guid id) =>
    repo.TryGet(id, out var m) ? Results.Ok(m) : Results.NotFound());

// Opprett
group.MapPost("/", (IMoviesRepository repo, MovieDto input) =>
{
    var validationError = Validate(input);
    if (validationError != null) return Results.BadRequest(new { error = validationError });

    var movie = new Movie
    {
        Id = Guid.NewGuid(),
        Title = input.Title.Trim(),
        Year = input.Year,
        Genres = input.Genres?.ToList() ?? new List<string>(),
        Rating = input.Rating
    };
    repo.Add(movie);
    return Results.Created($"/api/movies/{movie.Id}", movie);
});

// Oppdater
group.MapPut("/{id:guid}", (IMoviesRepository repo, Guid id, MovieDto input) =>
{
    var validationError = Validate(input);
    if (validationError != null) return Results.BadRequest(new { error = validationError });

    if (!repo.TryGet(id, out var existing)) return Results.NotFound();

    existing.Title = input.Title.Trim();
    existing.Year = input.Year;
    existing.Genres = input.Genres?.ToList() ?? new List<string>();
    existing.Rating = input.Rating;

    repo.Update(existing);
    return Results.NoContent();
});

// Slett
group.MapDelete("/{id:guid}", (IMoviesRepository repo, Guid id) =>
    repo.Delete(id) ? Results.NoContent() : Results.NotFound());

// Seed‑data
Seed(app);

app.Run();

// --- Hjelp/Modeller/Repo ---
static string? Validate(MovieDto m)
{
    if (string.IsNullOrWhiteSpace(m.Title)) return "Title is required.";
    if (m.Year is < 1900 or > 2100) return "Year must be between 1900 and 2100.";
    if (m.Rating is < 0 or > 10) return "Rating must be between 0 and 10.";
    return null;
}

static void Seed(WebApplication app)
{
    var repo = app.Services.GetRequiredService<IMoviesRepository>();
    var seed = new[]
    {
        new Movie { Id = Guid.NewGuid(), Title = "The Matrix", Year = 1999, Genres = new List<string>{"Sci-Fi","Action"}, Rating = 8.7 },
        new Movie { Id = Guid.NewGuid(), Title = "Interstellar", Year = 2014, Genres = new List<string>{"Sci-Fi","Drama"}, Rating = 8.6 },
        new Movie { Id = Guid.NewGuid(), Title = "Inception", Year = 2010, Genres = new List<string>{"Sci-Fi","Thriller"}, Rating = 8.8 }
    };
    foreach (var m in seed) repo.Add(m);
}

record Movie
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public int Year { get; set; }
    public List<string>? Genres { get; set; } = new();
    public double Rating { get; set; }
}

record MovieDto(string Title, int Year, IEnumerable<string>? Genres, double Rating);

interface IMoviesRepository
{
    IEnumerable<Movie> GetAll();
    bool TryGet(Guid id, out Movie? movie);
    void Add(Movie movie);
    void Update(Movie movie);
    bool Delete(Guid id);
}

class InMemoryMoviesRepository : IMoviesRepository
{
    private readonly Dictionary<Guid, Movie> _store = new();

    public IEnumerable<Movie> GetAll() => _store.Values.OrderBy(m => m.Title);
    public bool TryGet(Guid id, out Movie? movie) => _store.TryGetValue(id, out movie);
    public void Add(Movie movie) => _store[movie.Id] = movie;
    public void Update(Movie movie) => _store[movie.Id] = movie;
    public bool Delete(Guid id) => _store.Remove(id);
}
