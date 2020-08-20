using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class Food : MonoBehaviour
{
    public float x;
    public float y;
    public Tuple<int, int> coord;
    GameManager gm;
    public void Awake()
    {
    }

    public void Initialize(GameManager gm)
    {
        this.gm = gm;
        GetComponent<SpriteRenderer>().color = Utils.RandomColor();
        SetLocation();
    }


    void SetLocation()
    {
        float randX = UnityEngine.Random.Range(-Utils.GAME_WIDTH / 2, Utils.GAME_WIDTH / 2);
        float randY = UnityEngine.Random.Range(-Utils.GAME_HEIGHT / 2, Utils.GAME_HEIGHT / 2);
        Vector3 randLoc = new Vector3(randX, randY, 0);

        transform.position = randLoc;
        x = randX;
        y = randY;

        coord = Utils.GetCoordinate(randX, randY);

        AddToMap();
    }

    void AddToMap()
    {
        List<Food> list;
        if (!gm.foodMap.TryGetValue(coord, out list))
        {
            list = new List<Food>();
            gm.foodMap.Add(coord, list);
        }
        list.Add(this);
    }
    public void Death()
    {
        gm.foodMap[coord].Remove(this);
        SetLocation();
    }
}