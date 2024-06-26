using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByteStormApi.Models;
using ByteStormApi.Repositories;

namespace ByteStormApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class EquiposController : ControllerBase
{
    private readonly IEquipoRepository _repo;

    public EquiposController(IEquipoRepository repo)
    {
        _repo = repo;
    }



    // GET: api/Equipos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EquipoDTO>>> GetEquipos()
    {
        return await _repo.GetAllAsync();
    }

    // GET: api/Equipos/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<EquipoDTO>> GetEquipo(long id)
    {
        var equipo = await _repo.GetById(id);
        if (equipo == null)
        {
            return NotFound();
        }
        return equipo;
    }
    // </snippet_GetByID>

    // PUT: api/Equipos/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEquipo(long id, EquipoDTO equipoDTO)
    {
        if (id != equipoDTO.Id)
        {
            return BadRequest();
        }

        var equipo = await _repo.Update(equipoDTO, id);
        if (equipo == "notfound")
        {
            return NotFound();
        }
        else if (equipo == "badRequest")
        {
            return BadRequest();
        }
        else if (equipo == "noContent")
        {
            return NoContent();
        }
        return Ok();
    }
    // </snippet_Update>

    // POST: api/Equipos
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<EquipoDTO>> PostEquipo(EquipoDTO equipoDTO)
    {
        var equipo = await _repo.Insert(equipoDTO);
        if (equipo == null)
        {
            return BadRequest();
        }

        return CreatedAtAction(
            nameof(GetEquipo),
            new { id = equipo.Id },
            equipo
            );
    }
    // </snippet_Create>

    // DELETE: api/Equipos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipo(long id)
    {
        var equipo = await _repo.Delete(id);
        if (equipo == false)
        {
            return NotFound();
        }
        return NoContent();
    }

}
