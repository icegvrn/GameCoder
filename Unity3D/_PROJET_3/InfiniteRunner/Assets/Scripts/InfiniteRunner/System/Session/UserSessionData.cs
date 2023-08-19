using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static DBConstant;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UIElements.UxmlAttributeDescription;


[Table("users_times")]
public class DBUsers_Time
{
    [Column("id_user")]
    public int Id_user { get; set; }
    [Column("id_time")]
    public int Id_time { get; set; }
    [Column("bestime")]
    public int bestime { get; set; }
}

[Table("times")]
public class DBTimes
{
    [Column("id")]
    public int Id { get; set; }
    [Column("description")]
    public int Descriptions { get; set; }
    [Column("name")]
    public string Name { get; set; }
}

[Table("users_times_join")]
public class DBUsers_TimeJoinTime
{
    [Column("id_user")]
    public int Id_user { get; set; }
    [Column("id_time")]
    public int Id_time { get; set; }
    [Column("name")]
    public string Time_Name { get; set; }

    [Column("best_time")]
    public int best_time { get; set; }

    [Column("fragments")]
    public int Fragments { get; set; }
}

[Table("fragments")]
public class DBFragment
{
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("content")]
    public string Content { get; set; }

    [Column("rarety")]
    public int Rarety { get; set; }

    [Column("date")]
    public int Date { get; set; }

    [Column("id_user")]
    public int Id_user { get; set; }

    [Column("id_time")]
    public int Id_time { get; set; }

    [Column("username")]
    public string username { get; set; }

    [Column("name")]
    public string timeName { get; set; }

    [Column("idtime")]
    public int TimeId { get; set; }
}

public class DBRank
{
    [Column("username")]
    public string Username { get; set; }

    [Column("total")]
    public int TotalFragments{ get; set; }
}



public class UserSessionData : MonoBehaviour
{
    private string token;
    private SQLiteConnection db;

    // Start is called before the first frame update
    void Start()
    {
        token = ServiceLocator.Instance.GetService<SessionManager>().Token;
        string dataPath = Application.streamingAssetsPath + "/Database";
        db = new SQLiteConnection(dataPath + "/database.db");
        ServiceLocator.Instance.RegisterService(this);
      
    }

    public List<int> GetAvailableTimeForUser()
    {
        List<int> result = new List<int>();
        // Récupérer les données de l'utilisateur associées à la session
        List<DBUsers_TimeJoinTime> userData = db.Query<DBUsers_TimeJoinTime>(
          "SELECT users_times.id_user, users_times.id_time, users.id, times.id  " +
          "FROM users_times " +
          "INNER JOIN users ON users.id = users_times.id_user " + "INNER JOIN times ON times.id = users_times.id_time "
          +
          "  WHERE users.username = ?", ServiceLocator.Instance.GetService<SessionManager>().CurrentUser);


        if (userData.Count > 0)
        {
            foreach (DBUsers_TimeJoinTime item in userData)
            {
                result.Add(item.Id_time);
            }
            return result;
        }

        Debug.LogError("Aucun portail de temps disponible pour cet utilisateur !");
        return null;
    }

    public void RegisterTimeForUser(int timeID)
    {
        try
        {
            db.Execute("INSERT INTO users_times(id_user, id_time) VALUES(?, ?)", ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId, timeID);
        }
        catch
        {
            Debug.LogError("Register time for user : Une erreur s'est produite dans la base de données.");
        }
    }

    public void RegisterNewBestTimeForUser(int time, int timeID)
    {
        try
        {
            db.Execute("UPDATE users_times SET best_time = ? WHERE id_user = ? AND id_time = ?", time, ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId, timeID);
        }
        catch
        {
            Debug.LogError("Register besttime for user : Une erreur s'est produite dans la base de données.");
        }
    }

