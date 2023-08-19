
public class UserSessionData
{
    private string currentUser;
    public string CurrentUser { get { return currentUser; } set { currentUser = value; } }

    private int currentUserId;
    public int CurrentUserId { get { return currentUserId; } set { currentUserId = value; } }

    private string token;
    public string Token { get { return token; } set { token = value; } }


    public void SetDatas(int p_id, string p_username, string p_token)
    {
        currentUser = p_username;
        currentUserId = p_id;
        token = p_token;
    }

    public void DeletePreviousSessions()
    {
        currentUser = null;
        currentUserId = 0;
        token = null;
    }

}
