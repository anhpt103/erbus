using System.Runtime.Serialization;
namespace ERBus.Service
{
    [DataContract]
    public class ChoiceObject
    {
        [DataMember]
        public string PARENT { get; set; }
        [DataMember]
        public string DESCRIPTION { get; set; }
        [DataMember]
        public bool OLD_SELECTED { get; set; }
        [DataMember]
        public string REFERENCE_DATA_ID { get; set; }
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string VALUE { get; set; }
        [DataMember]
        public decimal GIATRI { get; set; }
        [DataMember]
        public string EXTEND_VALUE { get; set; }
        [DataMember]
        public string TEXT { get; set; }
        [DataMember]
        public bool SELECTED { get; set; }
        [DataMember]
        public string INFOMATION { get; set; }
    }
}
