using HealthMed.CommandAPI.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthMed.CommandAPI.Controllers.Dtos.Patient.Input
{
    public class ScheduleConsultationInput
    {
        public Guid DoctorId { get; set; }

        [DataType(DataType.Date)]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime ScheduleDate { get; set; }

        public TimeSpan ScheduleTime { get; set; }
    }
}