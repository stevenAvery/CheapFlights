using System.Collections.Generic;

namespace CheapFlights.Models {
    /// <summary>
    /// Model for DataTables server-side processing result
    /// Source documentation: https://www.datatables.net/manual/server-side#Returned-data
    /// </summary>
    public class DataTablesResultModel<T> {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}