using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.JobTitles
{
    public class JobTitleFormViewModel
    {
        public int JobTitleID { get; set; } 

        [Required(ErrorMessage = "Job title is required.")]
        [StringLength(150)]
        [Display(Name = "Title Name")]
        public string TitleName { get; set; }
    }
}
