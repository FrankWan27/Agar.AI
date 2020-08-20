using UnityEngine;
using System.Collections;
using System.Threading;

public class Settings : MonoBehaviour
{
    public float timer = 60f;
    public int populationSize = 50;
    public float AddNodeRate = 0.03f;
    public float AddConnectionRate = 0.05f;
    void Awake()
    {
        Utils.TIMER = timer;
        GenomeUtils.POP_SIZE = populationSize;
        GenomeUtils.ADD_NODE_RATE = AddNodeRate;
        GenomeUtils.ADD_CONNECTION_RATE = AddConnectionRate;

    }

}
