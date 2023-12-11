using Type;

namespace Protocol;

public class SetUserWinRateHistoryRes
{
    
}

public class SetUserWinRateHistoryReq
{
    public List<UserWinRateHistory> UserWinRateHistory { get; set; }
    public string NickName { get; set; }
    public int Seq { get; set; }
}