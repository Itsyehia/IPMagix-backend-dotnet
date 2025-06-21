# Ipx.Api - Intelligent Document Management System

A modern ASP.NET Core 8.0 Web API for managing compounds, departments, and documents with integrated AI-powered search capabilities using Azure Cognitive Search and OpenAI.

## 🚀 Features

- **Multi-tenant Architecture**: Support for multiple compounds with isolated data
- **Document Management**: Upload, organize, and manage documents across departments
- **AI-Powered Search**: Intelligent document search with natural language queries
- **Department Organization**: Hierarchical structure for organizing documents
- **RESTful API**: Clean, well-documented API endpoints
- **Azure Integration**: Seamless integration with Azure Cognitive Search and OpenAI
- **Containerized Deployment**: Docker support for easy deployment

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server with Dapper ORM
- **Search**: Azure Cognitive Search
- **AI**: Azure OpenAI Service
- **Documentation**: Swagger/OpenAPI
- **Containerization**: Docker

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server (local/Azure)
- Azure Cognitive Search instance
- Azure OpenAI Service
- Docker (optional, for containerization)

## ⚙️ Installation & Setup

### 1. Clone the Repository
```bash
git clone <your-repository-url>
cd Ipx.Api
```

### 2. Install Dependencies
```bash
dotnet restore
```

### 3. Configuration
Update `appsettings.json` with your configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Driver={ODBC Driver 18 for SQL Server};Server=your_server;Database=your_db;UID=your_user;PWD=your_password;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;"
  },
  "Azure": {
    "SearchEndpoint": "https://your-search-service.search.windows.net",
    "SearchKey": "your_search_admin_key",
    "SearchIndexName": "documents-index",
    "OpenAIAzureEndpoint": "https://your-openai-service.openai.azure.com/",
    "OpenAIKey": "your_openai_key",
    "OpenAIDeployment": "gpt-4"
  }
}
```

### 4. Database Setup
Ensure your SQL Server database has the required tables:

```sql
-- Create tables
CREATE TABLE ipx_compounds (
    id INT IDENTITY(1,1) PRIMARY KEY,
    title NVARCHAR(255) NOT NULL
);

CREATE TABLE ipx_departments (
    id INT IDENTITY(1,1) PRIMARY KEY,
    title NVARCHAR(255) NOT NULL,
    compound_id INT NOT NULL,
    FOREIGN KEY (compound_id) REFERENCES ipx_compounds(id)
);

CREATE TABLE ipx_documents (
    id INT IDENTITY(1,1) PRIMARY KEY,
    title NVARCHAR(255) NOT NULL,
    url NVARCHAR(500),
    size BIGINT,
    sha256 NVARCHAR(64),
    status NVARCHAR(50),
    index_name NVARCHAR(255),
    indexer_name NVARCHAR(255),
    compound_id INT NOT NULL,
    FOREIGN KEY (compound_id) REFERENCES ipx_compounds(id)
);

CREATE TABLE ipx_departments_documents (
    department_id INT NOT NULL,
    document_id INT NOT NULL,
    compound_id INT NOT NULL,
    PRIMARY KEY (department_id, document_id),
    FOREIGN KEY (department_id) REFERENCES ipx_departments(id),
    FOREIGN KEY (document_id) REFERENCES ipx_documents(id),
    FOREIGN KEY (compound_id) REFERENCES ipx_compounds(id)
);
```

### 5. Run the Application
```bash
dotnet run
```

The API will be available at `https://localhost:7000` (or the configured port).

## 📚 API Documentation

### Base URL
```
https://localhost:7000/api
```

### Authentication Headers
All endpoints (except compounds) require the following header:
```
X-Compound-ID: {compound_id}
```

### 🏢 Compounds Management

#### Get All Compounds
```http
GET /api/compounds
```

**Response Example:**
```json
[
  {
    "id": 1,
    "title": "Tech Corp Headquarters"
  },
  {
    "id": 2,
    "title": "Manufacturing Plant A"
  }
]
```

#### Create Compound
```http
POST /api/compounds
Content-Type: application/json

{
  "title": "New Company Branch"
}
```

**Response Example:**
```json
{
  "id": 3,
  "title": "New Company Branch"
}
```

### 🏬 Department Management

#### Get Departments by Compound
```http
GET /api/departments
X-Compound-ID: 1
```

**Response Example:**
```json
[
  {
    "id": 1,
    "title": "Human Resources",
    "compoundId": 1
  },
  {
    "id": 2,
    "title": "Information Technology",
    "compoundId": 1
  }
]
```

#### Create Department
```http
POST /api/departments
X-Compound-ID: 1
Content-Type: application/json

{
  "title": "Marketing Department"
}
```

#### Get Department by ID
```http
GET /api/departments/1
X-Compound-ID: 1
```

### 📄 Document Management

#### Get Documents by Compound
```http
GET /api/documents
X-Compound-ID: 1
```

**Response Example:**
```json
[
  {
    "id": 1,
    "title": "Employee Handbook 2024",
    "url": "https://storage/handbook.pdf",
    "size": 2048576,
    "sha256": "abc123...",
    "status": "indexed",
    "indexName": "documents-index",
    "indexerName": "documents-indexer"
  }
]
```

### 🔗 Department-Document Association

