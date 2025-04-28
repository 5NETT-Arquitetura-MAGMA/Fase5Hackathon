using HealthMed.Migrator.Data.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthMed.Migrator.Data.Entities
{
    [Table("Users")]
    public class User : Model<Guid>
    {
        public User()
        {
            Id = Guid.NewGuid();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string SecurityHash { get; set; }

        [Required]
        public UserType Type { get; set; }

        public string? Specialty { get; set; }

        #region Navigation

        public virtual MedicalConsultation? DoctorConsultationStatus { get; set; }
        public virtual MedicalConsultation? PatientConsultationStatus { get; set; }
        public virtual List<DoctorSchedule>? DoctorSchedules { get; set; }

        #endregion Navigation
    }
}