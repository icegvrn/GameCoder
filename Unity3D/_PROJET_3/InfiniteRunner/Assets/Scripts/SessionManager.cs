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


    public void SetCurrentUser(string username)
    {
        userSession.CurrentUser = username;

    }


    public void EndSession()
    {
        Debug.Log("==SESSION MANAGER : Nettoyage session ==");
        userSession.DeletePreviousSessions();

    }

    public void NewSession(string username, int id)
    {
        string token = GenerateAuthToken(id, username);
        userSession.DeletePreviousSessions();
        userSession.SetDatas(id, username, token);
    }

    /// <summary>
    /// Création d'un token d'authentification à la connexion
    /// </summary>
    /// <returns></returns>
    private string GenerateAuthToken(int id, string username)
    {
       return JWT.GenerateToken(id, username);
    }


    public DBUserData GetUserSessionData()
    {
        Debug.Log("J'essai de lire les données avec " + userSession.Token);
        return query.GetUserSessionData(userSession.CurrentUser, userSession.Token);
    }


}
