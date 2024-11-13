using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength=3)]
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
    }
}
