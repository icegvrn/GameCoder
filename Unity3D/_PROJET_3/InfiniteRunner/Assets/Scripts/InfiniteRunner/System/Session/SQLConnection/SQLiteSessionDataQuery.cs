using System;
using System.Collections.Generic;
using UnityEngine;

public class SQLiteSessionDataQuery : SQLiteSessionQuery
{

    public SQLiteSessionDataQuery(ISessionService sessionManager) : base(sessionManager) { }


    #region USER_SESSION_DATA
    public DBUserData GetUserSessionData(string username, string providedToken)
    {
        List<DBUserData> userIdData = db.Query<DBUserData>("SELECT Users.id, Users.username FROM Users WHERE Users.username = ?", username);

        if (userIdData.Count > 0)
        {
            if (JWT.JWTTokenVerify(userIdData[0].Id_user, providedToken))
            {
                List<DBUserData> userData = db.Query<DBUserData>("SELECT Users.id, Users.username, Users.salt FROM Users WHERE Users.id = ?", userIdData[0].Id_user);

                if (userData.Count > 0)
                {
                    return userData[0];
                }
                else
                {
                    Debug.LogError("La session est invalide ou les données utilisateur n'ont pas été trouvées!");
                    return null;
                }
            }
        }
        Debug.LogError("Token d'authentification est incorrect !");
        return null;
    }

    public DBUserData GetUserSessionData()
    {
        return GetUserSessionData(sessionManager.UserData.CurrentUser, sessionManager.UserData.Token);
    }
    #endregion

    #region USER_TIME_AND_PORTALS

    public DBUsers_TimeJoinTime GetAllInformationsForUserByTime(int timeID)
    {
        DBUsers_TimeJoinTime infos = GetCurrentUserBestScoreTimeFromTime(timeID);
        infos.Fragments = GetFragmentsCountForCurrentUserByTime(timeID);
        return infos;
    }
    public List<int> GetAvailableTimeForUser()
    {
        List<int> result = new List<int>();

        List<DBUsers_TimeJoinTime> userData = db.Query<DBUsers_TimeJoinTime>("SELECT users_times.id_user, users_times.id_time, users.id, times.id FROM users_times " +
          "INNER JOIN users ON users.id = users_times.id_user " +
          "INNER JOIN times ON times.id = users_times.id_time " +
          "WHERE users.username = ?", sessionManager.UserData.CurrentUser);

        if (userData.Count > 0)
        {
            foreach (DBUsers_TimeJoinTime item in userData)
            {
                result.Add(item.Id_time);
            }
            return result;
        }

        return null;
    }


    public void RegisterTimeForUser(int timeID)
    {
        try
        {
            db.Execute("INSERT INTO users_times(id_user, id_time) VALUES(?, ?)", sessionManager.UserData.CurrentUserId, timeID);
        }
        catch
        {
            Debug.LogError("Register time for user : Une erreur s'est produite dans la base de données.");
        }
    }

    public bool UnlockNewTimeIfAvailable()
    {
        // Checker tous les temps
        List<DBFragment> allUserTimes = db.Query<DBFragment>("SELECT id_time FROM users_times WHERE id_user = ? GROUP BY id_time", sessionManager.UserData.CurrentUserId);

        // Checker tous les temps supérieur où égal à 2
        List<DBFragment> timeWithMoreThanTwoFragmentForUser = db.Query<DBFragment>("SELECT id_time FROM fragments WHERE id_user = ? GROUP BY id_time HAVING COUNT(*) >= 2;", sessionManager.UserData.CurrentUserId);

        // Comparer le nombre. 
        if (timeWithMoreThanTwoFragmentForUser.Count == allUserTimes.Count)
        {
            return RegisterARandomAvailableTimeForUser();
        }
        return false;
    }

    public bool RegisterARandomAvailableTimeForUser()
    {
        List<DBTimes> result = db.Query<DBTimes>("SELECT times.id FROM times INNER JOIN users_times ON users_times.id_time = times.id WHERE id_time NOT IN (SELECT id_time FROM users_times WHERE id_user = ?) ORDER BY RANDOM() LIMIT 1;", sessionManager.UserData.CurrentUserId);

        if (result.Count > 0 && result[0].Id != 0)
        {
            RegisterTimeForUser(result[0].Id);
            return true;
        }
        return false;
    }
    #endregion

