using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_food_delivery_system.Models
{
    public class Agent
    {
        [Key]
        public int AgentID { get; set; }


        [Required(ErrorMessage = "Please Enter Your Name")]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please Enter Your AgentContact")]
        [MaxLength(15)]
        public string? AgentContact { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email")]
        [EmailAddress]
        public string? Email { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<Delivery>? Deliveries { get; set; } = new List<Delivery>(); 

    }
}
