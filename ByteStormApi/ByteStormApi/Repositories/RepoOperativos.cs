using ByteStormApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByteStormApi.Repositories
{
    public interface IOperativoRepository
    {
        Task<OperativoDTO> Insert(OperativoDTO operativoDTO);
        Task<OperativoDTO?> GetById(long id);
        Task<ActionResult<IEnumerable<OperativoDTO>>> GetAllAsync();
        Task<string> Update(OperativoDTO operativoDTO, long id);
        Task<bool> Delete(long id);
    }

    public class RepoOperativos : IOperativoRepository
    {
        private readonly ByteStormContext _context;
        public RepoOperativos(ByteStormContext context)
        {
            _context = context;
        }

        public async Task<OperativoDTO> Insert(OperativoDTO operativoDTO)
        {
            var operativo = new Operativo
            {
                Rol = operativoDTO.Rol,
                Nombre = operativoDTO.Nombre,
            };
            operativo.Misiones = new List<Mision>();
            if (operativoDTO.Misiones != null)
            {
                if (operativoDTO.Misiones.Count > 0)
                {
                    for (int i = 0; i < operativoDTO.Misiones.Count; i++)
                    {
                        var aux = await _context.Misiones.FindAsync(operativoDTO.Misiones[i]);
                        if (aux != null)
                        {
                            operativo.Misiones.Add(aux);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            _context.Operativos.Add(operativo);
            await _context.SaveChangesAsync();

            return ItemToDTO(operativo);
        }

        public virtual async Task<OperativoDTO?> GetById(long id){
            var operativo = await _context.Operativos.FindAsync(id);

            if (operativo == null)
            {
                return null;
            }

            return ItemToDTO(operativo);
        }


        public async Task<ActionResult<IEnumerable<OperativoDTO>>> GetAllAsync() => await _context.Operativos
                    .Include(o => o.Misiones)
                    .Select(x => ItemToDTO(x))
                    .ToListAsync();

        public async Task<string> Update(OperativoDTO operativoDTO, long id)
        {
            var operativo = await _context.Operativos.Where(o => o.Id == id).Include(o => o.Misiones).FirstOrDefaultAsync();
            if (operativo == null)
            {
                return "notfound";
            }

            if (operativoDTO.Nombre != null)
                operativo.Nombre = operativoDTO.Nombre;
            if (operativoDTO.Rol != null)
                operativo.Rol = operativoDTO.Rol;

            if (operativoDTO.Misiones != null)
            {
                if (operativoDTO.Misiones.Count > 0)
                {
                    for (int i = 0; i < operativoDTO.Misiones.Count; i++)
                    {
                        if (operativo.Misiones != null)
                        {
                            var aux = await _context.Misiones.FindAsync(operativoDTO.Misiones[i]);
                            if (aux != null)
                            {
                                if (operativo.Misiones != null)
                                    operativo.Misiones.Add(aux);
                            }
                            else
                            {
                                return "badRequest";
                            }
                        }
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!OperativoExists(id))
            {
                return "notfound";
            }

            return "noContent";
        }

        public async Task<bool> Delete(long id)
        {
            var operativo = await _context.Operativos.FindAsync(id);
            if (operativo == null)
            {
                return false;
            }

            _context.Operativos.Remove(operativo);
            await _context.SaveChangesAsync();

            return true;
        }
        private static OperativoDTO ItemToDTO(Operativo operativo)
        {
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
        private bool OperativoExists(long id)
        {
            return _context.Operativos.Any(o => o.Id == id);
        }
    }

}
