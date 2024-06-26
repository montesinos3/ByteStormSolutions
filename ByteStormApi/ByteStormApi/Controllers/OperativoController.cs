using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ByteStormApi.Models;
using ByteStormApi.Repositories;
using SQLitePCL;

namespace ByteStormApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OperativoController : ControllerBase
{
    private readonly IOperativoRepository _repo;

    public OperativoController(IOperativoRepository repo)
    {
        _repo = repo;
    }

    // GET: api/Operativos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OperativoDTO>>> GetOperativos()
    {
        return await _repo.GetAllAsync();
    }

    // GET: api/Operativos/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<OperativoDTO>> GetOperativo(long id)
    {
        var operativo = await _repo.GetById(id);
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
    public async Task<IActionResult> PutOperativo(long id, OperativoDTO operativoDTO)
    {
        if (id != operativoDTO.Id)
        {
            return BadRequest();
        }

        var operativo = await _repo.Update(operativoDTO, id);
        if (operativo == "notfound")
        {
            return NotFound();
        } else if (operativo == "badRequest")
        {
            return BadRequest();
        } else if(operativo == "noContent")
        {
            return NoContent();
        }
        return Ok();
    }
    // </snippet_Update>

    // POST: api/Operativos
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<OperativoDTO>> PostOperativo(OperativoDTO operativoDTO)
    {
        var operativo = await _repo.Insert(operativoDTO);
        if (operativo == null)
        {
            return BadRequest(); 
        }

        return CreatedAtAction(
            nameof(GetOperativo),
            new { id = operativo.Id },
            operativo
            );
    }
    // </snippet_Create>

    // DELETE: api/Operativos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOperativo(long id)
    {
        var operativo = await _repo.Delete(id);
        if (operativo == false)
        {
            return NotFound();
        }
        return NoContent();
    }
}
