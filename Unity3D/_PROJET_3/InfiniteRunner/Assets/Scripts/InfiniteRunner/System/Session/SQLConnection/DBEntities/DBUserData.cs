using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;

[Table("UserData")]
public class DBUserData
{
    [Column("id")]
    public int Id_user{ get; set; }
    [Column("username")]
    public string Username { get; set; }
    [Column("token")]
    public string Token { get; set; }
    [Column("salt")]
    public string Salt { get; set; }
}

