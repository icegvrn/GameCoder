using SQLite;
using System;

[Table("Sessions")]
public class DBSessionData
{
    [Column("id")]
    public int Id { get; set; }
    [Column("id_user")]
    public int Id_user { get; set; }
    [Column("token")]
    public string Token { get; set; }
    [Column("expiration")]
    public DateTime Expiration { get; set; }
}
