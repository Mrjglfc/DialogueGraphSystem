// Code taken from TechWebDots, Thanks go to him for open-sourcing this graph implementation.
// Source: https://github.com/TechWebDots/GraphDataStructureInC-Sharp/blob/master/GenericGraph.cs
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Mrjglfc.DialogueGraphSystem.Runtime
{
    [Serializable]
    public class GraphNode<T>
    {
        private T value;
        [SerializeReference] private List<GraphNode<T>> neighbors;

        public GraphNode(T value)
        {
            this.value = value;
            neighbors = new List<GraphNode<T>>();
        }

        public T Value => value;
        public IList<GraphNode<T>> Neighbors => neighbors.AsReadOnly();

        public bool AddNeighbors(GraphNode<T> neighbor)
        {
            if (neighbors.Contains(neighbor))
            {
                return false;
            }
            else
            {
                neighbors.Add(neighbor);
                return true;
            }
        }

        public bool RemoveNeighbors(GraphNode<T> neighbor) => neighbors.Remove(neighbor);

        public bool RemoveAllNeighbors()
        {
            for (int i = neighbors.Count; i >= 0; i--)
            {
                neighbors.RemoveAt(i);
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder nodeString = new();
            nodeString.Append($"[ Node Value {value} with Neighbors : ");
            for (int i = 0; i < neighbors.Count; i++)
            {
                nodeString.Append(neighbors[i].Value + " ");
            }
            nodeString.Append("]");
            return nodeString.ToString();
        }
    }

    enum SearchType
    {
        DFS,
        BFS
    }

    [Serializable]
    public class Graph<T>
    {
        List<GraphNode<T>> nodes = new();

        public Graph()
        {

        }

        public int Count => nodes.Count;
        public IList<GraphNode<T>> Nodes => nodes.AsReadOnly();

        public bool AddNode(T value)
        {
            if (Find(value) == null)
            {
                nodes.Add(new GraphNode<T>(value));
                return true;
            }

            return false;
        }

        public bool AddEdge(T parent, T child)
        {
            GraphNode<T> parentNode = Find(parent);
            GraphNode<T> childNode = Find(child);

            if (parentNode == null || childNode == null)
            {
                return false;
            }
            else if (parentNode.Neighbors.Contains(childNode))
            {
                return false;
            }
            else
            {
                //for directed graph only below 1st line is required  node1->node2
                parentNode.AddNeighbors(childNode);

                //for undirected graph need below line as well
                //node2.AddNeighbors(node1);
                return true;
            }
        }
        public GraphNode<T> Find(T value)
        {
            foreach (GraphNode<T> node in nodes)
            {
                if (node.Value.Equals(value))
                {
                    return node;
                }
            }
            return null;
        }

        public bool RemoveNode(T value)
        {
            GraphNode<T> removeNode = Find(value);

            if (removeNode == null)
            {
                return false;
            }
            else
            {
                nodes.Remove(removeNode);
                foreach (GraphNode<T> node in nodes)
                {
                    node.RemoveNeighbors(removeNode);
                }
                return true;
            }
        }

        public bool RemoveEdge(T value1, T value2)
        {
            GraphNode<T> node1 = Find(value1);
            GraphNode<T> node2 = Find(value2);

            if (node1 == null || node2 == null)
            {
                return false;
            }
            else if (!node1.Neighbors.Contains(node2))
            {
                return false;
            }
            else
            {
                //for direted graph only below 1st line is required  node1->node2
                node1.RemoveNeighbors(node2);
                //for undireted graph need below line as well
                node2.RemoveNeighbors(node1);
                return true;
            }
        }

        public void Clear()
        {
            foreach (GraphNode<T> node in nodes)
            {
                node.RemoveAllNeighbors();
            }

            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                nodes.RemoveAt(i);
            }
        }

        public override string ToString()
        {
            StringBuilder nodeString = new();
            for (int i = 0; i < Count; i++)
            {
                nodeString.Append(nodes[i].ToString());
                if (i < Count - 1)
                {
                    nodeString.Append("\n");
                }
            }
            return nodeString.ToString();
        }

        internal static string PrintGraph(Graph<T> graph) => graph.ToString();

        /// <summary>
        /// search for a path from start to finish on graph using givem type 
        /// </summary>
        /// <param name="start">start value</param>
        /// <param name="finish">finish value</param>
        /// <param name="graph">graph to search</param>
        /// <param name="searchType">DFS/BFS</param>
        /// <returns>string for path or empty string if there is no path</returns>
        internal string Search(int start, int finish, Graph<int> graph, SearchType searchType)
        {
            LinkedList<GraphNode<int>> searchList = new();
            if (start == finish)
            {
                return start.ToString();
            }
            else if (graph.Find(start) == null || graph.Find(finish) == null)
            {
                return "";
            }
            else
            {
                //add start node to the dictionary
                GraphNode<int> startNode = graph.Find(start);
                Dictionary<GraphNode<int>, PathNodeInfo<int>> pathNodes = new()
                {
                    { startNode, new PathNodeInfo<int>(null) }
                };
                searchList.AddFirst(startNode);

                while (searchList.Count > 0)
                {
                    //extract front of search list
                    GraphNode<int> currentNode = searchList.First.Value;
                    searchList.RemoveFirst();

                    //explore each neighbour of this node
                    foreach (GraphNode<int> neighbor in currentNode.Neighbors)
                    {
                        if (neighbor.Value == finish)
                        {
                            pathNodes.Add(neighbor, new PathNodeInfo<int>(currentNode));
                            return "\nFinal Path is " + ConvertPathToString(neighbor, pathNodes);
                        }
                        else if (pathNodes.ContainsKey(neighbor))
                        {
                            //check for cycle, skip this neighbour
                            continue;
                        }
                        else
                        {
                            pathNodes.Add(neighbor, new PathNodeInfo<int>(currentNode));
                            
                            if (searchType == SearchType.DFS)
                            {
                                searchList.AddFirst(neighbor);
                            }
                            else
                            {
                                searchList.AddLast(neighbor);
                            }
                        }
                    }
                }
                //didn't find a path from start to finish
                return "";
            }
        }

        static string ConvertPathToString(GraphNode<int> endNode, Dictionary<GraphNode<int>, PathNodeInfo<int>> pathNodes)
        {
            //build ll for path in the correct order
            LinkedList<GraphNode<int>> path = new();
            path.AddFirst(endNode);
            GraphNode<int> previous = pathNodes[endNode].Previous;
            while (previous != null)
            {
                path.AddFirst(previous);
                previous = pathNodes[previous].Previous;
            }

            //build and return string
            StringBuilder pathString = new();
            LinkedListNode<GraphNode<int>> currentNode = path.First;
            int nodeCount = 0;
            while (currentNode != null)
            {
                nodeCount++;
                pathString.Append(currentNode.Value.Value);
                if (nodeCount < path.Count)
                {
                    pathString.Append(" ");

                }
                currentNode = currentNode.Next;
            }
            return pathString.ToString();
        }

        public class PathNodeInfo<T>
        {
            //Graph: internal previous node variable
            GraphNode<T> previous;
            //Graph: constructor to initialize the previous node
            public PathNodeInfo(GraphNode<T> previous)
            {
                this.previous = previous;
            }
            //Graph: Readonly return previous node prop
            public GraphNode<T> Previous
            {
                get
                {
                    return previous;
                }
            }
        }
    }
}