using Type;

namespace Protocol;

public class SetTeamRes
{
    public bool IsSuccess { get; set; }
}

public class SetTeamReq
{
    public LogMatchHistory LogMatchHistory { get; set; }
}