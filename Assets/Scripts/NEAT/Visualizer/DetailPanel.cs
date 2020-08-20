using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailPanel : MonoBehaviour
{
    public GameObject nodePanel;
    public GameObject linePanel;
    public GameObject xButton;

    //Node Info
    NNet.Node node = null;
    public Text nodeID;
    public Text nodeType;
    public Text nodeValue;
    public Text nodeActivation;

    //Line Info
    NNet.Connection line = null;
    public Text lineInNode;
    public Text lineOutNode;
    public Text lineWeight;
    public Text lineRecurrent;
    

    void Update()
    {
        if(node != null)
        {
            nodeValue.text = node.value.ToString("F3");
        }
    }

    public void OpenNodePanel(NNet.Node node)
    {
        if (node == null)
            return;

        nodePanel.SetActive(true);
        xButton.SetActive(true);
        linePanel.SetActive(false);

        this.node = node;
        nodeID.text = "Node ID: " + node.id.ToString();
        nodeType.text = "Type: " + node.type.ToString();
        nodeActivation.text = "Activation: " + node.activation.ToString();

    }

    public void ClosePanel()
    {
        nodePanel.SetActive(false);
        linePanel.SetActive(false);
        xButton.SetActive(false);
        node = null;
       
    }

    public void OpenLinePanel(NNet.Connection connection)
    {
        if (connection == null)
            return;

        nodePanel.SetActive(false);
        xButton.SetActive(true);
        linePanel.SetActive(true);

        this.line = connection;
        lineInNode.text = "In Node: " + line.inNode;
        lineOutNode.text = "Out Node: " + line.outNode;
        lineWeight.text = "Weight: " + line.weight;
        lineRecurrent.text = "Recurrent: " + line.recurrent;
    }
}
