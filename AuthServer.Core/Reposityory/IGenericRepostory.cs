using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Reposityory
{
    public interface IGenericRepostory<TEntity>where TEntity:class
    {
        //Busines Code cok fazla varsa GenericRepostory olmasa daha iyi


        Task<TEntity> GetByIdAsync(int id);
       Task<IEnumerable<TEntity>> GetAllAsync();//Yani tek sorgu işini görcek yeniden filtre falan yapmıcaksan IEnumarable olur

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>>predicate);//Eğer gelen data üzerinde where sartı yazıp devam ediceksen 
       
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
        TEntity Update(TEntity entity);


    }
}
