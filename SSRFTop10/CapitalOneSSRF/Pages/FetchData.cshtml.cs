using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class FetchDataModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    public string? ApiResponse { get; set; }
    public string? TargetUrl { get; set; }

    public FetchDataModel(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public async Task OnGetAsync(string url)
    {
        if (string.IsNullOrEmpty(url)) return;
        TargetUrl = url;

        try {
            var client = _httpClientFactory.CreateClient();
            
            // SSRF TRIGGER: The server makes a request to a URL provided by the USER.
            // In Azure, we must add the 'Metadata: true' header to talk to IMDS.
            // A truly vulnerable app might allow header injection or have this hardcoded.
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Metadata", "true"); 

            var response = await client.SendAsync(request);
            ApiResponse = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex) {
            ApiResponse = "Error fetching data: " + ex.Message;
        }
    }
}