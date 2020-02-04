using System;
using System.Collections.Generic;
using System.Linq;
using CheapFlights.Models;

namespace CheapFlights.Helpers {
    public static class GraphHelpers {
        /// <summary>
        /// Converts list of edges (specifically flights) into an adjacency list.`
        /// </summary>
        /// <param name="edgeList">List of graph (specifically flights).</param>
        /// <returns>Adjacency list of Iatacodes, and distances.</returns>
        public static Dictionary<string, List<(string, decimal)>> ToAdjacencyList(
            this IEnumerable<FlightModel> edgeList) {

            var adjList = new Dictionary<string, List<(string, decimal)>>();

            var originVertices = edgeList.Select(edge => edge.Origin.IataCode);
            var destinationVertices = edgeList.Select(edge => edge.Destination.IataCode);
            var distinctVertices = originVertices.Concat(destinationVertices).Distinct();

            foreach (var vertex in distinctVertices)
                adjList.Add(vertex, new List<(string, decimal)>());

            foreach(var edge in edgeList)
                adjList[edge.Origin.IataCode].Add((edge.Destination.IataCode, edge.Cost));

            return adjList;
        }

        /// <summary>
        /// Finds the shortest path in a weighted graph from origin to destination.
        /// </summary>
        /// <param name="adList">The adjacency list that represents the graph.</param>
        /// <param name="origin">Origin airport iatacode</param>
        /// <param name="destination">Destination airport iatacode</param>
        /// <returns>Shortest path from origin to destination.</returns>
        public static List<FlightModel> ShortestPath(
            this Dictionary<string, List<(string, decimal)>> adjList,
            string origin,
            string destination) {
            if (!adjList.ContainsKey(origin))
                throw new ArgumentException($"{nameof(origin)} not found in {nameof(adjList)}", nameof(origin));
            if (!adjList.ContainsKey(destination))
                throw new ArgumentException($"{nameof(destination)} not found in {nameof(adjList)}", nameof(destination));

            // initialize list of vertices as undiscovered
            var vertices = new Dictionary<string, Vertex>();
            foreach (var key in adjList.Keys)
                vertices.Add(key, new Vertex());

            // we consider the origin vertex as discovered
            vertices[origin] = new Vertex() {
                Colour = Vertex.ColourType.Grey,
                Distance = 0,
                Previous = null
            };

            // maintain a queue of currently discovered verticies
            var discoveredVertices = new Queue<string>();
            discoveredVertices.Enqueue(origin);

            while (discoveredVertices.TryDequeue(out string currentVertex)) {
                foreach (var (adjVertex, adjDistance) in adjList[currentVertex]) {
                    // only update the previous vertex and distance, if this is a shorter path
                    if (vertices[currentVertex].Distance + adjDistance < vertices[adjVertex].Distance) {
                        vertices[adjVertex].Distance = vertices[currentVertex].Distance + adjDistance;
                        vertices[adjVertex].Previous = currentVertex;
                    }

                    if (vertices[adjVertex].Colour == Vertex.ColourType.White) {
                        vertices[adjVertex].Colour = Vertex.ColourType.Grey;
                        discoveredVertices.Enqueue(adjVertex);
                    }
                }
                vertices[currentVertex].Colour = Vertex.ColourType.Black;
            }

            // get specific path from origin to destination
            var result = new List<FlightModel>();
            var vertex = destination;
            while (vertex != origin) {
                var previous = vertices[vertex].Previous;
                var distance = adjList[previous].Where(v => v.Item1 == vertex).First().Item2;
                result.Add(new FlightModel() {
                    Origin = new AirportModel() { IataCode = previous },
                    Destination = new AirportModel() { IataCode = vertex },
                    Cost = distance
                });
                vertex = previous;
            }
            result.Reverse();

            return result;
        }

        private class Vertex {
            public enum ColourType {
                White, Grey, Black
            }

            public ColourType Colour { get; set; } = ColourType.White;
            public decimal Distance { get; set; } = decimal.MaxValue; // default to infinity
            public string Previous { get; set; } = null;
        }
    }
}