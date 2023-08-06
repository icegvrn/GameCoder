using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PasswordEncrypter))]
public class ConnectUser : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TextMeshProUGUI errorUnknownIdentifiant;
    [SerializeField] TextMeshProUGUI validationConnection;
    [SerializeField] int SceneToLoadOnConnect;

    private PasswordEncrypter passwordEncrypter;
    private SQLiteConnection db;
    // Start is called before the first frame update
    void Start()
    {
        passwordEncrypter = GetComponent<PasswordEncrypter>(); 
        ResetMessages();
    }

    public void TryToConnectUser()
    {
        db = ServiceLocator.Instance.GetService<SessionManager>().DB;
        ResetMessages();

        if (CheckUsernameFormat(usernameInput.text))
        {
            if (CheckIfPlayerExist(db, usernameInput.text))
            {

                if (!TryConnectUser(db, usernameInput.text, passwordInput.text))
                {
                    errorUnknownIdentifiant.gameObject.SetActive(true);
                }
                else
                {
                    SceneManager.LoadScene(SceneToLoadOnConnect);
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
        List<DBUsers> obj = db.Query<DBUsers>("SELECT salt FROM Users WHERE username == ?", p_username);
      
        foreach (DBUsers item in obj)
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
            ServiceLocator.Instance.GetService<SessionManager>().NewSession(db,username);
            Debug.Log("VOUS ETES CONNECTE EN TANT QUE " + username + " !");
           ServiceLocator.Instance.GetService<SessionManager>().GetUserSessionData(db, ServiceLocator.Instance.GetService<SessionManager>().Token);
            return true;
        }
        return false;
    }

    bool CheckPasswordMatch(SQLiteConnection db, string p_username, string password)
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

  

}
