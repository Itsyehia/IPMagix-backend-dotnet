using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Azure.AI.OpenAI;
using Azure;

public class SearchService : ISearchService
{
    private readonly SearchClient _searchClient;
    private readonly OpenAIClient _openAi;
    private readonly string _deployment;
    
    public SearchService(IConfiguration config)
    {
        var endpoint = new Uri(config["Azure:SearchEndpoint"]);
        var key = new AzureKeyCredential(config["Azure:SearchKey"]);
        _searchClient = new SearchClient(endpoint, config.GetValue<string>("Azure:SearchIndexName"), key);
        _openAi = new OpenAIClient(new Uri(config["Azure:OpenAIAzureEndpoint"]), new AzureKeyCredential(config["Azure:OpenAIKey"]));
        _deployment = config["Azure:OpenAIDeployment"];
    }
    
   public async Task<SearchResult> SearchDepartmentDocumentsAsync(int departmentId, string query, IEnumerable<int> documentIds, int compoundId)
{
    var options = new SearchOptions { Size = 3 };
    if (documentIds?.Any() == true)
        options.Filter = string.Join(" or ", documentIds.Select(id => $"id eq '{id}'"));

    var resp = await _searchClient.SearchAsync<SearchDocument>(query, options);
    var contents = resp.Value.GetResults().Select(r => r.Document.GetString("content")).Where(c => !string.IsNullOrEmpty(c));
    var context = string.Join("\n\n", contents);

    var chatOptions = new ChatCompletionsOptions
    {
        DeploymentName = _deployment, 
        Messages =
        {
            new ChatRequestSystemMessage("You are an AI assistant that helps people find information."),
            new ChatRequestUserMessage($"Information:\n{context}\nQuestion: {query}")
        },
        MaxTokens = 800
    };

    var completion = await _openAi.GetChatCompletionsAsync(chatOptions); 

    var answer = completion.Value.Choices.First().Message.Content;
    return new SearchResult { Answer = answer, Sources = null };
}

} 