    #region USER_FRAGMENTS 
    
    public int GetFragmentsCountForCurrentUserByTime(int timeID)
    {
        List<DBUsers_TimeJoinTime> fragments = db.Query<DBUsers_TimeJoinTime>(
     "SELECT fragments.id_user FROM fragments " +
     "INNER JOIN users_times ON users_times.id_user = fragments.id_user  AND users_times.id_time = fragments.id_time " +
     "INNER JOIN users ON users.id = fragments.id_user AND users.id = users_times.id_user " +
     "INNER JOIN times ON times.id = fragments.id_time AND times.id = users_times.id_time  " +
     "WHERE fragments.id_user = ? AND fragments.id_time = ?",
     sessionManager.UserData.CurrentUserId, timeID);

        int nb = 0;

        if (fragments.Count > 0)
        {
            foreach (DBUsers_TimeJoinTime frag in fragments)
            {
                nb++;
            }
        }
        return nb;
    }


    public void InsertNewFragmentForPlayer(int timeID)
    {
        int date = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Debug.Log("TENTATIVE ID JOUEUR : " + sessionManager.UserData.CurrentUserId + " date " + date + "time id " + timeID);
        try
        {
            db.Execute("UPDATE fragments SET id_user = ?, date = ? WHERE id IN (SELECT id FROM fragments WHERE id_user IS NULL AND id_time = ? ORDER BY RANDOM() LIMIT 1)", sessionManager.UserData.CurrentUserId, date, timeID);
        }

        catch
        {
            Debug.LogError("Register time for user : Une erreur s'est produite dans la base de données.");
        }
    }


