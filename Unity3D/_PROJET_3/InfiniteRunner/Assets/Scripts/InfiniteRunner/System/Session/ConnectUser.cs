using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

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
        Cursor.visible = true;
        ResetMessages();
    }
    public void TryToConnectUser()
    {
        ResetMessages();

        string username = usernameInput.text;
        string password = passwordInput.text;

        if (!CheckUsernameFormat(username) || !dbQuery.TryConnectUser(username, password))
        {
            errorUnknownIdentifiant.gameObject.SetActive(true);
            return;
        }

        validationConnection.gameObject.SetActive(true);
        ServiceLocator.Instance.GetService<ApplicationManager>().LoadScene(SceneToLoadOnConnect);
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
