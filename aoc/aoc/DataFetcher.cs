using aoc22;
using Microsoft.Extensions.Configuration;
using System.Net;


public class DataFetcher
{
    private readonly HttpClient _httpClient;
    public string? Data { get; private set; }

    public DataFetcher(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("DataFetcherClient") ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    //pass in type of the innermost element
    //trim execive whitespace
    //split on the separators and parse to inner type
    public IEnumerable<IList<T>> Parse<T>(string initalSeparator, char[] separators, int skip = 0)
        =>  Data!.Split(initalSeparator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.Trim())
            .Skip(skip)
            .Select(x =>  x.Split(separators)
                .Select(y => (T)Convert.ChangeType(y, typeof(T))).ToList());



    //pass in type of the innermost element
    //trim execive whitespace        
    //split on the separators and parse to type
    public IList<T> Parse<T>(string separator, int skip = 0)
        =>  Data!
            .Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(x => x.Trim())
            .Skip(skip)
            .Select(y => (T)Convert.ChangeType(y, typeof(T)))
            .ToList();
    

    private void StoreData(int year, int day) => File.WriteAllText($"year{year}day{day}.txt", Data);
    private bool TryLoadDataFromFile(int year, int day)
    {
        try
        {
            Data = File.ReadAllText($"year{year}day{day}.txt");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task GetAndStoreData(int year = 0, int day = 0, string part = "")
    {
        day = day == 0 ?  DateTime.Now.Day : day;
        year = year == 0 ? DateTime.Now.Year : year;
        if (TryLoadDataFromFile(year, day)) return;
        var res = await _httpClient.GetAsync(new Uri($"{year}/day/{day}/input{part}", UriKind.Relative));
        Data = await res.Content.ReadAsStringAsync();
        StoreData(year, day);
    }
    

    public void SetTestData(string testData)
    {
        Data = testData.Replace("\r", "");
    }
}

