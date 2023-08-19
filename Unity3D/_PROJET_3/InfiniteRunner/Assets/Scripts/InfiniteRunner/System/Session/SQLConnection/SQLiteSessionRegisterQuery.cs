
using System.Collections.Generic;

public class SQLiteSessionRegisterQuery : SQLiteSessionQuery
{
    private PasswordEncrypter passwordEncrypter;
    public SQLiteSessionRegisterQuery(ISessionService sessionManager) : base(sessionManager) 
    {
        passwordEncrypter = new PasswordEncrypter();
    }

   public bool CheckIfPlayerExist(string p_user)
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

   public void RegisterNewPlayer(string username, string password)
    { 
        string userSalt = passwordEncrypter.CreateSalt();
        string pwd = passwordEncrypter.HashPassword(password, userSalt);
        db.Query<DBUsers>("INSERT INTO users(username, password, salt) VALUES(?, ?, ?)", username, pwd, userSalt);
    }

}
