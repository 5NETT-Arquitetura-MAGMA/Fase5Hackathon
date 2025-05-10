using System.ComponentModel.DataAnnotations;

namespace HealthMed.Migrator.Data.Entities
{
    public class Model
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime? UpdateTime { get; set; }
    }

    public class Model<T>
    {
        [Key]
        public T Id { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;
        public DateTime? UpdateTime { get; set; }
    }
}