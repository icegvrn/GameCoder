using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SQLite;


public class SessionManager : MonoBehaviour
{

    private string currentUser;
    public string CurrentUser { get { return currentUser; } set { currentUser = value; } }

    private int currentUserId;
    public int CurrentUserId { get { return currentUserId; } set { currentUserId = value; } }

    private string token;
    public string Token { get { return token; }  set { token = value; } }
    private SQLiteConnection db;
    public SQLiteConnection DB { get { return db; } }

    public SessionManager()
    {
        if (ServiceLocator.Instance.GetService<SessionManager>() == null)
        {
            ServiceLocator.Instance.RegisterService(this);
            string dataPath = Application.streamingAssetsPath + "/Database";
            db = new SQLiteConnection(dataPath + "/database.db");
        }
    }

    private void Start()
    {
        Debug.Log("==SESSION MANAGER : Initialisation...==");   
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void SetCurrentUser(string username)
    {
        currentUser = username;

    }

    public void UpdateActivityTime()
    {

    }

    public void EndSession()
    {
        Debug.Log("==SESSION MANAGER : Nettoyage session ==");
        DeletePreviousSessions();
        currentUser = null;
        currentUserId = 0;
        token = null;
      
    }

    public void NewSession(SQLiteConnection db, string username, int id)
    {
        DeletePreviousSessions(db, username);
        string token = GenerateAuthToken();
        InsertToken(db, username, token, (int)DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds());
        CurrentUser = username;
        CurrentUserId = id;
        Token = token;
    }

    /// <summary>
    /// Création d'un token d'authentification à la connexion
    /// </summary>
    /// <returns></returns>
    private string GenerateAuthToken()
    {
        byte[] randomBytes = new byte[128];

        using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
        {
            rngCsp.GetBytes(randomBytes);
        }

        return Convert.ToBase64String(randomBytes);
    }

    private void InsertToken(SQLiteConnection db, string p_username, string token, int date)
    {
        Debug.Log("J'insere la session avec " + p_username + " " + token + "et la date " + date);
        int idUser = 0;
        List<DBUsers> obj = db.Query<DBUsers>("SELECT id FROM Users WHERE username == ?", p_username);
        foreach (DBUsers item in obj)
        {
            idUser = item.Id;

        }

        obj = db.Query<DBUsers>("INSERT INTO Sessions(id_user, token, expiration) VALUES(?, ?, ?)", idUser, token, date);
    }


    private void DeletePreviousSessions(SQLiteConnection db, string p_username)
    {
        int idUser = 0;
        List<DBUsers> obj = db.Query<DBUsers>("SELECT id FROM Users WHERE username == ?", p_username);
        foreach (DBUsers item in obj)
        {
            idUser = item.Id;
        }

        obj = db.Query<DBUsers>("DELETE FROM Sessions WHERE id_user == ?", idUser);
    }

    private void DeletePreviousSessions()
    {
        int idUser = 0;
        List <DBUsers> obj = db.Query<DBUsers>("SELECT id, username FROM Users WHERE username == ?", currentUser);
        foreach (DBUsers item in obj)
        {
            idUser = item.Id;
        }
        obj = db.Query<DBUsers>("DELETE FROM Sessions WHERE id_user == ?", idUser);
    }

    

    public DBUserData GetUserSessionData(SQLiteConnection db, string providedToken)
    {
     DeleteExpiredSessions();
        // Récupérer les données de l'utilisateur associées à la session
        List<DBUserData> userData = db.Query<DBUserData>(
            "SELECT Users.id, Users.username, Users.salt, Sessions.token " +
            "FROM Users " +
            "INNER JOIN Sessions ON Users.id = Sessions.id_user " +
            "WHERE Sessions.token = ?", providedToken);

        if (userData.Count > 0)
        {
            return userData[0];
        }

        Debug.LogError("La session est invalide ou les données utilisateur n'ont pas été trouvées!");
        return null;
    }

    public DBUserData GetUserSessionData()
    {
        // Récupérer les données de l'utilisateur associées à la session
        List<DBUserData> userData = db.Query<DBUserData>(
            "SELECT Users.id, Users.username, Users.salt, Sessions.token " +
            "FROM Users " +
            "INNER JOIN Sessions ON Users.id = Sessions.id_user " +
            "WHERE Sessions.token = ?", token);

        if (userData.Count > 0)
        {
            return userData[0];
        }

        Debug.LogError("La session est invalide ou les données utilisateur n'ont pas été trouvées!");
        return null;
    }

    private void DeleteExpiredSessions()
    {
        int currentDate = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        db.Query<DBSessionData>("DELETE FROM Sessions WHERE expiration <= ?", currentDate);
    }

    private void OnApplicationQuit()
    {
        EndSession();
    }


}
