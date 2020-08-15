using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int foodCount = 0;
    public List<PlayerController> players;
    public GameObject foodPrefab;
    public GameObject foodParent; 
    public GameObject playerPrefab;
    public GameObject playerParent;
    // Start is called before the first frame update
    void Start()
    {
        players = new List<PlayerController>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(players.Count <= 1)
        {
            GameOver();
        }

        while (foodCount < Utils.FOOD_MAX)
        {
            SpawnFood();
        }
    }

    void GameOver()
    {
        SpawnPopulation();
    }

    private void LateUpdate()
    {
        Debug.Log("PlayerCount: ");
    }

    void SpawnFood()
    {
        float randX = UnityEngine.Random.Range(-Utils.GAME_WIDTH / 2, Utils.GAME_WIDTH / 2);
        float randY = UnityEngine.Random.Range(-Utils.GAME_HEIGHT / 2, Utils.GAME_HEIGHT / 2);
        Vector3 randLoc = new Vector3(randX, randY, 0);
        GameObject food = Instantiate(foodPrefab, foodParent.transform);
        food.transform.position = randLoc;
        foodCount++;
    }

    void SpawnPopulation()
    {
        while (players.Count < Utils.POP_SIZE)
        {
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, playerParent.transform);
        players.Add(player.GetComponent<PlayerController>());
    }

    public void Kill(PlayerController other)
    {
        players.Remove(other);
        Destroy(other.gameObject);
    }
}
