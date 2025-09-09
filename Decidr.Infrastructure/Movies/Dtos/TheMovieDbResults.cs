using Decidr.Operations.BusinessObjects;
using System.Text.Json.Serialization;

namespace Decidr.Infrastructure.Movies.Dtos;

public class TheMovieDbResults
{
    [JsonPropertyName("results")]
    public List<TheMovieDbMovieDto> Movies { get; set; } = [];
}
