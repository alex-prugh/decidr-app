using Decidr.Api.Dtos;
using Decidr.Api.Extensions;
using Decidr.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Decidr.Api.Controllers;

[ApiController]
[Route("api/sets")]
[RequiresUserContext]
public class SetsController : ControllerBase
{
    private readonly ISetsOperation _setsOperation;

    public SetsController(
        ISetsOperation setsOperation)
    {
        _setsOperation = setsOperation;
    }

    /// <summary>
    /// Gets all of the sets that the logged-in user can view / vote on.
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<SetDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        var sets = await _setsOperation.GetAllSetsAsync(cancellationToken);
        return sets.Select(s => s.ToDto()).ToList();
    }

    /// <summary>
    /// Gets a single set by id.
    /// </summary>
    [HttpGet("{id:long}")]
    [Authorize]
    public async Task<ActionResult<SetDto?>> GetById(long id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            return BadRequest("SetId must be greater than 0");
        }

        var set = await _setsOperation.GetSetAsync(id, cancellationToken);
        if (set == null)
        {
            return NotFound();
        }

        return Ok(set.ToDto());
    }

    /// <summary>
    /// Returns the voting results of a set.
    /// </summary>
    [HttpGet("{id:long}/results")]
    public async Task<ActionResult<SetResultDto?>> GetResults(long id, CancellationToken cancellationToken = default)
    {
        var setResult = await _setsOperation.GetSetResultAsync(id, cancellationToken);
        return setResult?.ToDto();
    }

    /// <summary>
    /// Adds a member to be able to view / vote on a set.
    /// </summary>
    [HttpPost("{id:long}/addMember")]
    public async Task<ActionResult<bool>> AddMember(long id, AddMemberRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _setsOperation.AddMemberAsync(id, request.Email, cancellationToken);
        if (!result)
        {
            return NotFound();
        }

        return true;
    }
}
