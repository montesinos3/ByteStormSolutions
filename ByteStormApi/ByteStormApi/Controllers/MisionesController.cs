using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByteStormApi.Models;

namespace ByteStormApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class MisionesController : ControllerBase
{
    private readonly ByteStormContext _context;

    public MisionesController(ByteStormContext context)
    {
        _context = context;
    }

    // GET: api/Misiones
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mision>>> GetMisiones()
    {
        return await _context.Misiones
            .ToListAsync();
    }

    // GET: api/Misiones/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<Mision>> GetMision(long id)
    {
        var mision = await _context.Misiones.FindAsync(id);

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
    public async Task<IActionResult> PutMision(long id, Mision _mision)
    {
        if (id != _mision.Id)
        {
            return BadRequest();
        }

        var mision = await _context.Misiones.FindAsync(id);
        if (mision == null)
        {
            return NotFound();
        }

        mision.Descripcion = _mision.Descripcion;
        mision.Estado = _mision.Estado;
        mision.Equipos = _mision.Equipos;
        mision.Operativo = _mision.Operativo;
        mision.IdOperativo = _mision.Operativo.Id;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!MisionExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }
    // </snippet_Update>

    // POST: api/Misiones
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<Mision>> PostMision(Mision _mision)
    {
        var mision = new Mision
        {
            Descripcion = _mision.Descripcion,
            Estado = _mision.Estado,
            Equipos = _mision.Equipos,
            Operativo = _mision.Operativo,
            IdOperativo = _mision.Operativo.Id
        };

        _context.Misiones.Add(mision);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetMision),
            new { id = mision.Id }
            );
    }
    // </snippet_Create>

    // DELETE: api/Misiones/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMision(long id)
    {
        var mision = await _context.Misiones.FindAsync(id);
        if (mision == null)
        {
            return NotFound();
        }

        _context.Misiones.Remove(mision);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MisionExists(long id)
    {
        return _context.Misiones.Any(m => m.Id == id);
    }
}
