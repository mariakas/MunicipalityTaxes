using System.ComponentModel.DataAnnotations;

namespace MunicipalityTaxes.Core.Domain.Classifications
{
    public class Municipality : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
