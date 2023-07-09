using Bogus.DataSets;
using System.ComponentModel.DataAnnotations;

namespace PracticeAppMvc.Net.Models
{
    public class CreatePostModel:Post
    {
        [Display(Name = "Chuyên mục")]
        public int[] CategoryIDs { get; set; }
    }
}
