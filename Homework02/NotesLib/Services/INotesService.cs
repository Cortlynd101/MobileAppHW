using NotesLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesLib.Services;

public interface INotesService : IAsyncDisposable
{
    public Task<List<Note>> GetAllNotes();
    public Task<Note> GetNote(Guid Id);
    public Task<Note> PostNote(Note note);
    public Task UpdateNote(Note note);
    public Task DeleteNote(Guid Id);
}
