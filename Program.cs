// Program.cs
using Microsoft.Data.SqlClient;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.AI.OpenAI;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Configuration.AddEnvironmentVariables();

// Services
builder.Services.AddSingleton<DBConnectionManager>();
builder.Services.AddScoped<ICompoundRepository, CompoundRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IDepartmentDocumentRepository, DepartmentDocumentRepository>();
builder.Services.AddScoped<ISearchService, SearchService>();

builder.Services.AddControllers();

builder.Services.AddCors(options => options.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();