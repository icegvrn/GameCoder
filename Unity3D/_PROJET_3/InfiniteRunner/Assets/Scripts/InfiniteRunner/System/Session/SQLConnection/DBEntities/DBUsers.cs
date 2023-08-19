using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;

[Table("Users")]
public class DBUsers
{
    [Column("id")]
    public int Id { get; set; }
    [Column("username")]
    public string Username { get; set; }
    [Column("password")]
    public string Password { get; set; }
    [Column("salt")]
    public string Salt { get; set; }
}
