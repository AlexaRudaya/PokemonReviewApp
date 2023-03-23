﻿using AutoMapper;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDTO>();

            CreateMap<PokemonDTO, Pokemon>();

            CreateMap<Category, CategoryDTO>();

            CreateMap<CategoryDTO, Category>();

            CreateMap<Country, CountryDTO>();

            CreateMap<CountryDTO, Country>();

            CreateMap<Owner, OwnerDTO>();

            CreateMap<OwnerDTO, Owner>();

            CreateMap<Review, ReviewDTO>();

            CreateMap<ReviewDTO, Review>();

            CreateMap<Reviewer, ReviewerDTO>();

            CreateMap<ReviewerDTO, Reviewer>();
        }
    }
}