#### Get Documents for Department
```http
GET /api/departments/1/documents
X-Compound-ID: 1
```

#### Assign Document to Department
```http
POST /api/departments/1/documents
X-Compound-ID: 1
Content-Type: application/json

{
  "documentId": 5
}
```

### 🔍 AI-Powered Search

#### Search Department Documents
```http
POST /api/departments/1/search
X-Compound-ID: 1
Content-Type: application/json

{
  "query": "What are the vacation policies?",
  "documentIds": [1, 2, 3]
}
```

**Response Example:**
```json
{
  "answer": "Based on the employee handbook, employees are entitled to 15 days of paid vacation per year, with additional days earned based on tenure...",
  "sources": null
}
```

## 🐳 Docker Setup

### Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Ipx.Api.csproj", "."]
RUN dotnet restore "Ipx.Api.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Ipx.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ipx.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ipx.Api.dll"]
```

### Build and Run with Docker

#### Build the Docker Image
```bash
docker build -t ipx-api .
```

#### Run the Container
```bash
docker run -d \
  --name ipx-api-container \
  -p 8080:80 \
  -e ConnectionStrings__DefaultConnection="your_connection_string" \
  -e Azure__SearchEndpoint="your_search_endpoint" \
  -e Azure__SearchKey="your_search_key" \
  -e Azure__SearchIndexName="your_index_name" \
  -e Azure__OpenAIAzureEndpoint="your_openai_endpoint" \
  -e Azure__OpenAIKey="your_openai_key" \
  -e Azure__OpenAIDeployment="your_deployment_name" \
  ipx-api
```

#### Docker Compose (Optional)
Create a `docker-compose.yml`:

```yaml
version: '3.8'
services:
  ipx-api:
    build: .
    ports:
      - "8080:80"
    environment:
      - ConnectionStrings__DefaultConnection=your_connection_string
      - Azure__SearchEndpoint=your_search_endpoint
      - Azure__SearchKey=your_search_key
      - Azure__SearchIndexName=your_index_name
      - Azure__OpenAIAzureEndpoint=your_openai_endpoint
      - Azure__OpenAIKey=your_openai_key
      - Azure__OpenAIDeployment=your_deployment_name
    depends_on:
      - sqlserver

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123!
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql

volumes:
  sqlserver_data:
```

Run with:
```bash
docker-compose up -d
```

## 🧪 Testing

### Manual Testing with cURL

#### 1. Create a Compound
```bash
curl -X POST "https://localhost:7000/api/compounds" \
  -H "Content-Type: application/json" \
  -d '{"title": "Test Company"}'
```

#### 2. Create a Department
```bash
curl -X POST "https://localhost:7000/api/departments" \
  -H "Content-Type: application/json" \
  -H "X-Compound-ID: 1" \
  -d '{"title": "IT Department"}'
```

#### 3. Search Documents
```bash
curl -X POST "https://localhost:7000/api/departments/1/search" \
  -H "Content-Type: application/json" \
  -H "X-Compound-ID: 1" \
  -d '{"query": "server maintenance schedule", "documentIds": [1,2,3]}'
```

### Testing with Postman

Import the following collection:

```json
{
  "info": {
    "name": "Ipx.Api Collection",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "variable": [
    {
      "key": "baseUrl",
      "value": "https://localhost:7000/api"
    },
    {
      "key": "compoundId",
      "value": "1"
    }
  ],
  "item": [
    {
      "name": "Compounds",
      "item": [
        {
          "name": "Get All Compounds",
          "request": {
            "method": "GET",
            "url": "{{baseUrl}}/compounds"
          }
        },
        {
          "name": "Create Compound",
          "request": {
            "method": "POST",
            "url": "{{baseUrl}}/compounds",
            "header": [
              {
                "key": "Content-Type",
                "value": "application/json"
              }
            ],
            "body": {
              "mode": "raw",
              "raw": "{\n  \"title\": \"Sample Compound\"\n}"
            }
          }
        }
      ]
    }
  ]
}
```

## 🚀 Development

### Project Structure
```
Ipx.Api/
├── Controllers/           # API Controllers
├── Models/               # Data Models
├── Repositories/         # Data Access Layer
├── Services/            # Business Logic
├── DTOs/                # Data Transfer Objects
├── Program.cs           # Application Entry Point
├── appsettings.json     # Configuration
└── Dockerfile          # Container Configuration
```

### Adding New Features

1. **Create Model**: Add new model in `Models/`
2. **Create Repository**: Add interface and implementation in `Repositories/`
3. **Register Services**: Update `Program.cs` with DI registration
4. **Create Controller**: Add new controller in `Controllers/`
5. **Update Documentation**: Update this README with new endpoints

## 🔒 Security Considerations

- Use HTTPS in production
- Implement proper authentication/authorization
- Validate input parameters
- Use parameterized queries (already implemented with Dapper)
- Secure Azure service keys
- Implement rate limiting
- Add request/response logging

## 📊 Monitoring & Logging

Consider implementing:
- Application Insights for Azure monitoring
- Structured logging with Serilog
- Health checks endpoint
- Metrics collection
- Error tracking

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Check the API documentation at `/swagger` when running locally

---

**Built with ❤️ using ASP.NET Core 8.0** 