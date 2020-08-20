using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class NNetVisualizer : MonoBehaviour
{


    public float WIDTH = 18f;
    public float HEIGHT = 18f;
    public NNet nnet;

    public GameObject nodePrefab;
    public GameObject linePrefab;
    Dictionary<int, GameObject> nodeMap;
    List<GameObject> lines;


    public void SetPlayer(PlayerController followingPlayer)
    {
        ResetStructure();
        nnet = followingPlayer.brain.nnet;
        CreateStructure();
    }

    void ResetStructure()
    {
        if (nodeMap != null)
            foreach (GameObject o in nodeMap.Values)
                Destroy(o);

        if (lines != null)
            foreach (GameObject o in lines)
                Destroy(o);
    }

    void CreateStructure()
    {
        nodeMap = new Dictionary<int, GameObject>();
        lines = new List<GameObject>();

        DrawNodes();
        DrawConnections();
        
    }

    void DrawConnections()
    {
        foreach(NNet.Connection connection in nnet.connections)
        {
            float colorVal = (connection.weight + 1) / 2f;
            Color color = new Color((1 - colorVal), colorVal, 0f);
            GameObject newCon = Instantiate(linePrefab, transform);
            
            LineRenderer line = newCon.GetComponent<LineRenderer>();
            line.SetPosition(0, nodeMap[connection.inNode].transform.position);
            line.SetPosition(1, nodeMap[connection.outNode].transform.position);
            line.startColor = color;
            line.endColor = color;
            lines.Add(newCon);

            newCon.GetComponent<LineVis>().SetCon(connection);
        }
    }

    void DrawNodes()
    {
        //Draw Input Nodes

        float xPos = -WIDTH / 2;
        float yPos = -HEIGHT / 2;
        foreach (NNet.Node node in nnet.inputNodes)
        {
            GameObject newNode = Instantiate(nodePrefab, transform);
            newNode.transform.Translate(new Vector3(xPos, yPos));
            newNode.GetComponent<NodeVis>().SetNode(node);
            nodeMap.Add(node.id, newNode);

            xPos += (WIDTH / (nnet.inputNodes.Count - 1));
        }

        //Draw Hidden Nodes

        foreach (NNet.Node node in nnet.hiddenNodes)
        {
            GameObject newNode = Instantiate(nodePrefab, transform);
            xPos = UnityEngine.Random.Range(-WIDTH / 2 + 1f, WIDTH / 2 - 1f);
            yPos = UnityEngine.Random.Range(-WIDTH / 2 + 1f, WIDTH / 2 - 1f);

            newNode.transform.Translate(new Vector3(xPos, yPos));
            newNode.GetComponent<NodeVis>().SetNode(node);
            nodeMap.Add(node.id, newNode);
        }

        //Draw Output Nodes

        xPos = -WIDTH / 2;
        yPos = HEIGHT / 2;
        foreach (NNet.Node node in nnet.outputNodes)
        {
            GameObject newNode = Instantiate(nodePrefab, transform);
            newNode.transform.Translate(new Vector3(xPos, yPos));
            newNode.GetComponent<NodeVis>().SetNode(node);
            nodeMap.Add(node.id, newNode);

            xPos += (WIDTH / (nnet.outputNodes.Count - 1));
        }
    }

}