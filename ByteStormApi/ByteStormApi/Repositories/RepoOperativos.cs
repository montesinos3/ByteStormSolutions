using ByteStormApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ByteStormApi.Repositories
{
    public interface IOperativoRepository
    {
        Task<Operativo> Insert(Operativo entity);
        Task<OperativoDTO?> GetById(long id);
        Task<ActionResult<IEnumerable<OperativoDTO>>> GetAllAsync();
        void Update(Operativo entity);
        Task<bool> Delete(long id);
    }

    public abstract class RepoOperativos : IOperativoRepository
    {
        private readonly ByteStormContext _context;
        protected RepoOperativos(ByteStormContext context)
        {
            _context = context;
        }

        public async Task<Operativo> Insert(Operativo entity)
        {
            EntityEntry<Operativo> insertedValue = await _context.Set<Operativo>().AddAsync(entity);
            return insertedValue.Entity;
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

        public void Update(Operativo entity)
        {
            //entity.LastUpdateUtc = DateTime.UtcNow;
            //_context.Set<Operativo>().Update(entity);
        }

        public async Task<bool> Delete(long id)
        {
            //Operativo? entity = await GetById(id);
            //if (entity is null)
            //    return false;
            //_context.Set<Operativo>().Remove(entity);
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
    }

}
