using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectUser : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TextMeshProUGUI errorUnknownIdentifiant;
    [SerializeField] TextMeshProUGUI validationConnection;
    [SerializeField] ApplicationManager.Scene SceneToLoadOnConnect;

    private SQLiteSessionConnexionQuery dbQuery;

    void Start()
    {
        dbQuery = ServiceLocator.Instance.GetService<ISessionService>().ConnexionQuery;
        ResetMessages();
    }

    public void TryToConnectUser()
    {
        ResetMessages();

        if (CheckUsernameFormat(usernameInput.text))
        {
            Debug.Log("Query : " + dbQuery);
            if (dbQuery.CheckIfPlayerExist(usernameInput.text))
            {

                if (!dbQuery.TryConnectUser(usernameInput.text, passwordInput.text))
                {
                    errorUnknownIdentifiant.gameObject.SetActive(true);
                }
                else
                {
                    validationConnection.gameObject.SetActive(true);
                    ServiceLocator.Instance.GetService<ApplicationManager>().LoadScene(SceneToLoadOnConnect);
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



}
