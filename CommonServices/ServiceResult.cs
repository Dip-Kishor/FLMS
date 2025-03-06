using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices
{
    public class ServiceResult<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public ResultStatus Status { get; set; }
        public int TotalCount { get; set; }
    }
    public enum ResultStatus
    {
        processError,
        dataBaseError,
        ComError,
        unHandeledError,
        Ok,
        InvalidToken
    }
}
