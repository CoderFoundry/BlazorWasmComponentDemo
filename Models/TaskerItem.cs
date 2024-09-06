using System.ComponentModel.DataAnnotations;

namespace BlazorWasmComponentDemo.Models
{
    public class TaskerItem
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Every task must have a name")]
        public string? Name { get; set; }

        public bool IsComplete { get; set; } = false;
    }
}
