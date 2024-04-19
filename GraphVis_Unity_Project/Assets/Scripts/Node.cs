using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public string name;

    public List<Node> parents;
    public List<Node> children;

    public Dictionary<(Node, Node), (Edge Edge, NodeEdgeRole Role)> edges;

    public VisualNode visualization;

    public Node(string name)
    {
        this.name = name;

        //nodes that are connected to this node: other -> this
        parents = new List<Node>();
        //nodes that this node is connected to: other <- this
        children = new List<Node>();

        edges = new Dictionary<(Node, Node), (Edge Edge, NodeEdgeRole Role)>();
    }

    public Node(string name, VisualNode visualization) //with visualization
    {
        this.name = name;

        this.visualization = visualization;
        visualization.InitializeNodeVisualizer(name, this);

        //nodes that are connected to this node: other -> this
        parents = new List<Node>();
        //nodes that this node is connected to: other <- this
        children = new List<Node>();

        edges = new Dictionary<(Node, Node), (Edge Edge, NodeEdgeRole Role)>();
    }

    public Node(string name, params Node[] connections)
    {
        this.name = name;

        //initialize initial connections
        foreach (Node n in connections)
        {
            children.Add(n);
            n.parents.Add(this);
        }
    }

    public void AddEdge((Node, Node) key, Edge edge, NodeEdgeRole role)
    {
        Debug.Log("Edges");
        Debug.Log(edges.ToString() + " edges");
        edges.Add(key, (edge, role));
    }

    public void AddConnection(Node other)
    {
        children.Add(other);
        other.parents.Add(this);
    }

    public void RemoveConnection(Node other)
    {
        if (children.Contains(other))
            children.Remove(other);
        else
            Debug.LogWarning("Attempted to Disconnect Node " + name + " from Node " + other.name + ", but the connection was not found");
    }

}
