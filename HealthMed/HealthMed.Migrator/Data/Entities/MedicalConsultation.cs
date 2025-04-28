using HealthMed.Migrator.Data.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthMed.Migrator.Data.Entities
{
    [Table("MedicalConsultations")]
    public class MedicalConsultation : Model<Guid>
    {
        public MedicalConsultation()
        {
            Id = Guid.NewGuid();
        }

        [ForeignKey("Doctor")]
        public Guid DoctorId { get; set; }

        [ForeignKey("Patient")]
        public Guid PatientId { get; set; }

        public DateTime ScheduledDate { get; set; }
        public ConsultationStatus Status { get; set; }
        public string? Justification { get; set; }

        #region Navigation

        public virtual User Doctor { get; set; }
        public virtual User Patient { get; set; }

        #endregion Navigation
    }
}