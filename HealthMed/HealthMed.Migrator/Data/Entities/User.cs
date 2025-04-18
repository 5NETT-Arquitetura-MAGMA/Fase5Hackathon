using HealthMed.Migrator.Data.Entities.Enum;
using System.ComponentModel.DataAnnotations;

namespace HealthMed.Migrator.Data.Entities
{
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

        public virtual ConsultationStatus? DoctorConsultationStatus { get; set; }
        public virtual ConsultationStatus? PatientConsultationStatus { get; set; }
        public virtual List<DoctorSchedule>? DoctorSchedules { get; set; }

        #endregion Navigation
    }
}