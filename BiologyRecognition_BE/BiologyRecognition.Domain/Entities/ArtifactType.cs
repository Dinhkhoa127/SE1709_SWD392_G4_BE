using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BiologyRecognition.Domain.Entities;

public partial class ArtifactType
{
    public int ArtifactTypeId { get; set; }

    [Required]
    public int ArtifactId { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Tên tối đa 100 ký tự.")]
    public string Name { get; set; }

    [Required]
    [RegularExpression("^(IMAGE|VIDEO|AUDIO|DOCUMENT)$", ErrorMessage = "Type phải là IMAGE, VIDEO, AUDIO hoặc DOCUMENT.")]
    public string Type { get; set; }

    [Url(ErrorMessage = "ArtifactUrl phải là một URL hợp lệ.")]
    public string ArtifactUrl { get; set; }

    [StringLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự.")]
    public string Description { get; set; }

    public virtual Artifact Artifact { get; set; }
}
