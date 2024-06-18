﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByteStormApi.Models;
using SQLitePCL;

namespace ByteStormApi.Controllers;

[Route("api/[controller]")]
[ApiController]
    public class OperativoController : ControllerBase
    {
        private readonly ByteStormContext _context;

        public OperativoController(ByteStormContext context)
        {
            _context = context;
        }

        // GET: api/Operativos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperativoDTO>>> GetOperativos()
        {
            return await _context.Operativos
                .Include(o => o.Misiones)
                .Select(x=>ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/Operativos/5
        // <snippet_GetByID>
        [HttpGet("{id}")]
        public async Task<ActionResult<OperativoDTO>> GetOperativo(long id)
        {
            var operativo = await _context.Operativos.FindAsync(id);

            if (operativo == null)
            {
                return NotFound();
            }

            return ItemToDTO(operativo);
        }
        // </snippet_GetByID>

        // PUT: api/Operativos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // <snippet_Update>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperativo(long id, OperativoDTO operativoDTO)
        {
            if (id != operativoDTO.Id)
            {
                return BadRequest();
            }

            var operativo = await _context.Operativos.FindAsync(id);
            if (operativo == null)
            {
                return NotFound();
            }

            if(operativoDTO.Nombre != null) 
                operativo.Nombre = operativoDTO.Nombre;
            if (operativoDTO.Rol != null)
                operativo.Rol = operativoDTO.Rol;

            if(operativoDTO.Misiones != null)
            {
                if (operativo.Misiones == null)
                {
                    operativo.Misiones = new List<Mision>();
                }
                for (int i = 0; i < operativoDTO.Misiones.Count; i++)
                {
                    var aux = await _context.Misiones.FindAsync(operativoDTO.Misiones[i]);
                    if (aux != null)
                        operativo.Misiones.Add(aux);
                }
            }

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
        public async Task<ActionResult<OperativoDTO>> PostOperativo(OperativoDTO operativoDTO)
        {
            var operativo = new Operativo
            {
                Rol = operativoDTO.Rol,
                Nombre = operativoDTO.Nombre,
            };
            if (operativoDTO.Misiones != null)
            {
                operativo.Misiones = new List<Mision>();
                for (int i = 0; i < operativoDTO.Misiones.Count; i++)
                {
                    var aux = await _context.Misiones.FindAsync(operativoDTO.Misiones[i]);
                    if (aux != null)
                        operativo.Misiones.Add(aux);
                }
                
            }
            _context.Operativos.Add(operativo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetOperativo),
                new { id = operativo.Id },
                ItemToDTO(operativo)
                );
        }
        // </snippet_Create>

        // DELETE: api/Operativos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperativo(long id)
        {
            var operativo = _context.Operativos.Find(id);
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
    private static OperativoDTO ItemToDTO(Operativo operativo) {
        var operativoDTO = new OperativoDTO
        {
            Id = operativo.Id,
            Nombre = operativo.Nombre,
            Rol = operativo.Rol
        };
        if (operativoDTO.Misiones == null)
        {
            operativoDTO.Misiones = new List<long>();
        }

        if (operativo.Misiones != null)
        {
            for (int i = 0; i < operativo.Misiones.Count; i++)
            {
                operativoDTO.Misiones.Add(operativo.Misiones[i].Id);
            }
        }
            return operativoDTO;
    }
}
