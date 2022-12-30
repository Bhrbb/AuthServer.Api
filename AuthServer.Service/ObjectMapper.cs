using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service
{
    public static class ObjectMapper
    {
        //static herhangi bir nesne olusturulmadn önce cagırabilirsin 
        ///bir nesne metodun ureteceği sonucu etkileyemecek ise 
        ///gec yuklenme özelliği  yapıyor lazy
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMapper>();
            });
            return config.CreateMapper();
        });
        public static IMapper Mapper=> lazy.Value;
        //burada ObjectMapper.Mapper diye cagırmadan içerideki kod başlamaz 
        //içerisi de Dtomapper classının ıcerısındekı emirleri yapar


    }
}
