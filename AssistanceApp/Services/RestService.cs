using AssistanceApp.Models;
using System.Text.Json;

public class RestService
{
    private readonly HttpClient _client;
    private const string BaseUrl = "https://tool-creator-childhood-scenarios.trycloudflare.com/api";

    private readonly JsonSerializerOptions _options;

    public RestService()
    {
        _client = new HttpClient();
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<List<School>> GetAllSchoolsAsync()
    {
        try
        {
            var json = await _client.GetStringAsync($"{BaseUrl}/School");

            Console.WriteLine($"DEBUG API RAW: {json}"); // <--- SEE WHAT WE GOT

            var schools = JsonSerializer.Deserialize<List<School>>(json, _options);
            return schools ?? new List<School>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API School Error: {ex.Message}");
            return new List<School>();
        }
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        try
        {
            var json = await _client.GetStringAsync($"{BaseUrl}/Students");
            var students = JsonSerializer.Deserialize<List<Student>>(json, _options);
            return students ?? new List<Student>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API Student Error: {ex.Message}");
            return new List<Student>();
        }
    }
}
