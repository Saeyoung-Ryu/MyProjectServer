using Type;

namespace Protocol;

public class SetNewUserRes
{
    public bool IsSuccess { get; set; }
}

public class SetNewUserReq
{
    public string NickName { get; set; }
}