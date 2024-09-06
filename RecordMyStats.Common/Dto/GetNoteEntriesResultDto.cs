using RecordMyStats.Common.Entities;

namespace RecordMyStats.Common.Dto
{
    public class GetNoteEntriesResultDto
    {
        public List<Note>? Notes { get; set; }
        public string? Errors { get; set; }
        public bool Result { get; set; }
    }
}
