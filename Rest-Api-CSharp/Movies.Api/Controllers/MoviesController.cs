﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers
{
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpPost(ApiEndpoints.Movies.Create)]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
        {
            var movie = request.MapToMovie();

            await _movieRepository.CreateAsync(movie);

            return Created($"/{ApiEndpoints.Movies.Create}/{movie.Id}", movie);
        }

        [HttpGet(ApiEndpoints.Movies.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieRepository.GetAllAsync();

            var moviesResponse = movies.MapToResponse();
            return Ok(moviesResponse);
        }

        [HttpGet(ApiEndpoints.Movies.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie is null)
            {
                return NotFound();
            }
            
            var response = movie.MapToResponse();
            return Ok(response);
        }
    }
}
