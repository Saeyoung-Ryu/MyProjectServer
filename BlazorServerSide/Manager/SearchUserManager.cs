using Type;
using RiotSharp;
using RiotSharp.Misc;
using Enum;
using RitoApiExample;

namespace Manager;

public class SearchUserManager
{
    public static async Task<Dictionary<string, Dictionary<WinLoseType, int>>> GetWinRateEachTeamMateAsync(string apiKey, string puuid)
    {
        int count = 0;
        
        var matchList = await MatchManager.GetMatchIdListAsync(apiKey, puuid, 100, QueueType.Flex);
        
        var winRateInfoDic = new Dictionary<string, Dictionary<WinLoseType, int>>(); // key : SummonerName, Key : WinLoseType, Value : Count
        
        foreach (var matchId in matchList)
        {
            var matchInfo = await MatchManager.GetMatchAsync(apiKey, matchId);
            // await Task.Delay(1000);
            Console.WriteLine($"{++count} / {matchList.Count}");

            var myTeamParticipantDtos = matchInfo.Info.GetMyTeamParticipantDtos(puuid);

            foreach (var mtpd in myTeamParticipantDtos)
            {
                if(mtpd.Puuid == puuid)
                    continue;
                
                var isAdded = winRateInfoDic.TryAdd(mtpd.SummonerName, new Dictionary<WinLoseType, int>());
                if (isAdded)
                {
                    winRateInfoDic[mtpd.SummonerName].Add(WinLoseType.Win, 0);
                    winRateInfoDic[mtpd.SummonerName].Add(WinLoseType.Lose, 0);
                }

                var winLoseType = mtpd.Win ? WinLoseType.Win : WinLoseType.Lose;
                winRateInfoDic[mtpd.SummonerName][winLoseType]++;
            }
        }

        var dicOrderByGamePlayCount = winRateInfoDic.OrderByDescending(e => e.Value[WinLoseType.Win] + e.Value[WinLoseType.Lose]).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var dicOrderByWinRate = winRateInfoDic.OrderByDescending(e => e.Value[WinLoseType.Win] != 0 ? e.Value[WinLoseType.Win] / e.Value[WinLoseType.Win] + e.Value[WinLoseType.Lose] : 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        
        return dicOrderByWinRate;
    }

    public static async Task<(Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<WinLoseType, int>>>> EachTeamMateChampWRDic, Dictionary<string, Dictionary<WinLoseType, int>> EachTeamMateWRDic)> GetWinRateEachTeamMateChampionAsync(string apiKey, string puuid, int gameCount)
    {
        int count = 0;
        
        var matchList = await MatchManager.GetMatchIdListAsync(apiKey, puuid, gameCount, QueueType.Flex);
        
        var winRateInfoDic = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<WinLoseType, int>>>>(); // key : SummonerName, Key : ChampName, Key : WinLoseType, Value : Count
        var winRateInfoDic2 = new Dictionary<string, Dictionary<WinLoseType, int>>(); // key : SummonerName, Key : WinLoseType, Value : Count
        
        foreach (var matchId in matchList)
        {
            var matchInfo = await MatchManager.GetMatchAsync(apiKey, matchId);
            // await Task.Delay(1000);
            Console.WriteLine($"{++count} / {matchList.Count}");

            var myTeamParticipantDtos = matchInfo.Info.GetMyTeamParticipantDtos(puuid);

            // var teamSummonerName = myTeamParticipantDtos.First(e => e.Puuid == puuid).SummonerName; // 내 이름
            var myChampName = myTeamParticipantDtos.First(e => e.Puuid == puuid).ChampionName; // 나의 챔피언 이름

            foreach (var mtpd in myTeamParticipantDtos)
            {
                if(mtpd.Puuid == puuid)
                    continue;
                
                winRateInfoDic.TryAdd(mtpd.SummonerName, new Dictionary<string, Dictionary<string, Dictionary<WinLoseType, int>>>());
                winRateInfoDic[mtpd.SummonerName].TryAdd(myChampName, new Dictionary<string, Dictionary<WinLoseType, int>>());
                var isChampAdded = winRateInfoDic[mtpd.SummonerName][myChampName].TryAdd(mtpd.ChampionName, new Dictionary<WinLoseType, int>());

                if (isChampAdded)
                {
                    winRateInfoDic[mtpd.SummonerName][myChampName][mtpd.ChampionName].Add(WinLoseType.Win, 0);
                    winRateInfoDic[mtpd.SummonerName][myChampName][mtpd.ChampionName].Add(WinLoseType.Lose, 0);
                }

                var winLoseType = mtpd.Win ? WinLoseType.Win : WinLoseType.Lose;
                winRateInfoDic[mtpd.SummonerName][myChampName][mtpd.ChampionName][winLoseType]++;

                if (winRateInfoDic2.Keys.Contains(mtpd.SummonerName))
                {
                    winRateInfoDic2[mtpd.SummonerName][winLoseType]++;
                }
                else
                {
                    var dic = new Dictionary<WinLoseType, int>();
                    dic.Add(WinLoseType.Win, 0);
                    dic.Add(WinLoseType.Lose, 0);
                    winRateInfoDic2.Add(mtpd.SummonerName, dic);
                    winRateInfoDic2[mtpd.SummonerName][winLoseType]++;
                }
                
            }
        }

        return (winRateInfoDic, winRateInfoDic2);
    }

    public static async Task<(string NickName, Dictionary<WinLoseType, int> WinLoseDictionary)> GetSoloRankBattleWinRateAsync(string apiKey, string puuid, int listGameCount, DateTime battleStartTime) // battleStartTime 은 시작버튼을 누른 시간! 데이터베이스에 따로 저장해두기
    {
        var matchIdList = await MatchManager.GetMatchIdListAsync(apiKey, puuid, 100, QueueType.Solo); // 100개 최대로 고정시키기
        
        Queue<MatchDto> matchDtoQueue = new Queue<MatchDto>();
        string nickName = String.Empty;

        foreach (var matchId in matchIdList)
        {
            var matchInfo = await MatchManager.GetMatchAsync(apiKey, matchId);

            if (matchInfo.Info.StartTime >= battleStartTime)
            {
                matchDtoQueue.Enqueue(matchInfo);

                if (matchDtoQueue.Count > listGameCount)
                    matchDtoQueue.Dequeue();
            }
        }

        Dictionary<WinLoseType, int> dictionary = new Dictionary<WinLoseType, int>();
        
        foreach (var matchDto in matchDtoQueue)
        {
            var myParticipantDto = matchDto.Info.GetMyParticipantDto(puuid);
            nickName = myParticipantDto.SummonerName;

            if (dictionary.Keys.Count == 0)
            {
                dictionary.Add(WinLoseType.Win, 0);
                dictionary.Add(WinLoseType.Lose, 0);
            }
            
            if (myParticipantDto.Win)
                dictionary[WinLoseType.Win]++;
            else
                dictionary[WinLoseType.Lose]++;
        }

        return (nickName, dictionary);
    }
    /*public static async Task<Dictionary<string, double>> GetWinnRateFor5vs5Async(string nickName, int gameCount)
    {
        var dictionary = new Dictionary<string, double>();
        
        
        
    }*/
}