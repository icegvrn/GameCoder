using SQLite;


[Table("fragments")]
public class DBFragment
{
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("content")]
    public string Content { get; set; }

    [Column("rarety")]
    public int Rarety { get; set; }

    [Column("date")]
    public int Date { get; set; }

    [Column("id_user")]
    public int Id_user { get; set; }

    [Column("id_time")]
    public int Id_time { get; set; }

    [Column("username")]
    public string username { get; set; }

    [Column("name")]
    public string timeName { get; set; }

    [Column("idtime")]
    public int TimeId { get; set; }
}