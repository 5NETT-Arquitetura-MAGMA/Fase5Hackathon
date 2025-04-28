using System.ComponentModel.DataAnnotations.Schema;

namespace HealthMed.Migrator.Data.Entities
{
    [Table("DoctorSchedules")]
    public class DoctorSchedule : Model<Guid>
    {
        public DoctorSchedule()
        {
            Id = Guid.NewGuid();
        }

        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        [ForeignKey("Doctor")]
        public Guid DoctorId { get; set; }

        #region Navigation

        public virtual User Doctor { get; set; }

        #endregion Navigation
    }
}