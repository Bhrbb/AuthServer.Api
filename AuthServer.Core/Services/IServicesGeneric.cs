using AuthServer.Core.Dto;
using SharedLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IServicesGeneric<TEntity,TDto> where TEntity:class where TDto:class
    {
        Task<Response<TDto>> GetByIdAsync(int id);
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IQueryable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);//Eğer gelen data üzerinde where sartı yazıp devam ediceksen 
        Task<Response<TDto>>AddAsync(TDto entity);

        Task<Response<NoDataDto>> Remove(int id);
        Task<Response<NoDataDto>> Update(TDto entity, int id);




    }
}
