using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum Mode {Human, AI};

    Mode mode = Mode.Human;
    public Player player;
    [ContextMenuItem("Eat Food", "EatFood")]
    public Sprite playerSprite;
    GameManager gm;


    float dx = 0;
    float dy = 0;
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        player = new Player(this);
        playerSprite = GetComponent<Sprite>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
        }


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
            Destroy(collision.gameObject);
            gm.foodCount--;
            EatFood();
        }
        else if (collision.gameObject.tag == "Player")
        {
            PlayerController other = collision.gameObject.GetComponent<PlayerController>();
            if (player.size >= other.player.size * 1.25f)
            {
                //Absorb(other);
                player.AddSize(other.player.size);
                gm.Kill(other);
            }
        }
    }

    void EatFood()
    {
        player.AddSize(1f);
    }

    void GetTargetDestination()
    {
        if(mode == Mode.AI)
        {
            Debug.Log("AI");
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
