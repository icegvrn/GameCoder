using SQLite;

public class DBRank
{
    [Column("username")]
    public string Username { get; set; }

    [Column("total")]
    public int TotalFragments { get; set; }
}