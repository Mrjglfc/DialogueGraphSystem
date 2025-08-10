// Code taken from TechWebDots, Thanks go to him for open-sourcing this graph implementation.
// Source: https://github.com/TechWebDots/GraphDataStructureInC-Sharp/blob/master/GenericGraph.cs
using System;
using System.Collections.Generic;
using System.Text;

namespace Mrjglfc.DialogueGraphSystem.Runtime
{
    #region GraphNode: Create Node Structure
    [Serializable]
    public class GraphNode<T>
    {
        #region Internal Variable - generic value and List of neighbor nodes       
        T value;
        List<GraphNode<T>> neighbors;
        #endregion nodes

        #region Constructor- initialize value and blank neighbour list
        public GraphNode(T value)
        {
            this.value = value;
            neighbors = new List<GraphNode<T>>();
        }
        #endregion

        #region Readonly Properties - value and List of neighbor nodes              
        public T Value
        {
            get { return value; }
        }
        public IList<GraphNode<T>> Neighbors
        {
            get { return neighbors.AsReadOnly(); }
        }
        #endregion

        #region Basic Operations-AddNeighbors, RemoveNeighbors, RemoveAllNeighbors, ToString
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
        public bool RemoveNeighbors(GraphNode<T> neighbor)
        {
            return neighbors.Remove(neighbor);
        }
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
        #endregion
    }
    #endregion

    #region Graph: Search Type Enum        
    enum SearchType
    {
        DFS,
        BFS
    }
    #endregion

    #region Graph: Create Undirected  Structure, Build, Search and Get the path
    [Serializable]
    public class Graph<T>
    {
        #region Graph: Internal Variable - list of nodes        
        public List<GraphNode<T>> nodes = new();
        #endregion

        #region Graph: constructor
        public Graph()
        {

        }
        #endregion

        #region Graph: Readonly Properties - Count, Nodes        
        public int Count
        {
            get
            {
                return nodes.Count;
            }
        }
        public IList<GraphNode<T>> Nodes
        {
            get
            {
                return nodes.AsReadOnly();
            }
        }
        #endregion

        #region Graph: Basic operations - AddNode, AddEdge, RemoveNode, RemoveEdge, Clear, ToString, Find                 
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
        #endregion

        #region Graph: Create with Edges        
        internal static Graph<int> BuidGraph()
        {
            Graph<int> graph = new();
            graph.AddNode(1);
            graph.AddNode(4);
            graph.AddNode(5);
            graph.AddNode(7);
            graph.AddNode(10);
            graph.AddNode(11);
            graph.AddNode(12);
            graph.AddNode(42);

            graph.AddEdge(1, 5);
            graph.AddEdge(4, 11);
            graph.AddEdge(4, 42);
            graph.AddEdge(5, 11);
            graph.AddEdge(5, 12);
            graph.AddEdge(5, 42);
            graph.AddEdge(7, 10);
            graph.AddEdge(7, 11);
            graph.AddEdge(10, 11);
            graph.AddEdge(11, 42);
            graph.AddEdge(12, 42);
            return graph;
        }
        #endregion

        #region Graph: Print
        internal static string PrintGraph(Graph<T> graph)
        {
            return graph.ToString();
        }
        #endregion

        #region Graph: Search the path between nodes        
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
                            return "\nFinal Path is " + CovertPathToString(neighbor, pathNodes);
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
        #endregion

        #region Graph: Convert the path into string        
        static string CovertPathToString(GraphNode<int> endNode, Dictionary<GraphNode<int>, PathNodeInfo<int>> pathNodes)
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
        #endregion

        #region Graph: Previous node setup
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
        #endregion
    }
    #endregion
}