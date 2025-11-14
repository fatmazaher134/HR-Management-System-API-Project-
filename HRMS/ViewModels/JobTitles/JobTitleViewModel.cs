using System.ComponentModel.DataAnnotations;

namespace HRMS.ViewModels.JobTitles
{
    public class JobTitleViewModel
    {
        public int JobTitleID { get; set; }

        [Display(Name = "Job Title")]
        public string TitleName { get; set; } 
    }
}
