﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CommunityHub.Infrastructure.Models
{
    public class UserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(50)]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string CountryCode { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string MaritalStatus { get; set; } = string.Empty;

        [Required]
        public string HomeTown { get; set; } = string.Empty;
        public string? HouseName { get; set; }

        public virtual SpouseInfo? SpouseInfo { get; set; }
        public virtual List<Children> Children { get; set; } = new List<Children>();

        public virtual FamilyPicture? FamilyPicture { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }
    }
}