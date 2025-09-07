using Decidr.Operations.BusinessObjects;
using System.Text.Json.Serialization;

namespace Decidr.Infrastructure.Movies.Models;

public class TheMovieDbResults
{
    [JsonPropertyName("results")]
    public List<TheMovieDbMovie> Movies { get; set; } = [];
}
