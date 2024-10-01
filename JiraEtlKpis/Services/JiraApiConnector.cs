using System.Net.Http.Headers;
using ETL.Interfaces;
using Interfaces;

namespace Services;

public class JiraApiConnector : IJiraConnector
{
    private const int DEFAULT_BLOCK_SIZE = 100;
    public int BlockSize { get; set; } = DEFAULT_BLOCK_SIZE;
    public IJiraArguments? JiraArguments { get; set; }

    public string GetIssuesSince(DateTime dateTime)
    {
        string uriString = OnUriNullGetEmptyString();

        using (HttpClient client = new HttpClient())
        {
            client.BaseAddress = new Uri(uriString);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JiraArguments?.Token);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string dateString = dateTime.ToString("yyyy-MM-dd");
            string queryString = $"jql=created >= \"{dateString}\""; //&maxResults={BlockSize}";

            HttpResponseMessage response = client.GetAsync($"/rest/api/2/search?{queryString}").Result;

            string responseContent = response.Content.ReadAsStringAsync().Result;

            return responseContent;
        }
    }

    private string OnUriNullGetEmptyString() => JiraArguments?.Url ?? "";
}
