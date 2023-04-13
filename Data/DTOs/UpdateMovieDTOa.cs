using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Data.DTOs;

public class UpdateMovieDTO
{

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(50, ErrorMessage = "Title must be less than 50 characters.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Genre is required.")]
    [StringLength(50, ErrorMessage = "Genre must be less than 50 characters.")]
    public string Genre { get; set; }

    [Required(ErrorMessage = "Duration is required.")]
    [Range(40, 600, ErrorMessage = "Duration must be between 40 and 600 minutes.")]
    public int Duration { get; set; }
}
