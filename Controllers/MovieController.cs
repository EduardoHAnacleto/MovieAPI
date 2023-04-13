using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoListAPI.Data;
using TodoListAPI.Data.DTOs;
using TodoListAPI.Models;

namespace RandomAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{

    private MovieContext _context;
    private IMapper _mapper;

    public MovieController(MovieContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Add a movie to database
    /// </summary>
    /// <param name="movieDto">Object with the necessary camps for creation</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Case added with success</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AddMovie([FromBody] CreateMovieDTO movieDto)
    {

        Movie movie = _mapper.Map<Movie>(movieDto);
        _context.Movies.Add(movie);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id },
            movie);
    }

    /// <summary>
    /// Get movies from database
    /// </summary>
    /// <param name="skip">Integer for the number of rows to be skipped</param>
    /// <param name="take">Integer for the number of rows to be taken</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Case request was successful</response>
    [HttpGet]
    public IEnumerable<ReadMovieDTO> GetMovie([FromQuery]int skip = 0,
        [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadMovieDTO>>(_context.Movies.Skip(skip).Take(take));
    }

    /// <summary>
    /// Get movies from database by using an integer ID
    /// </summary>
    /// <param name="id">Integer for the ID of the row selected to find</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Case request was successful</response>
    [HttpGet("{id}")]
    public IActionResult GetMovieById(int id)
    {
        var movie = _context.Movies
            .FirstOrDefault(x => x.Id == id);
        if ( movie == null) return NotFound();
        var movieDTO = _mapper.Map<ReadMovieDTO>(movie);
        return Ok(movie);
    }

    /// <summary>
    /// Update an item, saving to database
    /// </summary>
    /// <param name="id">Integer for the number of the row to be updated</param>
    /// <param name="movieDTO">Object necessary for update</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Case update was successful</response>
    [HttpPut("{id}")]
    public IActionResult UpdateMovie(int id, 
        [FromBody] UpdateMovieDTO movieDTO)
    {
        var movie = _context.Movies.FirstOrDefault(
            x => x.Id == id);
        if ( movie == null ) return NotFound();
        _mapper.Map(movieDTO, movie);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Patch an item, saving to database
    /// </summary>
    /// <param name="id">Integer for the number of row to be patched</param>
    /// <param name="patch">Variable to be patched</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Case patch was successful</response>
    [HttpPatch("{id}")]
    public IActionResult PatchMovie(int id,
        JsonPatchDocument<UpdateMovieDTO> patch)
    {
        var movie = _context.Movies.FirstOrDefault(
            x => x.Id == id);
        if (movie == null) return NotFound();

        var movieToUpdate = _mapper.Map<UpdateMovieDTO>(movie);
        patch.ApplyTo(movieToUpdate, ModelState);
        if (!TryValidateModel(movieToUpdate))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(movieToUpdate, movie);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Delete movie from database
    /// </summary>
    /// <param name="id">Integer for the number of row to be deleted</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Case delete was successful</response>
    [HttpDelete("{id}")]
    public IActionResult DeleteMovie(int id)
    {
        var movie = _context.Movies.FirstOrDefault(
            x => x.Id == id);
        if (movie == null) return NotFound();

        _context.Remove(movie);
        return NoContent();
    }
}
