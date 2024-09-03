using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.DAL.Models
{
    public class ResumeSkill
    {
        [Key]
        public Guid ResumeSkillId=Guid.NewGuid();

        [Required]
        public Guid ResumeId { get; set; }

        public Resume Resume { get; set; }

        [Required]
        [MaxLength(100)]
        public string SkillName { get; set; }  

        [MaxLength(50)]
        public string? ProficiencyLevel { get; set; } 

        [Range(0, 50)]
        public int? YearsOfExperience { get; set; }
    }
    

    
}