    public void RegisterNewBestTimeForUserFromNull(int time, int timeID)
    {
        try
        {
            db.Execute("INSERT INTO users_times(best_time, id_user, id_time) VALUES(?,?,?)", time, ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId, timeID);
        }
        catch
        {
            Debug.LogError("Register besttime for user : Une erreur s'est produite dans la base de données.");
        }
    }

    public DBUsers_TimeJoinTime GetBestTimeFromTime(int timeID)
    {
        DBUsers_TimeJoinTime infos = new DBUsers_TimeJoinTime();

        List<DBUsers_TimeJoinTime> fragments = db.Query<DBUsers_TimeJoinTime>(
     "SELECT fragments.id_user " +
     "FROM fragments " +
     "INNER JOIN users_times ON users_times.id_user = fragments.id_user  AND users_times.id_time = fragments.id_time " +
     "INNER JOIN users ON users.id = fragments.id_user AND users.id = users_times.id_user " +
     "INNER JOIN times ON times.id = fragments.id_time AND times.id = users_times.id_time  " +
     "WHERE fragments.id_user = ? AND fragments.id_time = ?",
     ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId, timeID);

        int nb = 0;
        if (fragments.Count > 0)
        {
            foreach (DBUsers_TimeJoinTime frag in fragments)
            {
                nb++;
            }
        }
        Debug.Log("NOMBRE DE FRAGMENTS TROUVE : " + nb);
        infos.Fragments = nb;



        List<DBUsers_TimeJoinTime> userData = db.Query<DBUsers_TimeJoinTime>(
         "SELECT users_times.id_user, users_times.id_time, users_times.best_time, users.id, times.id, times.name  " +
         "FROM users_times " +
         "INNER JOIN users ON users.id = users_times.id_user " + "INNER JOIN times ON times.id = users_times.id_time "
         +
         "  WHERE users.id = ? AND times.id = ?", ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId, timeID);


        if (userData.Count > 0)
        {
            infos.best_time = userData[0].best_time;
            infos.Id_time = userData[0].Id_time;
            infos.Id_user = userData[0].Id_user;
            infos.Time_Name = userData[0].Time_Name;
            Debug.Log("LE TIME NAME EST " + userData[0].Time_Name);
            return infos;
        }

        Debug.LogError("DB_GET_BEST_TIME : Pas d'informations Besttime trouvés ! ");
        return null;
    }

