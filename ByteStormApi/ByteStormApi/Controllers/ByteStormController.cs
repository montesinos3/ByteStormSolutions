using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByteStormApi.Models;

namespace ByteStormApi.Controllers;

[Route("api/[controller]")]
[ApiController]
    public class ByteStormController : ControllerBase
    {
        private readonly ByteStormContext _context;

        public ByteStormController(ByteStormContext context)
        {
            _context = context;
        }

        // GET: api/Operativos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operativo>>> GetOperativos()
        {
            return await _context.Operativos
                .ToListAsync();
        }

        // GET: api/Operativos/5
        // <snippet_GetByID>
        [HttpGet("{id}")]
        public async Task<ActionResult<Operativo>> GetOperativo(long id)
        {
            var operativo = await _context.Operativos.FindAsync(id);

            if (operativo == null)
            {
                return NotFound();
            }

            return operativo;
        }
        // </snippet_GetByID>

        // PUT: api/Operativos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // <snippet_Update>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperativo(long id, Operativo _operativo)
        {
            if (id != _operativo.Id)
            {
                return BadRequest();
            }

            var operativo = await _context.Operativos.FindAsync(id);
            if (operativo == null)
            {
                return NotFound();
            }

                operativo.Nombre = _operativo.Nombre;
                operativo.Rol = _operativo.Rol;
                operativo.Misiones = _operativo.Misiones;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!OperativoExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        // </snippet_Update>

        // POST: api/Operativos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // <snippet_Create>
        [HttpPost]
        public async Task<ActionResult<Operativo>> PostOperativo(Operativo _operativo)
        {
            var operativo = new Operativo
            {
                Rol = _operativo.Rol,
                Nombre = _operativo.Nombre,
                Misiones = _operativo.Misiones
            };

            _context.Operativos.Add(operativo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetOperativo),
                new { id = operativo.Id }
                );
        }
        // </snippet_Create>

        // DELETE: api/Operativos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperativo(long id)
        {
            var operativo = await _context.Operativos.FindAsync(id);
            if (operativo == null)
            {
                return NotFound();
            }

            _context.Operativos.Remove(operativo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OperativoExists(long id)
        {
            return _context.Operativos.Any(o => o.Id == id);
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

            mision.Descripcion= _mision.Descripcion;
            mision.Estado = _mision.Estado;
            mision.Equipos= _mision.Equipos;
            mision.Operativo= _mision.Operativo;
            mision.IdOperativo= _mision.Operativo.Id;

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
