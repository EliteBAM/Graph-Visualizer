using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphVisualizationController : MonoBehaviour
{
    //Singleton
    public static GraphVisualizationController instance;

    public Graph graph;

    [Header("Visualization Prefabs")]
    // to be instantiated
    public GameObject nodeVisualizer;
    public GameObject edgeVisualizer;

    //private variables


    private void Awake()
    {
        //singleton pattern (did I implement this right? I feel like other people's are more complicated. oh well.
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    void Start()
    {

        graph = new Graph();

    }


    private void Update()
    {
        
        //User Inputs


        //place node
        if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift)) //left shift + click
        {
            AddNode();
        }
        else if(Input.GetMouseButtonDown(0)) // draw edge
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {

                VisualNode selectedNode = hit.transform.gameObject.GetComponent<VisualNode>();

                if (selectedNode != null) // check if you hit a node{
                {
                    BeginEdge(selectedNode);
                }
            }
        }

        //chimetest
        if (Input.GetKeyDown(KeyCode.B) && Input.GetKey(KeyCode.LeftShift)) //left shift + click
        {
            Debug.Log("CHIME!");
            graph[0].visualization.Chime(Color.white);
        }

        //activate visual bfs
        else if (Input.GetKeyDown(KeyCode.B))
        {
            StartVisualBFS();
        }

        //activate visual shortest path
        if(Input.GetKeyDown(KeyCode.S))
        {
            //graph.ShortestPath(graph[0], graph[graph.nodes.Count - 1]);
            StartVisualShortestPath();
        }
    }

    public void StartVisualBFS()
    {
        StartCoroutine(graph.VisualBFSRoutine(graph[0]));
    }


    public void StartVisualShortestPath()
    {
        StartCoroutine(graph.VisualShortestPathRoutine(graph[0], graph[graph.nodes.Count - 1]));
    }

    public void AddNode()
    {
        //get position of mouse
        Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        //instantiate new Visual Node at mouse position
        GameObject newNodeVisualization = Instantiate(nodeVisualizer, position, Quaternion.identity);
        //get Visual Node script from new node for initialization
        VisualNode newNodeVisualizationScript = newNodeVisualization.GetComponent<VisualNode>();

        //Create node, attach the visualization we created
        Node newNode = new Node(graph.nodes.Count.ToString(), newNodeVisualizationScript);

        //add Node to graph with visual node component
        graph.AddNode(newNode);
    }

    public void BeginEdge(VisualNode startingNode)
    {
        GameObject newEdgeVisualization = Instantiate(edgeVisualizer, startingNode.transform.position, Quaternion.identity);
        newEdgeVisualization.GetComponent<VisualEdge>().InitializeEdgeVisualization(startingNode, EdgeType.Temp);
    }

}
