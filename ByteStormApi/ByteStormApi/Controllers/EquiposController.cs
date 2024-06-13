using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByteStormApi.Models;

namespace ByteStormApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class EquiposController : ControllerBase
{
    private readonly ByteStormContext _context;

    public EquiposController(ByteStormContext context)
    {
        _context = context;
    }



    // GET: api/Equipos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Equipo>>> GetEquipos()
    {
        return await _context.Equipos
            .ToListAsync();
    }

    // GET: api/Equipos/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<Equipo>> GetEquipo(long id)
    {
        var equipo = await _context.Equipos.FindAsync(id);

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
    public async Task<IActionResult> PutEquipo(long id, Equipo _equipo)
    {
        if (id != _equipo.Id)
        {
            return BadRequest();
        }

        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo == null)
        {
            return NotFound();
        }

        equipo.Descripcion = _equipo.Descripcion;
        equipo.Estado = _equipo.Estado;
        equipo.Tipo = _equipo.Tipo;
        equipo.Mision = _equipo.Mision;
        equipo.IdMision = _equipo.Mision.Id;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!EquipoExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }
    // </snippet_Update>

    // POST: api/Equipos
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<Equipo>> PostEquipo(Equipo _equipo)
    {
        var equipo = new Equipo
        {
            Descripcion = _equipo.Descripcion,
            Estado = _equipo.Estado,
            Tipo = _equipo.Tipo,
            Mision = _equipo.Mision,
            IdMision = _equipo.Mision.Id
        };

        _context.Equipos.Add(equipo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEquipo),
            new { id = equipo.Id }
            );
    }
    // </snippet_Create>

    // DELETE: api/Equipos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEquipo(long id)
    {
        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo == null)
        {
            return NotFound();
        }

        _context.Equipos.Remove(equipo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EquipoExists(long id)
    {
        return _context.Equipos.Any(e => e.Id == id);
    }
}
