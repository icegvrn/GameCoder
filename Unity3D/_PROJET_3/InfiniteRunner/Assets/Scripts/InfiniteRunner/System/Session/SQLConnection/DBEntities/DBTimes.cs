using SQLite;

[Table("times")]
public class DBTimes
{
    [Column("id")]
    public int Id { get; set; }
    [Column("description")]
    public int Descriptions { get; set; }
    [Column("name")]
    public string Name { get; set; }
}