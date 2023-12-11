using Type;

namespace Protocol;

public class GetUserWinRateHistoryRes
{
    public List<UserWinRateHistory> UserWinRateHistory { get; set; }
}

public class GetUserWinRateHistoryReq
{
    public int Seq { get; set; }
}