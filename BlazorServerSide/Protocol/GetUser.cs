using Type;

namespace Protocol;

public class GetUserRes
{
    public UserInfo UserInfo { get; set; }
}

public class GetUserReq
{
    public string NickName { get; set; }
}