using AuthServer.Core.Dto;
using AuthServer.Core.Reposityory;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    //Veritabanıyla iletişim yapıcak,savechanges method,
    //IGenericServiceden gelenleri implemete ediyoruz 
    public class GenericService<TEntity, TDto> : IServicesGeneric<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepostory<TEntity> _genericRepository;
        public GenericService(IUnitOfWork unitOfWork, IGenericRepostory<TEntity> genericRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
           var newEntity=ObjectMapper.Mapper.Map<TEntity>(entity);
            //bize gelen TDo yu TEntity e donusturduk
            await _genericRepository.AddAsync(newEntity);//hepsini topla ekle 
            await _unitOfWork.CommitAsync();//veritabanına yansıt
            var newDto=ObjectMapper.Mapper.Map<TDto>(newEntity);//tekrar Dto ya donsttur clientin anlayacagı tip
            return Response<TDto>.Succes(newDto, 200);

        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
           var product =ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());
            //IEnumarable dondugunden dolayı product.Where gibi baska işlem yaparsan memoryden cekıcek 
            return Response<IEnumerable<TDto>>.Succes(product,200);

        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var product= await _genericRepository.GetByIdAsync(id);
            if (product==null)
            {
                return Response<TDto>.Fail("ID not found", 404, true);

            }
            return Response<TDto>.Succes(ObjectMapper.Mapper.Map<TDto>(product), 200);
            //GetByAsync TDo istedii için teklrar mapleyip Dto ya dondurduk 

        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {
            var isExistEntity = await _genericRepository.GetByIdAsync(id);
            if (isExistEntity==null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);

            }
            _genericRepository.Remove(isExistEntity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Succes(200);

        }

        public async Task<Response<NoDataDto>> Update(TDto entity,int id)
        {
            var isExistEntity = await _genericRepository.GetByIdAsync(id);
            if (isExistEntity == null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);

            }
            var updateEntity=ObjectMapper.Mapper.Map<TEntity>(entity);
            _genericRepository.Update(updateEntity);
            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Succes(204);
            //no COntent

        }

        public async Task<Response<IQueryable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
           var list=_genericRepository.Where(predicate);//daha veri tabanına yansımadı iş yapabilirsin 
            list.Take(5);
            //list.ToListAsync();//şimdi yansıdı

            return Response<IQueryable<TDto>>.Succes(ObjectMapper.Mapper.Map<IQueryable<TDto>>(await list.ToListAsync()), 200);
        }

       
    }
}
