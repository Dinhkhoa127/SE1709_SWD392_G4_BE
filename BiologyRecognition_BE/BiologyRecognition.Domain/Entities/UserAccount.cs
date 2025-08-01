﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BiologyRecognition.Domain.Entities;

public partial class UserAccount
{
    public int UserAccountId { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public int RoleId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; }

    public string OtpCode { get; set; }

    public DateTime? OtpExpiry { get; set; }

    public virtual ICollection<Article> ArticleCreatedByNavigations { get; set; } = new List<Article>();

    public virtual ICollection<Article> ArticleModifiedByNavigations { get; set; } = new List<Article>();

    public virtual ICollection<Artifact> ArtifactCreatedByNavigations { get; set; } = new List<Artifact>();

    public virtual ICollection<Artifact> ArtifactModifiedByNavigations { get; set; } = new List<Artifact>();

    public virtual ICollection<Chapter> ChapterCreatedByNavigations { get; set; } = new List<Chapter>();

    public virtual ICollection<Chapter> ChapterModifiedByNavigations { get; set; } = new List<Chapter>();

    public virtual ICollection<Recognition> Recognitions { get; set; } = new List<Recognition>();

    public virtual ICollection<Subject> SubjectCreatedByNavigations { get; set; } = new List<Subject>();

    public virtual ICollection<Subject> SubjectModifiedByNavigations { get; set; } = new List<Subject>();

    public virtual ICollection<Topic> TopicCreatedByNavigations { get; set; } = new List<Topic>();

    public virtual ICollection<Topic> TopicModifiedByNavigations { get; set; } = new List<Topic>();
}