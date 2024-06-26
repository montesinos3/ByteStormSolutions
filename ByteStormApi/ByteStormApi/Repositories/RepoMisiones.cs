using ByteStormApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByteStormApi.Repositories
{
    public interface IMisionRepository
    {
        Task<MisionDTO> Insert(MisionDTO misionDTO);
        Task<MisionDTO?> GetById(long id);
        Task<ActionResult<IEnumerable<MisionDTO>>> GetAllAsync();
        Task<string> Update(MisionDTO misionDTO, long id);
        Task<bool> Delete(long id);
    }

    public class RepoMisiones : IMisionRepository
    {
        private readonly ByteStormContext _context;
        public RepoMisiones(ByteStormContext context)
        {
            _context = context;
        }

        public async Task<MisionDTO> Insert(MisionDTO misionDTO)
        {
            var mision = new Mision
            {
                Descripcion = misionDTO.Descripcion,
                IdOperativo = misionDTO.IdOperativo
            };
            if (misionDTO.Estado != null)
            {
                if (misionDTO.Estado != EstadoM.Planificada && misionDTO.Estado != EstadoM.Activa && misionDTO.Estado != EstadoM.Completada)
                {
                    return null;
                }
            }
            mision.Estado = misionDTO.Estado;

            mision.Equipos = new List<Equipo>();
            if (misionDTO.Equipos != null)
            {
                if (misionDTO.Equipos.Count > 0)
                {
                    for (int i = 0; i < misionDTO.Equipos.Count; i++)
                    {
                        var aux = _context.Equipos.Find(misionDTO.Equipos[i]);
                        if (aux != null)
                        {
                            mision.Equipos.Add(aux);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

            _context.Misiones.Add(mision);
            await _context.SaveChangesAsync();

            return ItemToDTO(mision);
        }

        public virtual async Task<MisionDTO?> GetById(long id){
            var mision = await _context.Misiones.FindAsync(id);

            if (mision == null)
            {
                return null;
            }

            return ItemToDTO(mision);
        }


        public async Task<ActionResult<IEnumerable<MisionDTO>>> GetAllAsync() => await _context.Misiones
                    .Include(o => o.Equipos)
                    .Select(x => ItemToDTO(x))
                    .ToListAsync();

        public async Task<string> Update(MisionDTO misionDTO, long id)
        {
            var mision = await _context.Misiones.Where(m => m.Id == id).Include(m => m.Equipos).FirstOrDefaultAsync();
            if (mision == null)
            {
                return "notfound";
            }

            if (misionDTO.Descripcion != null)
            {
                mision.Descripcion = misionDTO.Descripcion;
            }
            if (misionDTO.Estado != null)
            {
                if (misionDTO.Estado == EstadoM.Planificada || misionDTO.Estado == EstadoM.Activa || misionDTO.Estado == EstadoM.Completada)
                {
                    mision.Estado = misionDTO.Estado;
                }
                else
                {
                    return "badRequest";
                }
            }
            if (misionDTO.IdOperativo != null)
            {
                mision.IdOperativo = misionDTO.IdOperativo;
            }

            if (misionDTO.Equipos != null)
            {
                if (misionDTO.Equipos.Count > 0)
                {
                    for (int i = 0; i < misionDTO.Equipos.Count; i++)
                    {
                        if (misionDTO.Equipos[i] != null)
                        {
                            var aux = _context.Equipos.Find(misionDTO.Equipos[i]);
                            if (aux != null)
                            {
                                if (mision.Equipos != null)
                                    mision.Equipos.Add(aux);
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
            catch (DbUpdateConcurrencyException) when (!MisionExists(id))
            {
                return "notfound";
            }

            return "noContent";
        }

        public async Task<bool> Delete(long id)
        {
            var mision = await _context.Misiones.FindAsync(id);
            if (mision == null)
            {
                return false;
            }

            _context.Misiones.Remove(mision);
            await _context.SaveChangesAsync();

            return true;
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

}
