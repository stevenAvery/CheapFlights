using System;
using System.Collections.Generic;
using System.Linq;

namespace CheapFlights.Helpers {
    public static class GraphHelpers {
        /// <summary>
        /// Node in an Adjacency List.
        /// </summary>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        public class AdjacencyNode<TEdge> {
            public string Destination { get; set; }
            public TEdge Edge { get; set; }
        }

        /// <summary>
        /// Converts list of edges (specifically flights) into an adjacency list.
        /// </summary>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="edges">List of graph edges.</param>
        /// <param name="getFrom">Function to get origin id of a given TEdge.</param>
        /// <param name="getTo">Function to get destination id of a given TEdge.</param>
        /// <returns>Adjacency list representation of graph.</returns>
        public static Dictionary<string, List<AdjacencyNode<TEdge>>> ToAdjacencyList<TEdge>(
            this IEnumerable<TEdge> edges, Func<TEdge, string> getFrom, Func<TEdge, string> getTo) {

            // get list of all distinct vertices
            var fromVertices = edges.Select(edge => getFrom(edge));
            var toVertices   = edges.Select(edge => getTo(edge));
            var distinctVertices = fromVertices.Concat(toVertices).Distinct();

            // initialize adjList, so that every distinct vertex has an empty list of adjacent vertices
            var adjList = new Dictionary<string, List<AdjacencyNode<TEdge>>>();
            foreach (var vertex in distinctVertices)
                adjList.Add(vertex, new List<AdjacencyNode<TEdge>>());

            // add every edge to adjacency list
            foreach(var edge in edges) {
                adjList[getFrom(edge)].Add(new AdjacencyNode<TEdge> () {
                    Destination = getTo(edge),
                    Edge = edge
                });
            }

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
            this Dictionary<string, List<AdjacencyNode<TEdge>>> adjList,
            string source, string target) {

            if (!adjList.ContainsKey(source))
                throw new ArgumentException($"{nameof(source)} not found in {nameof(adjList)}", nameof(source));
            if (!adjList.ContainsKey(target))
                throw new ArgumentException($"{nameof(target)} not found in {nameof(adjList)}", nameof(target));

            // all paths that have been discovered
            var allPaths = new List<List<AdjacencyNode<TEdge>>>();

            // initialize all vertices as not visited
            var visited = new Dictionary<string, bool>();
            foreach(var key in adjList.Keys)
                visited.Add(key, false);

            // maintain a queue of currently discovered verticies
            var discoveredVertices = new Queue<List<AdjacencyNode<TEdge>>>();
            discoveredVertices.Enqueue(new List<AdjacencyNode<TEdge>>() { 
                new AdjacencyNode<TEdge> () {
                    Destination = source,
                    Edge = default(TEdge)
                }
            });

            while (discoveredVertices.TryDequeue(out List<AdjacencyNode<TEdge>> current)) {
                foreach (var adjacent in adjList[current.Last().Destination]) {
                    var path = current.Append(adjacent).ToList();
                    // if we're at the target, add the cuurent path to resulting list of paths
                    if (adjacent.Destination == target)
                        allPaths.Add(path.Skip(1).ToList());
                    // if we've never visited the adjacent vertex, add it to list of discovered vertices
                    else if (!visited[adjacent.Destination])
                        discoveredVertices.Enqueue(path);
                }
                visited[current.Last().Destination] = true;
            }

            return allPaths.Select(path => path.Select(elem => elem.Edge).ToList()).ToList();
        }
    }
}