using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace ERBus.Service
{
    [Serializable]
    [DataContract]
    public class TransferObj<T>
    {
        public TransferObj()
        {
            ExtData = new List<T>();
            //Message = new MessageObj();
        }
        [DataMember]
        public bool Status { get; set; }
        //[DataMember]
        //public TransferType Type { get; set; }
        [DataMember]
        public T Data { get; set; }
        [DataMember]
        public List<T> ExtData { get; set; }
        //[DataMember]
        //public MessageObj Message { get; set; }
        [DataMember]
        public string Message { get; set; }
    }

    [Serializable]
    [DataContract]
    public class TransferObj : TransferObj<object>
    {

    }
    [DataContract]
    public enum TransferType
    {
        [DataMember]
        Unknow,
        [DataMember]
        Json,
        [DataMember]
        HtmlContent,
        [DataMember]
        Command,
        [DataMember]
        NotifyState
    }
}
