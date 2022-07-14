﻿using Samp.Core.Entities;
using Samp.Movie.Database.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Samp.Movie.Database.Entities
{
    [Table("MovieEntity")]
    public class MovieEntity : BaseEntity
    {
        public MovieEntity()
        {
            Type = MovieType.movie;
            Categories = new HashSet<MovieCategoryEntity>();
            MovieDirectors = new HashSet<MovieDirectorEntity>();
            MovieWriters = new HashSet<MovieWriterEntity>();
            //PaymentHistory = new HashSet<PaymentHistory>();
        }

        //public virtual ICollection<PaymentHistory> PaymentHistory { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<MovieCategoryEntity> Categories { get; set; }

        [Required]
        public int RuntimeMinutes { get; set; }

        [Required]
        public int StartYear { get; set; }

        public virtual ICollection<MovieDirectorEntity> MovieDirectors { get; set; }
        public virtual ICollection<MovieWriterEntity> MovieWriters { get; set; }
        public string Description { get; set; }
        public Guid RatingId { get; set; }
        public virtual RatingEntity Rating { get; set; }
        public MovieType Type { get; set; }

        public decimal UsdPrice { get; set; }
    }
}