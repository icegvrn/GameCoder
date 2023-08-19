using SQLite;


[Table("users_times_join")]
public class DBUsers_TimeJoinTime
{
    [Column("id_user")]
    public int Id_user { get; set; }
    [Column("id_time")]
    public int Id_time { get; set; }
    [Column("name")]
    public string Time_Name { get; set; }

    [Column("best_time")]
    public int best_time { get; set; }

    [Column("fragments")]
    public int Fragments { get; set; }
}
