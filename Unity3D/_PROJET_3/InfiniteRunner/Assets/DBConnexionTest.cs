using SQLite;
using System.Collections.Generic;
using UnityEngine;

[Table("Player")]
public class DBPlayer
{
    [Column("id")]
    public string Id { get; set; }
    [Column("username")]
    public string Username { get; set; }
    [Column("password")]
    public string Password { get; set; }
    [Column("salt")]
    public string Salt { get; set; }
}
public class DBConnexionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string dataPath = Application.streamingAssetsPath + "/Database";
        SQLiteConnection db = new SQLiteConnection(dataPath + "/database.db");

      //  RegisterNewPlayer(db, "Dobby", "dobbyestlibre!");

        List<DBPlayer> obj = db.Query<DBPlayer>("SELECT * FROM Player");
        foreach (DBPlayer item in obj)
        {
            Debug.Log("J'ai trouvé un joueur "+ item.Username + " qui a l'ID " + item.Id + " et un mot de passe très secret : " + item.Password);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RegisterNewPlayer(SQLiteConnection db, string username, string password)
    {
        DBPlayer newPlayer = new DBPlayer
        {
            Username = username,
            Password = password
        };

        db.Insert(newPlayer);
    }
}
