using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{

    public List<Node> nodes;

    public Graph() 
    {
        nodes = new List<Node>();
    }

    public Graph(int nodes)
    {
        this.nodes = new List<Node>();

        for (int i = 0; i < nodes; i++)
        {
            this.nodes.Add(new Node((char)(65 + i) + ""));
        }
    }

    public Node this[int index]
    {
        get => nodes[index];
    }

    public Node AddNode(Node node)
    {
        nodes.Add(node);

        return node;
    }

    public void RemoveNode(Node node)
    {

        foreach(Node n in nodes)
            if (n.parents.Contains(node))
                n.parents.Remove(node);

        nodes.Remove(node);
    }

    public void ConnectNodes(Node n1, Node n2)
    {
        n1.children.Add(n2);
        n2.parents.Add(n1);

        n2.children.Add(n1);
        n1.parents.Add(n2);
    }


    //Algorithms

    public List<Node> BFS(Node start)
    {

        List<Node> order = new List<Node>();

        Dictionary<Node, bool> marked = new Dictionary<Node, bool>();
        foreach (Node n in nodes)
            marked.Add(n, false);

        Queue<Node> toVisit = new Queue<Node>();
        toVisit.Enqueue(start);

        while (toVisit.Count > 0)
        {
            Node v = toVisit.Dequeue();

            if (marked[v] == false)
            {
                //order
                order.Add(v);

                marked[v] = true;
                foreach (Node w in v.children)
                {
                    if (marked[w] == false)
                        toVisit.Enqueue(w);
                }
            }
        }

        return order;

    }

    public IEnumerator VisualBFSRoutine(Node start)
    {

        List<Node> order = new List<Node>();

        Dictionary<Node, bool> marked = new Dictionary<Node, bool>();
        foreach (Node n in nodes)
            marked.Add(n, false);

        Queue<Node> toVisit = new Queue<Node>();
        toVisit.Enqueue(start);

        while (toVisit.Count > 0)
        {
            Node v = toVisit.Dequeue();

            if (marked[v] == false)
            {
                v.visualization.Chime(Color.green);
                yield return new WaitForSeconds(0.7f);

                //order
                order.Add(v);
                marked[v] = true;

                //this extra for loop is exclusively to play edge animations in sequence as an additional step
                foreach (Node w in v.children)
                {
                    if(marked[w] == false)
                    {
                        foreach (KeyValuePair<(Node, Node), (Edge, NodeEdgeRole)> e in w.edges)
                        {
                            if (w.edges.ContainsKey((v, w)))
                            {
                                w.edges[(v, w)].Edge.visualization.ChangeColor(Color.yellow);
                                w.edges[(v, w)].Edge.visualization.AnimateForward(0.5f);
                            }
                            if (w.edges.ContainsKey((w, v)))
                            {
                                w.edges[(w, v)].Edge.visualization.ChangeColor(Color.yellow);
                                w.edges[(w, v)].Edge.visualization.AnimateBackward(0.5f);
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(0.5f);


                //and now we go to the original
                foreach (Node w in v.children)
                {

                    if (marked[w] == false)
                    {
                        w.visualization.Chime(Color.yellow);
                        toVisit.Enqueue(w);
                    }
                }

            }
            yield return new WaitForSeconds(2f);

            foreach (Node w in v.children)
            {
                foreach (KeyValuePair<(Node, Node), (Edge, NodeEdgeRole)> e in w.edges)
                {
                    if (w.edges.ContainsKey((v, w)))
                        w.edges[(v, w)].Edge.visualization.ChangeColor(w.edges[(v, w)].Edge.visualization.originalColor);
                    if (w.edges.ContainsKey((w, v)))
                        w.edges[(w, v)].Edge.visualization.ChangeColor(w.edges[(w, v)].Edge.visualization.originalColor);
                }
            }
        }

    }

    public List<Node> ShortestPath(Node start, Node end)
    {
        List<Node> shortestPath = new List<Node>();


        Dictionary<Node, bool> marked = new Dictionary<Node, bool>();
        Dictionary<Node, Node> parents = new Dictionary<Node, Node>();

        foreach (Node n in nodes)
            marked.Add(n, false);

        Queue<Node> toVisit = new Queue<Node>();
        toVisit.Enqueue(start);

        while (toVisit.Count > 0)
        {
            Node v = toVisit.Dequeue();

            if (marked[v] == false)
            {
                marked[v] = true;
                foreach (Node w in v.children)
                {
                    if (marked[w] == false)
                    {
                        toVisit.Enqueue(w);
                        if(!parents.ContainsKey(w))
                            parents.Add(w, v);
                    }
                }
            }
        }

        Node current = end;
        shortestPath.Add(end);
        while(current != start)
        {
            Node parent;
            parents.TryGetValue(current, out parent);

            if(parent == null && current != start)
            {
                Debug.LogWarning("end does not connect to start! Null output");
                shortestPath = null;
                break;
            }

            shortestPath.Add(parent);
            current = parent;
        }
        shortestPath.Reverse();

        Debug.Log(shortestPath.ToString());

        return shortestPath;
    }

    public IEnumerator VisualShortestPathRoutine(Node start, Node end)
    {
        List<Node> shortestPath = new List<Node>();


        Dictionary<Node, bool> marked = new Dictionary<Node, bool>();
        Dictionary<Node, Node> parents = new Dictionary<Node, Node>();

        foreach (Node n in nodes)
            marked.Add(n, false);

        Queue<Node> toVisit = new Queue<Node>();
        toVisit.Enqueue(start);

        while (toVisit.Count > 0)
        {
            Node v = toVisit.Dequeue();

            if (marked[v] == false)
            {
                v.visualization.Chime(Color.green);
                yield return new WaitForSeconds(0.7f);

                marked[v] = true;

                //this extra for loop is exclusively to play edge animations in sequence as an additional step
                foreach (Node w in v.children)
                {
                    if (marked[w] == false)
                    {
                        foreach (KeyValuePair<(Node, Node), (Edge, NodeEdgeRole)> e in w.edges)
                        {
                            if (w.edges.ContainsKey((v, w)))
                            {
                                w.edges[(v, w)].Edge.visualization.ChangeColor(Color.yellow);
                                w.edges[(v, w)].Edge.visualization.AnimateForward(0.5f);
                                break;
                            }
                            if (w.edges.ContainsKey((w, v)))
                            {
                                w.edges[(w, v)].Edge.visualization.ChangeColor(Color.yellow);
                                w.edges[(w, v)].Edge.visualization.AnimateBackward(0.5f);
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(0.5f);

                foreach (Node w in v.children)
                {
                    if (marked[w] == false)
                    {
                        w.visualization.Chime(Color.yellow);
                        toVisit.Enqueue(w);
                        if (!parents.ContainsKey(w))
                            parents.Add(w, v);
                    }
                }
                yield return new WaitForSeconds(2f);

                //reset line colors
                foreach (Node w in v.children)
                {
                    foreach (KeyValuePair<(Node, Node), (Edge, NodeEdgeRole)> e in w.edges)
                    {
                        if (w.edges.ContainsKey((v, w)))
                            w.edges[(v, w)].Edge.visualization.ChangeColor(w.edges[(v, w)].Edge.visualization.originalColor);
                        if (w.edges.ContainsKey((w, v)))
                            w.edges[(w, v)].Edge.visualization.ChangeColor(w.edges[(w, v)].Edge.visualization.originalColor);
                    }
                }

            }
        }

        Node current = end;
        shortestPath.Add(end);
        while (current != start)
        {
            Node parent;
            parents.TryGetValue(current, out parent);

            if (parent == null && current != start)
            {
                Debug.LogWarning("end does not connect to start! Null output");
                shortestPath = null;
                break;
            }

            shortestPath.Add(parent);

            current = parent;
        }
        shortestPath.Reverse();

        foreach(Node n in shortestPath)
        {
            n.visualization.Chime(Color.red);
            yield return new WaitForSeconds(0.7f);
            if(shortestPath.IndexOf(n) != shortestPath.Count - 1 && n.edges.ContainsKey((n, shortestPath[shortestPath.IndexOf(n) + 1])))
            {
                n.edges[(n, shortestPath[shortestPath.IndexOf(n) + 1])].Edge.visualization.ChangeColor(Color.red);
                n.edges[(n, shortestPath[shortestPath.IndexOf(n) + 1])].Edge.visualization.AnimateForward(0.5f);
            }
            yield return new WaitForSeconds(0.5f);

        }

        //return shortestPath;
    }

}
