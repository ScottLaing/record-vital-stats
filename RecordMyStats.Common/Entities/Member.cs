namespace RecordMyStats.Common.Entities
{
    public class Member
    {
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public bool IsActive { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Sex { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public DateTime? CreateDate { get; set; }

        public override string ToString()
        {
            return $"FirstName: {FirstName}, LastName: {LastName}, Email: {Email}, DateOfBirth: {DateOfBirth}, Sex: {Sex}, Id: {Id}";
        }
    }
}
