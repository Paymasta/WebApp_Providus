using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel
{
    [Serializable]
    public class Response<T>
    {
        public bool IsSuccess;
        public string Message;
        public List<Errorkey> ErrorList;
        public T Result;

        public Response<T> Create(bool success, string message, List<Errorkey> errorList, T result)
        {
            Response<T> response = new Response<T>();
            response.IsSuccess = success;
            response.Message = message;
            response.ErrorList = errorList;
            response.Result = result;
            return response;
        }
    }

    public class Errorkey
    {
        public Errorkey() { }
        public Errorkey(string _key, string _value)
        {
            Key = _key;
            Val = _value;
        }

        public string Key { get; set; }
        public string Val { get; set; }

    }
    public static class GlobalData
    {
        public static string Key { get; set; }
        public static int RoleId { get; set; }
        public static int AppVersion { get; set; }
        public static int AppId { get; set; }
    }
}
