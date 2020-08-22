using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Mode {Human, AI};

    public Mode mode = Mode.AI;
    public Player player;
    public Brain brain;
    public GameManager gm;
    GameObject crown;


    float dx = 0;
    float dy = 0;
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        crown = transform.GetChild(0).gameObject;
        player = new Player(this);
        brain = new Brain(player, gm);
    }

    // Update is called once per frame
    void Update()
    {
        if(mode == Mode.Human && Input.GetKeyDown("space"))
        {
            Split();
        }
        //Update Scale
        transform.localScale = new Vector3(player.scale, player.scale, 1);
        GetTargetDestination();
        player.Move(dx, dy);
        //Decay();
        //transform.localPosition = new Vector3(player.x, player.y, -1);

    }

    void Decay()
    {
        player.RemoveSize(player.size * Utils.DECAY_RATE * Time.deltaTime);
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

    public void Initialize(NNet nnet, bool isChampion)
    {
        GetComponent<SpriteRenderer>().color = Utils.RandomColor();
        if (isChampion)
            crown.SetActive(true);
        else
            crown.SetActive(false);

        brain.nnet = nnet;
    }

    public void EatFood()
    {
        player.AddSize(1f);
    }

    void Split()
    {
        player.Split();
    }

    void GetTargetDestination()
    {
        if(mode == Mode.AI)
        {
            float[] output = brain.GetOutput();

            float angle = (output[0]) * 2 * Mathf.PI;
            //float speed = Mathf.Clamp(output[1], 0f, 1f);
            float speed = 1f;
            SetAcceleration(angle, speed);

            float splitUrge = GenomeUtils.Sigmoid(output[1]);
            if(splitUrge >= 0.8f)
            {
                Split();
            }
        }
        else
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float xDiff = worldPosition.x - player.x;
            float yDiff = worldPosition.y - player.y;

            float angle = Mathf.Atan2(yDiff, xDiff);
            float distance = Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff);


            distance = Mathf.Clamp(distance, 0f, 1f);

            SetAcceleration(angle, distance);
        }
    }

    void SetAcceleration(float a, float b)
    {
        dx = a;
        dy = b;
    }
}
