using UnityEngine;

[RequireComponent(typeof(LineRenderer)), RequireComponent(typeof(EdgeAnimator))]
public class VisualEdge : MonoBehaviour
{

    EdgeType edgeType;

    LineRenderer lineRenderer;
    EdgeAnimator animator;

    Edge edge;

    public Color originalColor;

    public void InitializeEdgeVisualization(VisualNode startingNode, EdgeType edgeType)
    {

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        
        animator = GetComponent<EdgeAnimator>();

        this.edgeType = edgeType;

        edge = new Edge(startingNode.node, null, this);

        lineRenderer.SetPosition(0, edge.a.visualization.transform.position);
        lineRenderer.SetPosition(1, edge.a.visualization.transform.position);

        originalColor = lineRenderer.material.color;

    }

    public void ChangeColor(Color color)
    {
        lineRenderer.material.color = color;
    }

    public void AnimateForward(float duration)
    {
        StartCoroutine(animator.StartToEndAnimationRoutine(duration));
    }

    public void AnimateBackward(float duration)
    {
        StartCoroutine(animator.EndToStartAnimationRoutine(duration));
    }

    // Update is called once per frame
    void Update()
    {
        

        if(edgeType == EdgeType.Temp)
        {

            if(Input.GetMouseButtonUp(0))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {

                    VisualNode selectedNodeVisualization = hit.transform.gameObject.GetComponent<VisualNode>();

                    if (selectedNodeVisualization != null && selectedNodeVisualization != edge.a.visualization) // check if you hit a node
                    {
                        edge.b = selectedNodeVisualization.node;

                        lineRenderer.SetPosition(1, edge.b.visualization.transform.position);

                        //FORM CONNECTION ON INTERNAL GRAPH (BIDIRECTION HARDCODED FOR NOW)
                        edgeType = EdgeType.Bidirectional;
                        edge.a.AddConnection(edge.b);
                        edge.b.AddConnection(edge.a);

                        //Add edge to nodes
                        edge.a.AddEdge((edge.a, edge.b), edge, NodeEdgeRole.Originator);
                        edge.b.AddEdge((edge.a, edge.b), edge, NodeEdgeRole.Destination);
                    }
                    else
                    {
                        Debug.Log("Destroy Edge");
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Debug.Log("Destroy Edge");
                    Destroy(gameObject);
                }
            }
            else
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                lineRenderer.SetPosition(1, position);
            }

        }


    }
}
