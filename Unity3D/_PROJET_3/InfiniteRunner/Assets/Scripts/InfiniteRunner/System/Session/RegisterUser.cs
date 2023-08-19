using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Events;

public class RegisterUser : MonoBehaviour
{

    private SQLiteSessionRegisterQuery dbRegister;

    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TextMeshProUGUI errorUsernameAlreadyExist;
    [SerializeField] private TextMeshProUGUI errorUsernameWrongFormat;
    [SerializeField] private TextMeshProUGUI errorWrongPasswordFormat;
    [SerializeField] private TextMeshProUGUI validationRegister;

    [SerializeField] UnityEvent OnRegisterDone;


    void Start()
    {
        dbRegister = ServiceLocator.Instance.GetService<ISessionService>().RegisterQuery;
        ResetMessages();
    }

    public void TryToRegisterUser()
    {
        ResetMessages();

        string username = usernameInput.text;
        string password = passwordInput.text;

        if (!CheckUsernameFormat(username))
        {
            errorUsernameWrongFormat.gameObject.SetActive(true);
            return;
        }

        if (dbRegister.CheckIfPlayerExist(username))
        {
            errorUsernameAlreadyExist.gameObject.SetActive(true);
            Debug.LogError("Ce joueur existe déjà !");
            return;
        }

        if (!CheckPasswordFormat(password))
        {
            errorWrongPasswordFormat.gameObject.SetActive(true);
            return;
        }

        dbRegister.RegisterNewPlayer(username, password);
        validationRegister.gameObject.SetActive(true);
        OnRegisterDone.Invoke();
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
