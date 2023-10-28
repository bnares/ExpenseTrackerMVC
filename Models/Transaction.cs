using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerMVC.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        //CategoryId
        [Range(1, int.MaxValue, ErrorMessage="PLease select Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        [Range(1,int.MaxValue, ErrorMessage ="Amount is Required from 1$")]
        public int Amount { get; set; }
        [Column(TypeName = "nvarchar(75)")]
        public string? Note { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [NotMapped]
        public string? CategoryTitleWithIcon
        {
            get
            {
                return Category==null ? "" : Category.Title+" "+Category.Icon;
            }
        }

        [NotMapped]
        public string? FormattedAmount { get {
                return ((Category== null || Category.Type=="Expense") ? "-" :"+") + Amount.ToString("C0");
            } }


    }
}
