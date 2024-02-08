using Microsoft.AspNetCore.Mvc;
using NotesAPI.Initializers;
using NotesAPI.Request;
using NotesLib.Data;
using NotesLib.Services;
using SQLite;

namespace NotesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class NotesController(INotesService notesService) : Controller
{
    [HttpGet("/getNotes")]
    public Task<List<Note>> GetNotes()
    {
        return notesService.GetAllNotes();
    }

    [HttpGet("/getNote/{Id}")]
    public Task<Note> GetNote(Guid Id)
    {
        return notesService.GetNote(Id);
    }

    [HttpPost("/postNote")]
    public Task<Note> PostNote([FromBody] AddNoteRequest request)
    {
        var note = new Note { UserId = request.Id, Text = request.NoteBody };
        return notesService.PostNote(note);
    }

    [HttpPatch("/updateNote")]
    public Task UpdateNote([FromBody] Note note)
    {
        return notesService.UpdateNote(note);
    }

    [HttpDelete("/deleteNote/{Id}")]
    public Task UpdateNote(Guid Id)
    {
        return notesService.DeleteNote(Id);
    }
}
