using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dto
{
    //her istek yapıldığında buraya gelip basarılı basarısız durumlarına bakarsın
    public class Response<T> where T : class
    {
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
        public ErrorDto Error { get; private set; }
        [JsonIgnore]//yoksaymak ıcın 
        public bool IsSuccesful { get; private set; }//clientler gormucek //kısayoldan basarılı olup olmadıgını gormek ıcın 
        public static Response<T> Succes(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode,IsSuccesful=true };
        }
        public static Response<T>Succes (int statusCode)//update yada delete de data donmeye gerek yok 
        {
            return new Response<T> { StatusCode = statusCode ,IsSuccesful = true };
        }
        public static Response<T> Fail(ErrorDto errorDto, int statusCode)//başarısız durumunda errorDto da nesne olusturuuyor statuscodu alıyor
        {
            return new Response<T> { Error = errorDto, StatusCode = statusCode, IsSuccesful = false };
          //errordtonun ıcerisinde ne varsa burada var   
        }
        public static Response<T> Fail(string errorMessage,int statusCode,bool isShow)//başarısız durumunda tek bır hata varsa direk mesajı vermek için 
        {
            var errorDto = new ErrorDto(errorMessage, isShow);
            return new Response<T> { Error=errorDto, StatusCode = statusCode,IsSuccesful=false };
            //errordto ya girmediği için ne istyorsak direk söyleriz

        }

    }
}
