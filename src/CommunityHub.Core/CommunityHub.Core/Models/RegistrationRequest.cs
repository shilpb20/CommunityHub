﻿using CommunityHub.Core.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityHub.Core.Models
{
    public class RegistrationRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]

        public string RegistrationInfo { get; set; }
        public string RegistrationStatus { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? Review { get; set; }
    }
}