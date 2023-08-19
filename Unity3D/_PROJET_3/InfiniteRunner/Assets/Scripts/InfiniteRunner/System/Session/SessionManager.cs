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

    JWTTokenGenerator JWT;

    public SessionManager()
    {
        if (ServiceLocator.Instance.GetService<SessionManager>() == null)
        {
            ServiceLocator.Instance.RegisterService(this);
            string dataPath = Application.streamingAssetsPath + "/Database";
            db = new SQLiteConnection(dataPath + "/database.db");
            JWT = new JWTTokenGenerator();
        }
    }

    private void Start()
    {
        Debug.Log("==SESSION MANAGER : Initialisation...==");   
    }

    private void Update()
    {

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
        DeletePreviousSessions();
        string token = GenerateAuthToken(id, username);
        CurrentUser = username;
        CurrentUserId = id;
        Token = token;
    }

    /// <summary>
    /// Cr�ation d'un token d'authentification � la connexion
    /// </summary>
    /// <returns></returns>
    private string GenerateAuthToken(int id, string username)
    {
        JWTTokenGenerator JWT = new JWTTokenGenerator();
       return JWT.GenerateToken(id, username);
    }

    private void DeletePreviousSessions()
    {
        Token = null;
    }


    

    public DBUserData GetUserSessionData(SQLiteConnection db, string username, string providedToken)
    {
        Debug.Log("J'essai de r�cup�rer l'id joueur de " + username);
      
        List<DBUserData> userIdData = db.Query<DBUserData>(
               "SELECT Users.id, Users.username FROM Users WHERE Users.username = ?", username);

        if (userIdData.Count > 0)
        { Debug.Log("J'essai de r�cup�rer les donn�es avec le token " + providedToken + "et l'id user" + userIdData[0].Id_user);
            if (JWT.JWTTokenVerify(userIdData[0].Id_user, providedToken))
            {
               
                // R�cup�rer les donn�es de l'utilisateur associ�es � la session
                List<DBUserData> userData = db.Query<DBUserData>(
                    "SELECT Users.id, Users.username, Users.salt " +
                    "FROM Users WHERE Users.id = ?", userIdData[0].Id_user);

                if (userData.Count > 0)
                {
                    return userData[0];
                }
                else
                {
                    Debug.LogError("La session est invalide ou les donn�es utilisateur n'ont pas �t� trouv�es!");
                    return null;
                }
            }
        }

        Debug.LogError("Votre token d'authentification est incorrect !");
        return null;
    }

    public DBUserData GetUserSessionData()
    {
        Debug.Log("J'essai de lire les donn�es avec " + Token);
        return GetUserSessionData(db, currentUser, Token);
    }


    private void OnApplicationQuit()
    {
        EndSession();
    }


}
