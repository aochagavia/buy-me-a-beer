using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class CommentFormModel
    {
        [StringLength(50)]
        public string Nickname { get; set; }
        [StringLength(280)]
        public string Message { get; set; }
    }
}
