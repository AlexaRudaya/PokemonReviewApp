﻿using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public sealed class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _context.Owners
                    .Where(_ => _.Id == ownerId)
                    .FirstOrDefault();

            var category = _context.Categories
                    .Where(_ => _.Id == categoryId)
                    .FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon
            };

            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon
            };

            _context.Add(pokemonCategory);

            _context.Add(pokemon);

            return Save();
        }

        public Pokemon? GetPokemon(int id)  // it's gonna be a detail page
        {
            return _context.Pokemon
                        .Where(_ => _.Id == id)
                        .FirstOrDefault();
        }

        public Pokemon? GetPokemon(string name)
        {
            return _context.Pokemon
                        .Where(_ => _.Name == name)
                        .FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokemonId)
        {
            var review = _context.Reviews
                            .Where(_ => _.Pokemon!.Id == pokemonId);

            if (review.Count() <= 0)
            {
                return 0;
            }
            else
            {
                return ((decimal)review.Sum(_ => _.Rating) / review.Count());   
            }
        } 

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemon
                        .OrderBy(_ => _.Id).ToList(); // ToList - crucial.
        }

        public bool PokemonExists(int pokemonId)
        {
            return _context.Pokemon
                        .Any(_ => _.Id == pokemonId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}
