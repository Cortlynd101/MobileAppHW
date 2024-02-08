using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Drawing;
namespace NotesLib.Data;

[Table("User")]
public class User
{
    [PrimaryKey]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime LastSync { get; set; }

    [OneToMany(CascadeOperations = CascadeOperation.All)]
    public List<Note> Notes { get; set; }

    public override bool Equals(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            User u = (User)obj;
            return (Id == u.Id) && (Name == u.Name) && (LastSync == u.LastSync);
        }
    }

}
