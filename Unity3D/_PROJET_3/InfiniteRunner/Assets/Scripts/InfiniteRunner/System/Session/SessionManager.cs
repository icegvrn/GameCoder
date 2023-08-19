using UnityEngine;
using SQLite;

public class SessionManager : ISessionService
{
    private UserSessionData userSession;
    public UserSessionData UserData { get { return userSession; } }

    private SQLiteSessionDataQuery query;
    public SQLiteSessionDataQuery Query { get { return query; } }

    private SQLiteSessionConnexionQuery connexionQuery;
    public SQLiteSessionConnexionQuery ConnexionQuery { get { return connexionQuery; } }

    private SQLiteSessionRegisterQuery registerQuery;
    public SQLiteSessionRegisterQuery RegisterQuery { get { return registerQuery; } }


    private SQLiteConnection db;
    public SQLiteConnection DB { get { return db; } }

    JWTTokenGenerator jwt;
    public JWTTokenGenerator JWT { get { return jwt; } }    

    public SessionManager()
    {
            string dataPath = Application.streamingAssetsPath + "/Database";
            db = new SQLiteConnection(dataPath + "/database.db");
            
            jwt = new JWTTokenGenerator();
            userSession = new UserSessionData();

            query = new SQLiteSessionDataQuery(this);
            connexionQuery = new SQLiteSessionConnexionQuery(this);
            registerQuery = new SQLiteSessionRegisterQuery(this);
    }

    public void NewSession(string username, int id)
    {
        string token = GenerateAuthToken(id, username);
        userSession.DeletePreviousSessions();
        userSession.SetDatas(id, username, token);
    }

    private string GenerateAuthToken(int id, string username)
    {
        return JWT.GenerateToken(id, username);
    }

    public void EndSession()
    {
        if (userSession != null) { userSession.DeletePreviousSessions(); }
    }

}
