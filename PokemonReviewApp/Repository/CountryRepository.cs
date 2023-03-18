using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public sealed class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CountryExists(int id)
        {
            return _context.Countries
                        .Any(_ => _.Id == id);
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country? GetCountry(int id)
        {
            return _context.Countries
                        .Where(_ => _.Id == id)
                        .FirstOrDefault();      
        }

        public Country? GetCountryByOwner(int ownerId)
        {
            return _context.Owners
                        .Where(_ => _.Id == ownerId)
                        .Select(_ => _.Country)
                        .FirstOrDefault();
        }

        public ICollection<Owner>? GetOwnersFromACountry(int countryId)
        {
            return _context.Owners
                        .Where(_ => _.Country.Id == countryId)
                        .ToList();
        }
    }
}
