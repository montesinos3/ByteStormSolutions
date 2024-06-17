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
    public async Task<ActionResult<IEnumerable<MisionDTO>>> GetMisiones()
    {
        return await _context.Misiones
            .Include(m => m.Equipos)
            .Select(X=>ItemToDTO(X))
            .ToListAsync();
    }

    // GET: api/Misiones/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<MisionDTO>> GetMision(long id)
    {
        var mision = await _context.Misiones.FindAsync(id);

        if (mision == null)
        {
            return NotFound();
        }

        return ItemToDTO(mision);
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

        var mision = await _context.Misiones.FindAsync(id);
        if (mision == null)
        {
            return NotFound();
        }

        mision.Descripcion = misionDTO.Descripcion;
        mision.Estado = misionDTO.Estado;
        mision.IdOperativo = misionDTO.IdOperativo;

        if (misionDTO.Equipos != null)
        {
            if (mision.Equipos == null)
            {
                mision.Equipos = new List<Equipo>();
            }
            for (int i = 0; i < misionDTO.Equipos.Count; i++)
            {
                var aux = _context.Equipos.Find(misionDTO.Equipos[i]);
                if (aux != null)
                    mision.Equipos.Add(aux);
            }
        }
        

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
    public async Task<ActionResult<MisionDTO>> PostMision(MisionDTO misionDTO)
    {
        var mision = new Mision
        {
            Descripcion = misionDTO.Descripcion,
            Estado = misionDTO.Estado,
            IdOperativo = misionDTO.IdOperativo
        };

        if (misionDTO.Equipos != null)
        {
            mision.Equipos = new List<Equipo>();
            for (int i = 0; i < misionDTO.Equipos.Count; i++)
            {
                var aux = _context.Equipos.Find(misionDTO.Equipos[i]);
                if (aux != null)
                    mision.Equipos.Add(aux);
                }
        }

        _context.Misiones.Add(mision);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetMision),
            new { id = mision.Id },
            ItemToDTO(mision)
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

    private static MisionDTO ItemToDTO(Mision mision)
    {
        var misionDTO = new MisionDTO
        {
            Id = mision.Id,
            Descripcion = mision.Descripcion,
            Estado = mision.Estado,
            IdOperativo = mision.IdOperativo
        };
        if (misionDTO.Equipos == null)
        {
            misionDTO.Equipos = new List<long>();
        }
        if (mision.Equipos != null)
        {
            for (int i = 0; i < mision.Equipos.Count; i++)
            {
                misionDTO.Equipos.Add(mision.Equipos[i].Id);
            }
        }
        return misionDTO;

    }
}
