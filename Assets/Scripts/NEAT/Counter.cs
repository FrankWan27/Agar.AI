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

}