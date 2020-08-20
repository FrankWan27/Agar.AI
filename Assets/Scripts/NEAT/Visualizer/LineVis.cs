using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineVis : MonoBehaviour
{
    NNet.Connection con;
    public void SetCon(NNet.Connection con)
    {
        this.con = con;
        CreateCollider();
    }

    private void Awake()
    {
    }

    public NNet.Connection GetLine()
    {
        //Debug.Log("Node ID: " + node.id + "\nNode value: " + node.value + "\nNode activation function: " + node.activation);
        return con;
    }

    void CreateCollider()
    {
        LineRenderer line = GetComponent<LineRenderer>();
        CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();
        capsule.radius = line.startWidth;
        capsule.center = Vector3.zero;
        capsule.direction = 2; // Z-axis for easier "LookAt" orientation

        Vector3 start = line.GetPosition(0);
        Vector3 end = line.GetPosition(1);

        capsule.transform.position = start + (end - start) / 2;
        capsule.transform.LookAt(start);
        capsule.height = (end - start).magnitude;
    }
}
