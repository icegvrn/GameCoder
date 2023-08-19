using SQLite;

[Table("users_times")]
public class DBUsers_Time
{
    [Column("id_user")]
    public int Id_user { get; set; }
    [Column("id_time")]
    public int Id_time { get; set; }
    [Column("bestime")]
    public int bestime { get; set; }
}