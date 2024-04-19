using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VisualNode : MonoBehaviour
{
    [Header("Particle Systems")]
    public ParticleSystem node_chime;

    TMP_Text visualName;

    public Node node;

    public void Chime(Color color)
    {   
        node_chime.GetComponent<ParticleSystemRenderer>().material.color = color;
        node_chime.Play();
    }

    public void InitializeNodeVisualizer(string name, Node node)
    {
        visualName = GetComponentInChildren<TMP_Text>();
        visualName.text = name;

        //adjust text size to fit in canvas with increasing digits
        visualName.fontSize = visualName.fontSize * Mathf.Pow(0.75f, name.Length - 1);

        this.node = node;
    }
}
