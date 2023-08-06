using SQLite;
using System.Collections.Generic;
using UnityEngine;


public class DBConnexionTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string dataPath = Application.streamingAssetsPath + "/Database";
        SQLiteConnection db = new SQLiteConnection(dataPath + "/database.db");

      //  RegisterNewPlayer(db, "Dobby", "dobbyestlibre!");

        List<DBUsers> obj = db.Query<DBUsers>("SELECT * FROM Player");
        foreach (DBUsers item in obj)
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
        DBUsers newPlayer = new DBUsers
        {
            Username = username,
            Password = password
        };

        db.Insert(newPlayer);
    }
}
