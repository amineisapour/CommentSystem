using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        [MaxLength(254)]
        public string Username { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int Level { get; set; }

        [AllowNull]
        public int? ParentId { get; set; }
    }
}
