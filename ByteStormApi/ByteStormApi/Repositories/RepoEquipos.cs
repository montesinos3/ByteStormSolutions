using ByteStormApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByteStormApi.Repositories
{
    public interface IEquipoRepository
    {
        Task<EquipoDTO> Insert(EquipoDTO equipoDTO);
        Task<EquipoDTO?> GetById(long id);
        Task<ActionResult<IEnumerable<EquipoDTO>>> GetAllAsync();
        Task<string> Update(EquipoDTO equipoDTO, long id);
        Task<bool> Delete(long id);
    }

    public class RepoEquipos : IEquipoRepository
    {
        private readonly ByteStormContext _context;
        public RepoEquipos(ByteStormContext context)
        {
            _context = context;
        }

        public async Task<EquipoDTO> Insert(EquipoDTO equipoDTO)
        {
            var equipo = new Equipo
            {
                Descripcion = equipoDTO.Descripcion,
                Tipo = equipoDTO.Tipo,
                IdMision = equipoDTO.IdMision
            };
            if (equipoDTO.Estado != null)
            {
                if (equipoDTO.Estado != Estado.EnUso && equipoDTO.Estado != Estado.Disponible)
                {
                    return null;
                }
            }
            equipo.Estado = equipoDTO.Estado;
            if (equipoDTO.Tipo != null)
            {
                if (equipoDTO.Tipo != Tipo.Software && equipoDTO.Tipo != Tipo.Hardware)
                {
                    return null;
                }
            }
            equipo.Tipo = equipoDTO.Tipo;

            _context.Equipos.Add(equipo);
            await _context.SaveChangesAsync();

            return ItemToDTO(equipo);
        }

        public virtual async Task<EquipoDTO?> GetById(long id){
            var equipo = await _context.Equipos.FindAsync(id);

            if (equipo == null)
            {
                return null;
            }

            return ItemToDTO(equipo);
        }


        public async Task<ActionResult<IEnumerable<EquipoDTO>>> GetAllAsync() => await _context.Equipos
                    .Select(x => ItemToDTO(x))
                    .ToListAsync();

        public async Task<string> Update(EquipoDTO equipoDTO, long id)
        {
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return "notfound";
            }

            if (equipoDTO.Descripcion != null)
                equipo.Descripcion = equipoDTO.Descripcion;
            if (equipoDTO.Estado != null)
            {
                if (equipoDTO.Estado == Estado.EnUso || equipoDTO.Estado == Estado.Disponible)
                {
                    equipo.Estado = equipoDTO.Estado;
                }
                else
                {
                    return "badRequest";
                }
            }
            if (equipoDTO.Tipo != null)
            {
                if (equipoDTO.Tipo == Tipo.Software || equipoDTO.Tipo == Tipo.Hardware)
                {
                    equipo.Tipo = equipoDTO.Tipo;
                }
                else
                {
                    return "badRequest";
                }
            }
            if (equipoDTO.IdMision != null)
                equipo.IdMision = equipoDTO.IdMision;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!EquipoExists(id))
            {
                return "notfound";
            }

            return "noContent";
        }

        public async Task<bool> Delete(long id)
        {
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo == null)
            {
                return false;
            }

            _context.Equipos.Remove(equipo);
            await _context.SaveChangesAsync();

            return true;
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
        private bool EquipoExists(long id)
        {
            return _context.Equipos.Any(o => o.Id == id);
        }
    }

}
