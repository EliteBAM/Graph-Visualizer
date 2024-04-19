using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{

    public Node a;
    public Node b;

    public VisualEdge visualization;

    public Edge(Node a, Node b, VisualEdge visualization)
    {
        this.a = a;
        this.b = b;

        this.visualization = visualization;
    }

}
