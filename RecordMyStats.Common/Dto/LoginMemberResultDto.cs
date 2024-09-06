namespace RecordMyStats.Common.Dto
{
    public class LoginMemberResultDto
    {
        public string? SessionKey { get; set; }
        public string? FullName { get; set; }
        public string? Errors { get; set; }
        public bool Result { get; set; }
        public string? Token { get; set; }
    }
}
