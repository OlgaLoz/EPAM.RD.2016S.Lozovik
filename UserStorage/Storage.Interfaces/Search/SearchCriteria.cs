using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace Storage.Interfaces.Search
{
    [DataContract]
    public abstract class SearchCriteria<T>
    {
        [DataMember]
        public T Entity { get; set; }

        public abstract bool Compare(T entityToCompare);
    }
}