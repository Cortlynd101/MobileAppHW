using SQLite;
using SQLiteNetExtensions.Attributes;

namespace NotesLib.Data;

[Table("Note")]
public class Note
{
    [PrimaryKey]
    public Guid  Id{ get; set; }

    [ForeignKey(typeof(User))]
    public int UserId { get; set; }
    public string Text { get; set; }
    public DateTime LastSync { get; set; }

    [ManyToOne]      // Many to one relationship with Stock
    public User User { get; set; }
}
