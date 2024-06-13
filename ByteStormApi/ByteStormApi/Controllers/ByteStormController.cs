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
}
