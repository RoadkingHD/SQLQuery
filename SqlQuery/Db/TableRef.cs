using System;

namespace SqlQuery.Db
{
    public class TableRef
    {
        public string Schema { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public string FullName
        {
            get { return string.IsNullOrEmpty(Schema) ? Name : Schema + "." + Name; }
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