    public bool AreFragmentsAvailable(int timeID)
    {
        List<DBFragment> results = db.Query<DBFragment>("SELECT * FROM fragments WHERE id_user IS NULL AND id_time = ? ORDER BY RANDOM() LIMIT 1", timeID);
        if (results.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AreFragmentsAvailableOverInt(int timeID, int nb)
    {
        List<DBFragment> results = db.Query<DBFragment>("SELECT * FROM fragments WHERE id_user IS NULL AND id_time = ? ORDER BY RANDOM() LIMIT ?", timeID, nb + 1);
        if (results.Count > nb)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region USER_SCORE_TIME
    public void RegisterNewBestScoreTimeForUser(int time, int timeID)
    {
        try
        {
            db.Execute("UPDATE users_times SET best_time = ? WHERE id_user = ? AND id_time = ?", time, sessionManager.UserData.CurrentUserId, timeID);
        }
        catch
        {
            Debug.LogError("Register besttime for user : Une erreur s'est produite dans la base de données.");
        }
    }

    public void RegisterNewBestScoreTimeForUserFromNull(int time, int timeID)
    {
        try
        {
            db.Execute("INSERT INTO users_times(best_time, id_user, id_time) VALUES(?,?,?)", time, sessionManager.UserData.CurrentUserId, timeID);
        }
        catch
        {
            Debug.LogError("Register besttime for user : Une erreur s'est produite dans la base de données.");
        }
    }

    public DBUsers_TimeJoinTime GetCurrentUserBestScoreTimeFromTime(int timeID)
    {
        DBUsers_TimeJoinTime infos = new DBUsers_TimeJoinTime();

        List<DBUsers_TimeJoinTime> userData = db.Query<DBUsers_TimeJoinTime>(
         "SELECT users_times.id_user, users_times.id_time, users_times.best_time, users.id, times.id, times.name  " +
         "FROM users_times " +
         "INNER JOIN users ON users.id = users_times.id_user " + "INNER JOIN times ON times.id = users_times.id_time "
         +
         "  WHERE users.id = ? AND times.id = ?", sessionManager.UserData.CurrentUserId, timeID);


        if (userData.Count > 0)
        {
            infos.best_time = userData[0].best_time;
            infos.Id_time = userData[0].Id_time;
            infos.Id_user = userData[0].Id_user;
            infos.Time_Name = userData[0].Time_Name;
            return infos;
        }

        Debug.LogError("DB_GET_BEST_TIME : Pas d'informations Besttime trouvés ! ");
        return null;
    }
    #endregion

    #region ALL_USERS_QUERIES
    public List<DBRank> GetAllUsersRanking()
    {

        List<DBRank> allRank = db.Query<DBRank>("SELECT users.username, COUNT(fragments.id) AS total FROM users LEFT JOIN fragments ON fragments.id_user = users.id  GROUP BY users.username ORDER BY total DESC");
        return allRank;

    }

    public List<DBFragment> GetAllUsersFragmentsByCount(int count)
    {
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time LIMIT ?", count);
        return allFragments;
    }

    public List<DBFragment> GetAllUsersFragmentsAtTimeByCount(int time, int count)
    {
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_time = ? ORDER BY fragments.date DESC", time, count);
        return allFragments;
    }

    public List<DBFragment> GetAllUsersRandomFragmentsByCount(int count)
    {
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id IN (SELECT id FROM fragments ORDER BY RANDOM()) ORDER BY RANDOM()  LIMIT ?", count);
        return allFragments;
    }

    public List<DBFragment> GetAllUsersRandomFragmentsAtTimeByCount(int time, int count)
    {
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_time = ? AND fragments.id IN (SELECT id FROM fragments ORDER BY RANDOM()) ORDER BY RANDOM() LIMIT ?", time, count);
        return allFragments;
    }


    public List<DBFragment> GetAllCurrentUserFragments()
    {
        int userID = sessionManager.UserData.CurrentUserId;
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.title, fragments.content, fragments.date, users.username, times.name, times.id FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_user = ? ", userID);
        return allFragments;
    }

    public List<DBFragment> GetAllCurrentUserRandomFragments()
    {
        int userID = sessionManager.UserData.CurrentUserId;
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_user = ? AND fragments.id IN (SELECT id FROM fragments ORDER BY RANDOM()) ORDER BY RANDOM()", userID);
        return allFragments;
    }

    public List<DBFragment> GetAllCurrentUserRandomFragmentsByCount(int count)
    {
        int userID = sessionManager.UserData.CurrentUserId;
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_user = ? AND fragments.id IN (SELECT id FROM fragments ORDER BY RANDOM()) ORDER BY RANDOM() LIMIT ?", userID, count);
        return allFragments;
    }

    public List<DBFragment> GetAllCurrentUserFragmentsAtTime(int time)
    {
        int userID = sessionManager.UserData.CurrentUserId;
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_user = ? AND fragments.id_time = ? ORDER BY fragments.date DESC", userID, time);
        return allFragments;
    }

    public List<DBFragment> GetAllCurrentUserRandomFragmentsAtTime(int time)
    {
        int userID = sessionManager.UserData.CurrentUserId;
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_user = ? AND fragments.id_time = ? AND fragments.id IN (SELECT id FROM fragments ORDER BY RANDOM()) ORDER BY RANDOM()", userID, time);
        return allFragments;
    }

    public List<DBFragment> GetAllCurrentUsersFragmentsAtTimeByCount(int time, int count)
    {
        int userID = sessionManager.UserData.CurrentUserId;
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_user = ? AND fragments.id_time = ? AND fragments.id IN (SELECT id FROM fragments ORDER BY RANDOM()) ORDER BY RANDOM() LIMIT ?", userID, time, count);
        return allFragments;
    }

    public int[] GetAllFoundFragmentsForTime(int timeID)
    {
        int[] fragments = new int[2];

        List<DBFragment> totalFragments = db.Query<DBFragment>("SELECT * FROM fragments WHERE id_time = ?", timeID);

        List<DBFragment> foundFragments = db.Query<DBFragment>("SELECT * FROM fragments WHERE id_time = ? AND id_user IS NOT NULL", timeID);

        fragments[0] = foundFragments.Count;
        fragments[1] = totalFragments.Count;

        return fragments;
    }

    #endregion

}

