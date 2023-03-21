﻿using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public sealed class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context) 
        {
            _context = context; 
        }

        public Reviewer? GetReviewer(int reviewerId)
        {
            return _context.Reviewers
                         .Where(_ => _.Id == reviewerId)
                         .Include(_ => _.Reviews)
                         .FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.ToList();
        }

        public ICollection<Review> GetReviewsByAReviewer(int reviewerId)
        {
            return _context.Reviews
                        .Where(_ => _.Reviewer.Id == reviewerId)
                        .ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _context.Reviewers
                        .Any(_ => _.Id == reviewerId);
        }
    }
}
