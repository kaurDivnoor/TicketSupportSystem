using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TicketSupportSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a priority")]
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Critical

        [Required(ErrorMessage = "Please select a status")]
        public string Status { get; set; } = "Open"; // Open, In Progress, Resolved, Closed

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Last Updated")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation property for comments
        public List<Comment> Comments { get; set; } = new List<Comment>();

        // Helper property for display
        public string PriorityClass => Priority.ToLower() switch
        {
            "low" => "badge bg-success",
            "medium" => "badge bg-warning",
            "high" => "badge bg-danger",
            "critical" => "badge bg-dark",
            _ => "badge bg-secondary"
        };

        public string StatusClass => Status.ToLower() switch
        {
            "open" => "badge bg-primary",
            "in progress" => "badge bg-info",
            "resolved" => "badge bg-success",
            "closed" => "badge bg-secondary",
            _ => "badge bg-secondary"
        };
    }
}