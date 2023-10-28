using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerMVC.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Column(TypeName ="nvarchar(50)")]
        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(5)")]
        public string Icon { get; set; } = "";

        public string Type { get; set; } = "Expense";

        [NotMapped]
        public string? TitleWithIcon {
            get {
                return this.Title +" "+ this.Icon;
            
            } }



    }
}
