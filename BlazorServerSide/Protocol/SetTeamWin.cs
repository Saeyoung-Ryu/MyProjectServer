using Type;

namespace Protocol;

public class SetTeamWinRes
{
    
}

public class SetTeamWinReq
{
    public LogMatchHistory LogMatchHistory { get; set; }
    public int WinTeam { get; set; }
}