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

#### Option 1: Using appsettings.json (Not Recommended for Production)
Copy the template and fill in your values:
```bash
cp appsettings.template.json appsettings.local.json
```

Edit `appsettings.local.json` with your actual configuration values:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:YOUR-SQL-SERVER-NAME.database.windows.net,1433;Database=YOUR-DATABASE-NAME;User ID=YOUR-SQL-USERNAME;Password=YOUR-SQL-PASSWORD;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;"
  },
  "Azure": {
    "OpenAIAzureEndpoint": "https://YOUR-OPENAI-RESOURCE-NAME.openai.azure.com/",
    "OpenAIKey": "YOUR-AZURE-OPENAI-API-KEY",
    "OpenAIDeployment": "YOUR-OPENAI-DEPLOYMENT-NAME",
    "SearchEndpoint": "https://YOUR-SEARCH-SERVICE-NAME.search.windows.net",
    "SearchKey": "YOUR-AZURE-SEARCH-API-KEY",
    "SearchIndexName": "YOUR-SEARCH-INDEX-NAME",
    "BlobStorageConnection": "DefaultEndpointsProtocol=https;AccountName=YOUR-STORAGE-ACCOUNT-NAME;AccountKey=YOUR-STORAGE-ACCOUNT-KEY;EndpointSuffix=core.windows.net",
    "ContainerName": "YOUR-BLOB-CONTAINER-NAME"
  }
}
```

#### Option 2: Using Environment Variables (Recommended)
Set the following environment variables:

```bash
# Database Configuration
ConnectionStrings__DefaultConnection="Server=tcp:your-server.database.windows.net,1433;Database=your-db;User ID=your-user;Password=your-password;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;"

# Azure OpenAI Configuration
Azure__OpenAIAzureEndpoint="https://your-openai-resource.openai.azure.com/"
Azure__OpenAIKey="your-openai-api-key"
Azure__OpenAIDeployment="your-deployment-name"

# Azure Search Configuration
Azure__SearchEndpoint="https://your-search-service.search.windows.net"
Azure__SearchKey="your-search-admin-key"
Azure__SearchIndexName="your-search-index-name"

# Azure Blob Storage Configuration
Azure__BlobStorageConnection="your-blob-storage-connection-string"
Azure__ContainerName="your-container-name"
```

#### Configuration Values Explanation:
- **YOUR-SQL-SERVER-NAME**: Your Azure SQL Server name (without .database.windows.net)
- **YOUR-DATABASE-NAME**: Your database name
- **YOUR-SQL-USERNAME**: SQL Server username
- **YOUR-SQL-PASSWORD**: SQL Server password
- **YOUR-OPENAI-RESOURCE-NAME**: Your Azure OpenAI resource name
- **YOUR-AZURE-OPENAI-API-KEY**: API key from Azure OpenAI resource
- **YOUR-OPENAI-DEPLOYMENT-NAME**: Name of your GPT model deployment
- **YOUR-SEARCH-SERVICE-NAME**: Your Azure Cognitive Search service name
- **YOUR-AZURE-SEARCH-API-KEY**: Admin key from Azure Cognitive Search
- **YOUR-SEARCH-INDEX-NAME**: Name of your search index
- **YOUR-STORAGE-ACCOUNT-NAME**: Azure Storage account name
- **YOUR-STORAGE-ACCOUNT-KEY**: Azure Storage account key
- **YOUR-BLOB-CONTAINER-NAME**: Blob container name for documents


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



---

