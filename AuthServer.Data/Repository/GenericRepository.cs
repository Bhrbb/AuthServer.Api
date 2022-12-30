using AuthServer.Core.Reposityory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data.Repository
{
    public class GenericRepository<TEntity> : IGenericRepostory<TEntity> where TEntity : class
    {
        private readonly DbContext _context;//veritabanı işlemleri için

        private readonly DbSet<TEntity> _dbset;//veritabanındakı tablolar üzerinde işlemleri için
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbset=context.Set<TEntity>();


        }
        public async Task AddAsync(TEntity entity)
        {
            await _dbset.AddAsync(entity);//memory e entitiy eklendi.COmmit dediğimizde kaydetcek commit diyene kadar bekleyecek 

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
           return await _dbset.ToListAsync();//buradan gelen data da baska ıslem yapmacagımız ıcın IEnumarable yaptık 

        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
           var entity=await _dbset.FindAsync(id);//primarykey uzerınden arama gercekleştirie sql de birdn falza primarykey oolabilir o yzden birden fazla obje koyabılırsın

            if (entity!=null)
            {
                _context.Entry(entity).State = EntityState.Detached;//memoryde tutmasın 
            }
            return entity;

        }

        public void Remove(TEntity entity)
        {
            _dbset.Remove(entity);
        }

        public TEntity Update(TEntity entity)
        {
          _context.Entry(entity).State= EntityState.Modified;
            //genericRepository dezavantajı burada 
            //eğer entıtynın bır tane özelliğini bile değiştirsen entityi guncelliyor buda performans kaybına sebeb olur 
            return entity;
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbset.Where(predicate);//buradan gelen verilere baska bır ıslem daha yapabılırım Tolist diyene kadar ıslem bıtmez 
        }
    }
}
