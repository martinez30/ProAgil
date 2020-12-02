using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.API.Repository;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {

        private readonly ProAgilContext _context;
        public ProAgilRepository(ProAgilContext context)
        {
            this._context = context;    
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Update(entity);        
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

       //Evento
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(x=> x.Lotes)
                .Include(x=> x.RedesSociais);

                if(includePalestrantes)
                    query = query
                    .Include(x=> x.PalestrantesEvento)
                    .ThenInclude(x=> x.Palestrante);

                query = query.AsNoTracking().OrderByDescending(x=> x.DataEvento);

                return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(x=> x.Lotes)
                .Include(x=> x.RedesSociais);

                if(includePalestrantes)
                    query = query
                    .Include(x=> x.PalestrantesEvento)
                    .ThenInclude(x=> x.Palestrante);

                query = query.AsNoTracking().OrderByDescending(x=> x.DataEvento)
                           .Where(x=> x.Tema.ToLower().Contains(tema.ToLower()));

                return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(x=> x.Lotes)
                .Include(x=> x.RedesSociais);

                if(includePalestrantes)
                    query = query
                    .Include(x=> x.PalestrantesEvento)
                    .ThenInclude(x=> x.Palestrante);

                query = query.AsNoTracking().OrderByDescending(x=> x.DataEvento)
                        .Where(x=> x.Id == EventoId);

                return await query.FirstOrDefaultAsync();
        }

       //Palestrante
        public async Task<Palestrante> GetPalestranteAsync(int PalestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(x=> x.RedesSociais);

                if(includeEventos)
                    query = query
                    .Include(x=> x.PalestrantesEvento)
                    .ThenInclude(x=> x.Evento);

                query = query.OrderBy(x=> x.Nome)
                       .Where(x=> x.Id == PalestranteId);

                return await query.AsNoTracking().FirstOrDefaultAsync();
        }

         public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos = false)
        {
             IQueryable<Palestrante> query = _context.Palestrantes
                .Include(x=> x.RedesSociais);

                if(includeEventos)
                    query = query
                    .Include(x=> x.PalestrantesEvento)
                    .ThenInclude(x=> x.Evento);

                query = query.AsNoTracking().Where(x=> x.Nome.ToLower().Contains(name.ToLower()));

                return await query.ToArrayAsync();
        }

    }
}