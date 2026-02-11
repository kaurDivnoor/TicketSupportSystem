using System;
using System.ComponentModel.DataAnnotations;

namespace TicketSupportSystem.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Foreign key
        public int TicketId { get; set; }

        // Navigation property
        public Ticket? Ticket { get; set; }
    }
}