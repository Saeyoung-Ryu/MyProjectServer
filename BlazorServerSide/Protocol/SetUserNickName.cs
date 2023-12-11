using Type;

namespace Protocol;

public class SetUserNickNameRes
{
    
}

public class SetUserNickNameReq
{
    public string NickName { get; set; }
    public int Seq { get; set; }
}