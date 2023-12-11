using RiotSharp.Endpoints.MatchEndpoint;

namespace Type;

public class InfoDto
{
    public List<ParticipantDto> Participants { get; set; }
    public long GameCreation { get; set; }
    public long GameEndTimestamp { get; set; }
    
    public DateTime StartTime => DateTimeOffset.FromUnixTimeMilliseconds(GameCreation).DateTime;
    public DateTime EndTime => DateTimeOffset.FromUnixTimeMilliseconds(GameEndTimestamp).DateTime;
    public List<ParticipantDto> GetMyTeamParticipantDtos(string myPuuid)
    {
        var myWinLose = Participants.First(e => e.Puuid == myPuuid).Win;
        return Participants.Where(e => e.Win == myWinLose).ToList();
    }
    
    public ParticipantDto GetMyParticipantDto(string myPuuid)
    {
        return Participants.First(e => e.Puuid == myPuuid);
    }
}