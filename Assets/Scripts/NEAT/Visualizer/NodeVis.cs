using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeVis : MonoBehaviour
{
    NNet.Node node;
    public Text label;
    public void SetNode(NNet.Node node)
    {
        this.node = node;
        label.text = node.id.ToString();
    }

    public void ShowNodeInfo()
    {
        Debug.Log("Node ID: " + node.id + "\nNode value: " + node.value + "\nNode activation function: " + node.activation);
    }
    
}
