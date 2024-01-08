using Type;

namespace Protocol;

public class SetUserNickNameRes
{
    public bool IsDuplicatedNickName { get; set; }
}

public class SetUserNickNameReq
{
    public string NickName { get; set; }
    public int Seq { get; set; }
}