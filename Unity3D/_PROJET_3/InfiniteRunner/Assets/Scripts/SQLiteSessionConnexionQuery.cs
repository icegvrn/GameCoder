using System;
using System.Collections.Generic;
using SQLite;
using UnityEngine;

public class SQLiteSessionConnexionQuery : SQLiteSessionQuery
{
    private PasswordEncrypter passwordEncrypter;

    public SQLiteSessionConnexionQuery(ISessionService sessionManager) : base(sessionManager) 
    { 
        passwordEncrypter = new PasswordEncrypter();
    }

    public bool CheckIfPlayerExist(string p_user)
    {
        List<DBUsers> obj = db.Query<DBUsers>("SELECT * FROM Users");
        foreach (DBUsers item in obj)
        {
            if (p_user.Trim().Equals(item.Username))
            {
                return true;
            }
        }
        return false;
    }

    public bool TryConnectUser(string username, string pswd)
    {
        string userSalt = GetUserSalt(username);
        string Hpassword = passwordEncrypter.HashPassword(pswd, userSalt);
        if (CheckPasswordMatch(username, Hpassword))
        {
            ServiceLocator.Instance.GetService<ISessionService>().NewSession(username, GetUserIdByName(username));
            Debug.Log("VOUS ETES CONNECTE EN TANT QUE " + username + " !");
            ServiceLocator.Instance.GetService<ISessionService>().GetUserSessionData();
            return true;
        }
        return false;
    }

    string GetUserSalt(string p_username)
    {
        List<DBUsers> obj = db.Query<DBUsers>("SELECT salt FROM Users WHERE username == ?", p_username);

        foreach (DBUsers item in obj)
        {
            return item.Salt;
        }

        return null;
    }


    bool CheckPasswordMatch(string p_username, string password)
    {
        List<DBUsers> obj = db.Query<DBUsers>("SELECT password FROM Users WHERE username == ?", p_username);
        foreach (DBUsers item in obj)
        {
            if (item.Password == password)
            {
                return true;
            }
        }
        return false;
    }

    int GetUserIdByName(string p_username)
    {
        List<DBUsers> obj = db.Query<DBUsers>("SELECT id FROM users WHERE username == ?", p_username);
        return obj[0].Id;

    }

}

