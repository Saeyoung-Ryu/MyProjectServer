using Type;

namespace Protocol;

public class ResetRankingRes
{
    public List<RankInfo> TotalRankList = new List<RankInfo>();
 
    public List<RankInfo> AdcRankList = new List<RankInfo>();
    public List<RankInfo> SupRankList = new List<RankInfo>();
    public List<RankInfo> MidRankList = new List<RankInfo>();
    public List<RankInfo> JgRankList = new List<RankInfo>();
    public List<RankInfo> TopRankList = new List<RankInfo>();
}

public class ResetRankingReq
{
    
}