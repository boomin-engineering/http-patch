using System.ComponentModel.DataAnnotations;

namespace HttpPatch.Schema
{
    public class PatchDocument
    {
        [MinLength(1)]
        [Required]
        public IEnumerable<PatchOperation> Operations { get; set; }
    }
}