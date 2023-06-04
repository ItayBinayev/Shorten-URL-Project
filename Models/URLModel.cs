using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectShortenURL.Models
{
    public class URLModel
    {
        [Required]
        [Key]
        public string ShortURL { get; set; }
        [Required]
        public string FullURL { get; set; }
        [Required]
        public int CounterOfRequests { get; set; }
        [Required]
        public DateTime URLCreated { get; set; } = DateTime.Now;
        public int? UrlUserId { get; set; }
        public ICollection<Entry> Entries { get; set; } = new List<Entry>();
        
    }
}
