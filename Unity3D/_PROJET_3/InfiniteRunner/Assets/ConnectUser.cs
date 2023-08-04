using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(PasswordEncrypter))]
public class ConnectUser : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TextMeshProUGUI errorUnknownIdentifiant;
    [SerializeField] TextMeshProUGUI validationConnection;

    private PasswordEncrypter passwordEncrypter;

    // Start is called before the first frame update
    void Start()
    {
        passwordEncrypter = GetComponent<PasswordEncrypter>();
        ResetMessages();
    }

    public void TryToConnectUser()
    {

        ResetMessages();

        string dataPath = Application.streamingAssetsPath + "/Database";
        SQLiteConnection db = new SQLiteConnection(dataPath + "/database.db");

        if (CheckUsernameFormat(usernameInput.text))
        {
            if (CheckIfPlayerExist(db, usernameInput.text))
            {

                if (!TryConnectUser(db, usernameInput.text, passwordInput.text))
                {
                    errorUnknownIdentifiant.gameObject.SetActive(true);
                }

            }
            else
            {
                errorUnknownIdentifiant.gameObject.SetActive(true);
            }

        }
        else
        {
            errorUnknownIdentifiant.gameObject.SetActive(true);
        }


    }

    bool CheckIfPlayerExist(SQLiteConnection db, string p_user)
    {
        List<DBPlayer> obj = db.Query<DBPlayer>("SELECT * FROM Player");
        foreach (DBPlayer item in obj)
        {
            if (p_user.Trim().Equals(item.Username))
            {
                return true;
            }
        }
        return false;
    }

    bool CheckUsernameFormat(string user)
    {
        Regex validationRegex = new Regex(@"^[a-zA-Z0-9]+$");
        return (validationRegex.IsMatch(user));
    }


    void ResetMessages()
    {
        errorUnknownIdentifiant.gameObject.SetActive(false);
        validationConnection.gameObject.SetActive(false);
    }

   

    string GetUserSalt(SQLiteConnection db, string p_username)
    {
        List<DBPlayer> obj = db.Query<DBPlayer>("SELECT salt FROM Player WHERE username == ?", p_username);
      
        foreach (DBPlayer item in obj)
        {
            return item.Salt;
        }

        errorUnknownIdentifiant.gameObject.SetActive(true);
        return null;
    }

    bool TryConnectUser(SQLiteConnection db, string username, string pswd)
    {
        string userSalt = GetUserSalt(db, username);
        string Hpassword = passwordEncrypter.HashPassword(pswd, userSalt);
        if (CheckPasswordMatch(db, username, Hpassword))
        {
            validationConnection.gameObject.SetActive(true);
            Debug.Log("VOUS ETES CONNECTE EN TANT QUE " + username + " !");
            return true;
        }
        return false;
    }

    bool CheckPasswordMatch(SQLiteConnection db, string p_username, string password)
    {
        List<DBPlayer> obj = db.Query<DBPlayer>("SELECT password FROM Player WHERE username == ?", p_username);
        foreach (DBPlayer item in obj)
        {
            if (item.Password == password)
            {
                return true;
            }
        }
        return false;
    }
}
