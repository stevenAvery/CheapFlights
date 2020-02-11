using System.Collections.Generic;

namespace CheapFlights.Models {
    // 
    /// <summary>
    /// Model for DataTables server-side processing input
    /// Source documentation: https://www.datatables.net/manual/server-side#Sent-parameters 
    /// </summary>
    public class DataTablesParamsModel {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SearchValue { get; set; }
        public int OrderColumn { get; set; }
        public string OrderDirection { get; set; }
    }
}