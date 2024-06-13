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
    public async Task<ActionResult<IEnumerable<EquipoDTO>>> GetEquipos()
    {
        return await _context.Equipos
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    // GET: api/Equipos/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<EquipoDTO>> GetEquipo(long id)
    {
        var equipo = await _context.Equipos.FindAsync(id);

        if (equipo == null)
        {
            return NotFound();
        }

        return ItemToDTO(equipo);
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

        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo == null)
        {
            return NotFound();
        }

        equipo.Descripcion = equipoDTO.Descripcion;
        equipo.Estado = equipoDTO.Estado;
        equipo.Tipo = equipoDTO.Tipo;
        equipo.IdMision = equipoDTO.IdMision;

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
    public async Task<ActionResult<EquipoDTO>> PostEquipo(EquipoDTO equipoDTO)
    {
        var equipo = new Equipo
        {
            Descripcion = equipoDTO.Descripcion,
            Estado = equipoDTO.Estado,
            Tipo = equipoDTO.Tipo,
            IdMision = equipoDTO.IdMision
        };

        _context.Equipos.Add(equipo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEquipo),
            new { id = equipo.Id },
            ItemToDTO(equipo)
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

    private static EquipoDTO ItemToDTO(Equipo equipo) =>
       new EquipoDTO
       {
           Id = equipo.Id,
           Tipo = equipo.Tipo,
           Descripcion = equipo.Descripcion,
           Estado = equipo.Estado,
           IdMision = equipo.IdMision
       };
}
