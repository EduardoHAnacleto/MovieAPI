using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Data.DTOs;

public class ReadMovieDTO
{

    public string Title { get; set; }

    public string Genre { get; set; }

    public int Duration { get; set; }

    public DateTime ReadTime { get; set; } = DateTime.Now;
}
