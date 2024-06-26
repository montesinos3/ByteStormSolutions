using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByteStormApi.Models;
using ByteStormApi.Repositories;
using SQLitePCL;

namespace ByteStormApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MisionesController : ControllerBase
{
    private readonly IMisionRepository _repo;

    public MisionesController(IMisionRepository repo)
    {
        _repo = repo;
    }

    // GET: api/Misiones
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MisionDTO>>> GetMisiones()
    {
        return await _repo.GetAllAsync();
    }

    // GET: api/Misiones/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<MisionDTO>> GetMision(long id)
    {
        var mision = await _repo.GetById(id);
        if (mision == null)
        {
            return NotFound();
        }
        return mision;
    }
    // </snippet_GetByID>

    // PUT: api/Misiones/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMision(long id, MisionDTO misionDTO)
    {
        if (id != misionDTO.Id)
        {
            return BadRequest();
        }

        var mision = await _repo.Update(misionDTO, id);
        if (mision == "notfound")
        {
            return NotFound();
        }
        else if (mision == "badRequest")
        {
            return BadRequest();
        }
        else if (mision == "noContent")
        {
            return NoContent();
        }
        return Ok();
    }
    // </snippet_Update>

    // POST: api/Misiones
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<MisionDTO>> PostMision(MisionDTO misionDTO)
    {
        var mision = await _repo.Insert(misionDTO);
        if (mision == null)
        {
            return BadRequest();
        }

        return CreatedAtAction(
            nameof(GetMision),
            new { id = mision.Id },
            mision
            );
    }
    // </snippet_Create>

    // DELETE: api/Misiones/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMision(long id)
    {
        var mision = await _repo.Delete(id);
        if (mision == false)
        {
            return NotFound();
        }
        return NoContent();
    }
}
