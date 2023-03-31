﻿using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        public ICollection<Reviewer> GetReviewers();

        public Reviewer GetReviewer(int reviewerId);

        public ICollection<Review> GetReviewsByAReviewer(int reviewerId);

        public bool ReviewerExists(int reviewerId);

        public bool CreateReviewer(Reviewer reviewer);

        public bool UpdateReviewer(Reviewer reviewer);

        public bool DeleteReviewer(Reviewer reviewer);

        public bool Save();

    }
}
