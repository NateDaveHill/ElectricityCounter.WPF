using System;

namespace _65_WPF_Stromzähler
{
    public class SQLRetrievedDataList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public int CounterId { get; set; }
    }
}