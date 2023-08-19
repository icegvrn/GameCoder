using SQLite;

    public interface ISessionService
    {
        UserSessionData UserData { get; }
        SQLiteSessionDataQuery Query { get; }
        SQLiteSessionConnexionQuery ConnexionQuery { get; }
        SQLiteSessionRegisterQuery RegisterQuery { get; }
        SQLiteConnection DB { get; }
        JWTTokenGenerator JWT { get; }

        void SetCurrentUser(string username);
        void EndSession();
        void NewSession(string username, int id);
        DBUserData GetUserSessionData();
}