    public int[] GetAllFoundFragments(int timeID)
    {
        int[] fragments = new int[2];

        List<DBFragment> totalFragments = db.Query<DBFragment>(
     "SELECT *  " +
     "FROM fragments " +
     "  WHERE id_time = ?", timeID);

        List<DBFragment> foundFragments = db.Query<DBFragment>(
"SELECT *  " +
"FROM fragments " +
"  WHERE id_time = ? AND id_user IS NOT NULL", timeID);

        fragments[0] = foundFragments.Count;
        fragments[1] = totalFragments.Count;

        return fragments;
    }

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
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id IN (SELECT id FROM fragments ORDER BY RANDOM() LIMIT ?) ORDER BY RANDOM()", count);
        return allFragments;
    }

    public List<DBFragment> GetAllUsersRandomFragmentsAtTimeByCount(int time, int count)
    {
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_time = ? AND fragments.id IN (SELECT id FROM fragments ORDER BY RANDOM() LIMIT ?) ORDER BY RANDOM()", time, count);
        return allFragments;
    }


    public List<DBFragment> GetAllCurrentUserFragments()
    {
        int userID = ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId;
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.title, fragments.content, fragments.date, users.username, times.name, times.id FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_user = ? ", userID);
        return allFragments;
    }

    public List<DBFragment> GetAllCurrentUserRandomFragments()
    {
        int userID = ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId;
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_user = ? AND fragments.id IN (SELECT id FROM fragments ORDER BY RANDOM()) ORDER BY RANDOM()", userID);
        return allFragments;
    }

    public List<DBFragment> GetAllCurrentUserRandomFragmentsByCount(int count)
    {
        int userID = ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId;
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_user = ? AND fragments.id IN (SELECT id FROM fragments ORDER BY RANDOM() LIMIT ?) ORDER BY RANDOM()", userID, count);
        return allFragments;
    }

    public List<DBFragment> GetAllCurrentUserFragmentsAtTime(int time)
    {
        int userID = ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId;
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_user = ? AND fragments.id_time = ? ORDER BY fragments.date DESC", userID, time);
        return allFragments;
    }

    public List<DBFragment> GetAllCurrentUserRandomFragmentsAtTime(int time)
    {
        int userID = ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId;
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_user = ? AND fragments.id_time = ? AND fragments.id IN (SELECT id FROM fragments ORDER BY RANDOM()) ORDER BY RANDOM()", userID, time);
        return allFragments;
    }

    public List<DBFragment> GetAllCurrentUsersFragmentsAtTimeByCount(int time, int count)
    {
        int userID = ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId;
        List<DBFragment> allFragments = db.Query<DBFragment>("SELECT fragments.id, fragments.title, fragments.content, fragments.rarety, fragments.date, fragments.id_user, fragments.id_time, users.username, times.name, times.id as idtime FROM fragments INNER JOIN users ON users.id = fragments.id_user INNER JOIN times ON times.id = fragments.id_time WHERE fragments.id_user = ? AND fragments.id_time = ? AND fragments.id IN (SELECT id FROM fragments ORDER BY RANDOM() LIMIT ?) ORDER BY RANDOM()", userID, time, count);
        return allFragments;
    }

    public void InsertNewFragmentForPlayer(int timeID)
    {
        int date = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Debug.Log("TENTATIVE ID JOUEUR : " + ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId + " date " + date + "time id " + timeID);
        try
        {
                db.Execute("UPDATE fragments SET id_user = ?, date = ? WHERE id IN (SELECT id FROM fragments WHERE id_user IS NULL AND id_time = ? ORDER BY RANDOM() LIMIT 1)", ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId, date, timeID);
        }

        catch
        {
                Debug.LogError("Register time for user : Une erreur s'est produite dans la base de données.");
        }
    }

    //SELECT * FROM fragments WHERE id_user ISNULL AND id_time = ?

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
        List<DBFragment> results = db.Query<DBFragment>("SELECT * FROM fragments WHERE id_user IS NULL AND id_time = ? ORDER BY RANDOM() LIMIT ?", timeID, nb+1);
        if (results.Count > nb)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool UnlockNewTimeIfAvailable()
    {
        // Checker tous les temps
        List<DBFragment> allUserTimes = db.Query<DBFragment>("SELECT id_time FROM users_times WHERE id_user = ? GROUP BY id_time", ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId);

        // Checker tous les temps supérieur où égal à 2
        List<DBFragment> timeWithMoreThanTwoFragmentForUser = db.Query<DBFragment>("SELECT id_time FROM fragments WHERE id_user = ? GROUP BY id_time HAVING COUNT(*) >= 2;", ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId);

        // Comparer le nombre. 
        if (timeWithMoreThanTwoFragmentForUser.Count == allUserTimes.Count)
        {
            return RegisterARandomAvailableTimeForUser();
        }
        Debug.Log("Pas de nouveau portail pour cet utilisateur");
        return false;
    }

    public bool RegisterARandomAvailableTimeForUser()
    {
        List<DBTimes> result = db.Query<DBTimes>("SELECT times.id FROM times INNER JOIN users_times ON users_times.id_time = times.id WHERE id_time NOT IN (SELECT id_time FROM users_times WHERE id_user = ?) ORDER BY RANDOM() LIMIT 1;", ServiceLocator.Instance.GetService<SessionManager>().CurrentUserId);
        
        if (result.Count > 0 && result[0].Id != 0)
        {
            Debug.Log("Pour cet utilisateur j'enregistre le temps : " + result[0].Id);

            RegisterTimeForUser(result[0].Id);
            return true;
        }
        Debug.Log("Pas de nouveau portail pour cet utilisateur");
        return false;
    }




}
