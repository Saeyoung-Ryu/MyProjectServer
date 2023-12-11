using System.Text;
using Newtonsoft.Json;
using RiotSharp.Endpoints.MatchEndpoint;
using Enum;
using Type;

namespace RitoApiExample;

public class MatchManager
{
    public static async Task<List<string>> GetMatchIdListAsync(string api, string puuid, int count = 20, QueueType queueType = QueueType.None)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            List<string> matchInfoList = new List<string>();
            StringBuilder sb = new StringBuilder();
            
            var apiUrl = $"https://asia.api.riotgames.com/lol/match/v5/matches/by-puuid/{puuid}/ids?start=0&count={count}&api_key={api}";
            sb.Append(apiUrl);
            
            if(queueType != QueueType.None)
                sb.Append($"&queue={(int)queueType}");

            HttpResponseMessage response = await httpClient.GetAsync(sb.ToString());

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                matchInfoList = JsonConvert.DeserializeObject<List<string>>(jsonResponse);
            }

            return matchInfoList;
        }
    }

    public static async Task<MatchDto> GetMatchAsync(string api, string matchId)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            MatchDto match = new MatchDto();
            
            var apiUrl = $"https://asia.api.riotgames.com/lol/match/v5/matches/{matchId}?api_key={api}";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                match  = JsonConvert.DeserializeObject<MatchDto>(jsonResponse);
            }

            return match;
        }
        
    }
    
    /*private static List<MatchInfo> ParseMatchInfo(string json)
    {
        List<MatchInfo> matchInfoList = new List<MatchInfo>();

        string[] elements = json.Trim('[', ']').Split(',');

        foreach (string element in elements)
        {
            matchInfoList.Add(new MatchInfo(element));
        }

        return matchInfoList;
    }*/
}