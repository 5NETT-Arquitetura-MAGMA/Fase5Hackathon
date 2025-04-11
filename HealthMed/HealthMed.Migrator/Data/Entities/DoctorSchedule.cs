namespace HealthMed.Migrator.Data.Entities
{
    public class DoctorSchedule : Model<Guid>
    {
        public DoctorSchedule()
        {
            Id = Guid.NewGuid();
        }

        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public Guid DoctorId { get; set; }

        #region Navigation

        public virtual User Doctor { get; set; }

        #endregion Navigation
    }
}