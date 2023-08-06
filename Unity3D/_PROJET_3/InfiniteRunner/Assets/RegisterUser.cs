using SQLite;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;


[RequireComponent(typeof(PasswordEncrypter))]
public class RegisterUser : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TextMeshProUGUI errorUsernameAlreadyExist;
    [SerializeField] TextMeshProUGUI errorUsernameWrongFormat;
    [SerializeField] TextMeshProUGUI errorWrongPasswordFormat;
    [SerializeField] TextMeshProUGUI validationRegister;

    private PasswordEncrypter passwordEncrypter;
 

    void Start()
    {
        passwordEncrypter = GetComponent<PasswordEncrypter>();
        ResetMessages();
    }
    public void TryToRegisterUser()
    {
        ResetMessages();

        string dataPath = Application.streamingAssetsPath + "/Database";
        SQLiteConnection db = new SQLiteConnection(dataPath + "/database.db");

        if (CheckUsernameFormat(usernameInput.text))
        {
            if (!CheckIfPlayerExist(db, usernameInput.text))
            {
                if (CheckPasswordFormat(passwordInput.text))
                {
                    string userSalt = passwordEncrypter.CreateSalt();
                    string pwd = passwordEncrypter.HashPassword(passwordInput.text, userSalt);
                    RegisterNewPlayer(db, usernameInput.text, pwd, userSalt);
                }

                else
                {
                    errorWrongPasswordFormat.gameObject.SetActive(true);
                }

            }
            else
            {
                errorUsernameAlreadyExist.gameObject.SetActive(true);
                Debug.LogError("Ce joueur existe déjà !");
            }

        }
        else
        {
            errorUsernameWrongFormat.gameObject.SetActive(true);
        }


    }

    bool CheckIfPlayerExist(SQLiteConnection db, string p_user)
    {
        List<DBUsers> obj = db.Query<DBUsers>("SELECT * FROM Users");
        foreach (DBUsers item in obj)
        {
         if (p_user.Equals(item.Username))
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

    bool CheckPasswordFormat(string pswd)
    {
        Regex numericalCaseRegex = new Regex(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?!.* ).{8,}$");
        Regex specialCharCaseRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,}$"); 
        return (numericalCaseRegex.IsMatch(pswd) || specialCharCaseRegex.IsMatch(pswd));
    }

    void RegisterNewPlayer(SQLiteConnection db, string username, string password, string salt)
    {
        DBUsers newPlayer = new DBUsers
        {
            Username = username,
            Password = password,
            Salt = salt
        };

        db.Insert(newPlayer);
        validationRegister.gameObject.SetActive(true);
    }

    void ResetMessages()
    {
        errorUsernameAlreadyExist.gameObject.SetActive(false);
        errorWrongPasswordFormat.gameObject.SetActive(false);
        errorUsernameWrongFormat.gameObject.SetActive(false);
        validationRegister.gameObject.SetActive(false);
    }
}
