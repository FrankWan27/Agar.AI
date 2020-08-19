using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    float timer = Utils.TIMER;
    public List<PlayerController> players;
    public List<Food> foods;
    public Dictionary<Tuple<int, int>, List<Food>> foodMap;
    public GameObject foodPrefab;
    public GameObject foodParent; 
    public GameObject playerPrefab;
    public GameObject playerParent;
    GUIManager guiManager;
    NEATManager NEAT;
    // Start is called before the first frame update
    void Start()
    {

        NEAT = GetComponent<NEATManager>();
        guiManager = FindObjectOfType<GUIManager>();
        players = new List<PlayerController>();
        foods = new List<Food>();
        foodMap = new Dictionary<Tuple<int, int>, List<Food>>();
    }

    // Update is called once per frame
    void Update()
    {
        if(players.Count == 0)
            SpawnPopulation();

        if (players.Count <= 1)
        {
            GameOver();
        }

        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            GameOver();
        }


        while (foods.Count < Utils.FOOD_MAX)
        {
            SpawnFood();
        }
    }

    void GameOver()
    {
        timer = Utils.TIMER;
        while(players.Count > 0)
            Kill(players[0]);

        NEAT.NextGeneration();
        SpawnPopulation();
    }

    void SpawnFood()
    {
        Food food = Instantiate(foodPrefab, foodParent.transform).GetComponent<Food>();

        foods.Add(food);
        List<Food> list;
        if(!foodMap.TryGetValue(food.coord, out list))
        {
            list = new List<Food>();
            foodMap.Add(food.coord, list);
        }
        list.Add(food);
    }

    public void DestroyFood(Food food)
    {
        foods.Remove(food);
        foodMap[food.coord].Remove(food);
        Destroy(food.gameObject);
    }

    void SpawnPopulation()
    {
        while (players.Count < Utils.POP_SIZE)
        {
            SpawnPlayer();
        }
        guiManager.UpdatePlayerCount();
    }

    void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, playerParent.transform);
        player.GetComponent<SpriteRenderer>().color = Utils.RandomColor();
        player.GetComponent<PlayerController>().SetNNet(NEAT.GetNNet());
        players.Add(player.GetComponent<PlayerController>());
    }

    public void Kill(PlayerController other)
    {
        players.Remove(other);
        Destroy(other.gameObject);
        guiManager.UpdatePlayerCount();
    }

    public PlayerController GetBestPlayer()
    {
        if (players.Count == 0)
            return null;

        PlayerController best = players[0];

        for(int i = 1; i < players.Count; i++)
        {
            if (players[i].player.size > best.player.size)
                best = players[i];
        }

        return best;
    }

    public PlayerController GetPrevPlayer(PlayerController pc)
    {
        int index = players.IndexOf(pc) - 1;
        if (index < 0)
            index = players.Count - 1;
        return players[index];
    }

    public PlayerController GetNextPlayer(PlayerController pc)
    {
        int index = players.IndexOf(pc) + 1;
        if (index >= players.Count)
            index = 0;
        return players[index];
    }
}
