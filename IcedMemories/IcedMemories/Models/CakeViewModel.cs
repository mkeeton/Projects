using System;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IcedMemories.Models
{
  public class CakeViewModel
  {
    public Guid Id { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date Added")]
    public DateTime DateAdded { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Text)]
    [Display(Name = "Title")]
    public String Title { get; set; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Description")]
    public String Description { get; set; }

    [DataType(DataType.ImageUrl)]
    [Display(Name = "Image")]
    public String ImageLink { get; set; }

    [DataType(DataType.Upload)]
    [Display(Name = "Cake Image")]
    public HttpPostedFileBase ImageUpload { get; set; }
  }
}