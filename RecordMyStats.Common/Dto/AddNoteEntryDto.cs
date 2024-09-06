using RecordMyStats.Common.Entities;

namespace RecordMyStats.Common.Dto
{
    public class AddNoteEntryDto
    {
        public Note? NoteEntry { get; set; }
        public string? SessionKey { get; set; }      
    }
}
