using System;
using System.Collections.Generic;
using System.Linq;

namespace CheapFlights.Helpers {
    public static class GraphHelpers {
        /// <summary>
        /// Converts list of edges (specifically flights) into an adjacency list.
        /// </summary>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="edges">List of graph edges.</param>
        /// <param name="getFrom">Function to get origin id of a given TEdge.</param>
        /// <param name="getTo">Function to get destination id of a given TEdge.</param>
        /// <returns>Adjacency list representation of graph.</returns>
        public static Dictionary<string, List<(string, TEdge)>> ToAdjacencyList<TEdge>(
            this List<TEdge> edges, Func<TEdge, string> getFrom, Func<TEdge, string> getTo) {

            // get list of all distinct vertices
            var fromVertices = edges.Select(edge => getFrom(edge));
            var toVertices   = edges.Select(edge => getTo(edge));
            var distinctVertices = fromVertices.Concat(toVertices).Distinct();

            // initialize adjList, so that every distinct vertex has an empty list of adjacent vertices
            var adjList = new Dictionary<string, List<(string, TEdge)>>();
            foreach (var vertex in distinctVertices)
                adjList.Add(vertex, new List<(string, TEdge)>());

            // add every edge to adjacency list
            foreach(var edge in edges)
                adjList[getFrom(edge)].Add((getTo(edge), edge));

            return adjList;
        }

        /// <summary>
        /// Find all paths from the source to the target.
        /// </summary>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="adjList">Adjacency list representation of graph.</param>
        /// <param name="source">Id of the source vertex.</param>
        /// <param name="target">Id of the target vertex.</param>
        /// <returns>All paths from source to target.</returns>
        public static List<List<TEdge>> AllPaths<TEdge>(
            this Dictionary<string, List<(string, TEdge)>> adjList,
            string source, string target) {

            if (!adjList.ContainsKey(source))
                throw new ArgumentException($"{nameof(source)} not found in {nameof(adjList)}", nameof(source));
            if (!adjList.ContainsKey(target))
                throw new ArgumentException($"{nameof(target)} not found in {nameof(adjList)}", nameof(target));

            // all paths that have been discovered
            var allPaths = new List<List<(string, TEdge)>>();

            // initialize all vertices as not visited
            var visited = new Dictionary<string, bool>();
            foreach(var key in adjList.Keys)
                visited.Add(key, false);

            // maintain a queue of currently discovered verticies
            var discoveredVertices = new Queue<List<(string, TEdge)>>();
            discoveredVertices.Enqueue(new List<(string, TEdge)>() { (source, default(TEdge)) });

            while (discoveredVertices.TryDequeue(out List<(string, TEdge)> current)) {
                foreach (var adjacent in adjList[current.Last().Item1]) {
                    var path = current.Append(adjacent).ToList();
                    // if we're at the target, add the cuurent path to resulting list of paths
                    if (adjacent.Item1 == target)
                        allPaths.Add(path.Skip(1).ToList());
                    // if we've never visited the adjacent vertex, add it to list of discovered vertices
                    if (!visited[adjacent.Item1])
                        discoveredVertices.Enqueue(path);
                }
                visited[current.Last().Item1] = true;
            }

            return allPaths.Select(path => path.Select(elem => elem.Item2).ToList()).ToList();
        }
    }
}