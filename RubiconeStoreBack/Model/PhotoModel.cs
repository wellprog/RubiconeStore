using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubiconeStoreBack.Model
{
    public class PhotoModel<T> where T : class
    {
        public byte[] Photo { get; set; }
        public string PhotoType { get; set; }
        public string FileName { get; set; }
        public T Content { get; set; }
    }
}
