using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Backend.Entities
{
    public class Comment
    {
        public Comment() : base()
        {
        }

        [Required, Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(254)]
        public string Username { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime RegisterDateTime { get; set; }

        [AllowNull]
        public int? ParentId { get; set; }
        
        public int Level { get; set; }
        
        public virtual IEnumerable<Comment> Children { get; set; }
        public virtual Comment Parent { get; set; }

    }
}
