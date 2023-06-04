using System.ComponentModel.DataAnnotations;

namespace FinalProjectShortenURL.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public string PublicIP { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.Now;
    }
}
