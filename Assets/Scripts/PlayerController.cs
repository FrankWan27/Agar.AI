using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum Mode {Human, AI};

    Mode mode = Mode.AI;
    public Player player;
    public Brain brain;
    GameManager gm;


    float dx = 0;
    float dy = 0;
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        player = new Player(this);
        brain = new Brain(player, gm);
    }

    // Update is called once per frame
    void Update()
    {
        //Update Scale
        transform.localScale = new Vector3(player.scale, player.scale, 1);
        GetTargetDestination();
        player.Move(dx, dy);
        //transform.localPosition = new Vector3(player.x, player.y, -1);

    }

    void Decay()
    {
        player.RemoveSize(player.size * Utils.DECAY_RATE);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            collision.gameObject.GetComponent<Food>().Death();
            EatFood();
        }
        else if (collision.gameObject.tag == "Player")
        {
            PlayerController other = collision.gameObject.GetComponent<PlayerController>();
            if (player.size >= other.player.size * 1.25f)
            {
                //Absorb(other);
                player.AddSize(other.player.size);
                other.GameOver();
                
            }
        }
    }

    public void GameOver()
    {
        brain.EvaluateFitness();
        gm.Kill(this);
    }

    public void SetNNet(NNet nnet)
    {
        brain.nnet = nnet;
    }

    void EatFood()
    {
        player.AddSize(1f);
    }

    void GetTargetDestination()
    {
        if(mode == Mode.AI)
        {
            float[] output = brain.GetOutput();

            float xDiff = Mathf.Clamp(output[0], -1f, 1f);
            float yDiff = Mathf.Clamp(output[1], -1f, 1f);

            SetAcceleration(xDiff, yDiff);
        }
        else
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float xDiff = Mathf.Clamp(worldPosition.x - player.x, -1f, 1f);
            float yDiff = Mathf.Clamp(worldPosition.y - player.y, -1f, 1f);

            SetAcceleration(xDiff, yDiff);
        }
    }

    void SetAcceleration(float a, float b)
    {
        dx = a;
        dy = b;
    }
}
