using UnityEngine;
using System.Collections;

public class Counter
{
    private static int nodeCounter = 1;
    private static int connectionCounter = 1;

    public static int NextNode()
    {
        return nodeCounter++;
    }
    public static int NextConnection()
    {
        return connectionCounter++;
    }
    public static void Reset()
    {
        nodeCounter = 1;
        connectionCounter = 1;
    }

    public static void SetNodeCounter(int id)
    {
        if (id > nodeCounter)
            nodeCounter = id + 1;
    }
    public static void SetConnectionCounter(int innovation)
    {
        if (innovation > connectionCounter)
            connectionCounter = innovation + 1;
    }

}