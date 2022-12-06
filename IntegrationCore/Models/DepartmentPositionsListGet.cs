using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationCore.Models
{
    public class DepartmentPositionList
    {
        public string CountofRecords { get; set; }
    }

    public class SearchCls
    {
        public string FieldName { get; set; }
        public string Opertation { get; set; }
        public string Value { get; set; }
    }

    public class DepartmentPositionsReqCls
    {
        public List<SearchCls> SearchList { get; set; }
    }

}
