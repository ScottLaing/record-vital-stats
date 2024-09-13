namespace RecordMyStats.Common.Entities
{
    public class StatisticEntry
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public DateTime? CreateDate { get; set; }
        public double? BloodSugar { get; set; }
        public double? Weight { get; set; }
        public int? BPSystolic { get; set; }
        public int? BPDiastolic { get; set; }
        public int? HeartRate { get; set; }
        public string? WeightUnits { get; set; }
        public string? BSUnits { get; set; }

        public string BloodSugarDisplay
        {
            get
            {
                double amt = BloodSugar ?? 0;
                if (amt == 0)
                {
                    return Constants.DisplayStrings.NotAvailable;
                }
                string display = $"{amt} {BSUnits}";
                return display;
            }
        }

        public string WeightDisplay
        {
            get
            {
                double amt = Weight ?? 0;
                if (amt == 0)
                {
                    return Constants.DisplayStrings.NotAvailable;
                }
                string display = $"{amt} {WeightUnits}";
                return display;
            }
        }

        public string CreateDateDisplay
        {
            get
            {
                if (CreateDate == null) 
                {
                    return Constants.DisplayStrings.NotAvailable;
                }
                string display = CreateDate?.ToString("MMMM dd yyyy hh:mm tt") ?? Constants.DisplayStrings.NotAvailable;
                return display;
            }
        }

        public string HeartRateDisplay
        {
            get
            {
                if (HeartRate == null)
                {
                    return Constants.DisplayStrings.NotAvailable;
                }
                string display = $"{HeartRate} bpm";
                return display;
            }
        }

        public string BloodPressureDisplay
        {
            get
            {
                if (BPSystolic == null || BPDiastolic == null)
                {
                    return Constants.DisplayStrings.NotAvailable;
                }
                string display = $"{BPSystolic}/{BPDiastolic} mmHg";
                return display;
            }
        }

        public override string ToString()
        {
            return $"Id: {Id}, MemberId: {MemberId}, CreateDate: {CreateDate}, BloodSugar: {BloodSugar}, Weight: {Weight}, Systolic: {BPSystolic}, Diastolic: {BPDiastolic}";
        }
    }
}
