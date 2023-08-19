using SQLite;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class RegisterUser : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TextMeshProUGUI errorUsernameAlreadyExist;
    [SerializeField] TextMeshProUGUI errorUsernameWrongFormat;
    [SerializeField] TextMeshProUGUI errorWrongPasswordFormat;
    [SerializeField] TextMeshProUGUI validationRegister;

    [SerializeField] UnityEvent OnRegisterDone;

    private SQLiteSessionRegisterQuery dbRegister;

    public RegisterUser()
    {
        dbRegister = ServiceLocator.Instance.GetService<ISessionService>().RegisterQuery;
    }

    void Start()
    {
        ResetMessages();
    }

    public void TryToRegisterUser()
    {
        ResetMessages();

        if (CheckUsernameFormat(usernameInput.text))
        {
            if (!dbRegister.CheckIfPlayerExist(usernameInput.text))
            {
                if (CheckPasswordFormat(passwordInput.text))
                {

                    dbRegister.RegisterNewPlayer(usernameInput.text, passwordInput.text);
                    validationRegister.gameObject.SetActive(true);
                    OnRegisterDone.Invoke();
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


    void ResetMessages()
    {
        errorUsernameAlreadyExist.gameObject.SetActive(false);
        errorWrongPasswordFormat.gameObject.SetActive(false);
        errorUsernameWrongFormat.gameObject.SetActive(false);
        validationRegister.gameObject.SetActive(false);
    }
}
