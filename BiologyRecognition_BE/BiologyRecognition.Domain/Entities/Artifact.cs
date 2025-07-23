using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiologyRecognition.Domain.Entities;

public partial class Artifact
{
    public int ArtifactId { get; set; }

    [Required]
    public int TopicId { get; set; }

    [Required(ErrorMessage = "Tên không được để trống.")]
    [StringLength(150, ErrorMessage = "Tên không được vượt quá 150 ký tự.")]
    public string Name { get; set; }

    [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
    public string Description { get; set; }

    [StringLength(150, ErrorMessage = "Tên khoa học không được vượt quá 150 ký tự.")]
    public string ScientificName { get; set; }

    public int? CreatedBy { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<ArtifactImage> ArtifactImages { get; set; } = new List<ArtifactImage>();

    public virtual ICollection<ArtifactType> ArtifactTypes { get; set; } = new List<ArtifactType>();

    public virtual UserAccount CreatedByNavigation { get; set; }

    public virtual UserAccount ModifiedByNavigation { get; set; }

    public virtual ICollection<Recognition> Recognitions { get; set; } = new List<Recognition>();

    public virtual Topic Topic { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}
