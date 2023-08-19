
using SQLite;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SQLiteSessionQuery
{
    protected ISessionService sessionManager;
    protected SQLiteConnection db;
    protected JWTTokenGenerator JWT;

    public SQLiteSessionQuery(ISessionService sessionManager)
    {
        this.sessionManager = sessionManager;
        db = sessionManager.DB;
        JWT = sessionManager.JWT;
    }

}

