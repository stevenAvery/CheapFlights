using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CheapFlights.Helpers {
    public static class ServerSideProcessingHelper {
        private static CultureInfo _EnCulture = new CultureInfo("en-CA", false);

        /// <summary>
        /// Search filter to remove all items that don't contain the needle.
        /// </summary>
        public static IEnumerable<T> SearchFilter<T>(this IEnumerable<T> haystack, string needle) {
            IEnumerable<T> filteredHaystack;

            if (string.IsNullOrWhiteSpace(needle))
                filteredHaystack = haystack;
            else
                filteredHaystack = haystack.Where(item => 
                    _EnCulture.CompareInfo.IndexOf(item.ToString(), needle, CompareOptions.IgnoreCase) >= 0);

            return filteredHaystack;
        }

        /// <summary>
        /// Order data set by given column id.
        /// </summary>
        public static IEnumerable<T> OrderByColumn<T, TKey>(
            this IEnumerable<T> data, 
            Dictionary<int, Func<T, TKey>> columnLookup, int column, bool isAsc = true) {

            if (columnLookup.ContainsKey(column)) {
                data = data.OrderBy(columnLookup[column]);
                if (!isAsc) {
                    data = data.Reverse();
                }
            }
            
            return data;
        }

        /// <summary>
        /// Paginate given data set by start and length.
        /// </summary>
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> data, int start, int length) {
            return data.Skip(start).Take(length);
        }
    }
}