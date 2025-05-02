using System.ComponentModel.DataAnnotations.Schema;

namespace HealthMed.Migrator.Data.Entities
{
    [Table("DoctorOffDays")]
    public class DoctorOffDays : Model<Guid>
    {
        public DoctorOffDays()
        {
            Id = Guid.NewGuid();
        }

        public DateTime OffDate { get; set; }

        [ForeignKey("Doctor")]
        public Guid DoctorId { get; set; }

        #region Navigation

        public virtual User Doctor { get; set; }

        #endregion Navigation
    }
}