﻿#region Usings

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    abstract class ApplicationReferencable
    {
        public ApplicationReferencable() { }

        public ApplicationReferencable(Application application, string idForApplication)
        {
            ApplicationName = application.ApplicationName;
            Application = application;
            IdForApplication = idForApplication;
        }

        [Required]
        [ForeignKey("Application")]
        [Index("IX_OneInApplication", 1, IsUnique = true)]
        public string ApplicationName { get; set; }

        public virtual Application Application { get; set; }

        [Required]
        [Index("IX_OneInApplication", 2, IsUnique = true)]
        [MaxLength(128)]
        public string IdForApplication { get; set; }
    }
}