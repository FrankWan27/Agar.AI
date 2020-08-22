using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player
{
    PlayerController pc;

    public float x;
    public float y;
    public float size;
    public float scale;

    //public Color color;

    public Player(PlayerController pc)
    {
        this.pc = pc;
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        x = Random.Range(-Utils.GAME_WIDTH / 2, Utils.GAME_WIDTH / 2);
        y = Random.Range(-Utils.GAME_HEIGHT / 2, Utils.GAME_HEIGHT / 2);
        pc.transform.position = new Vector3(x, y);
        size = 1f;
        //color = Utils.RandomColor();
        UpdateScale();
    }

    public void AddSize(float f)
    {
        size += f;
        UpdateScale();
    }

    public void RemoveSize(float f)
    {
        size -= f;
        if (size < 1f)
            size = 1f;
        UpdateScale();
    }

    public void Move(float angle, float speed)
    {

        float maxSpeed = 2.2f * Mathf.Pow(size,-0.439f);


        pc.transform.Translate(Mathf.Cos(angle) * maxSpeed * speed * Time.deltaTime, Mathf.Sin(angle) * maxSpeed * speed * Time.deltaTime, 0);

        x = pc.transform.position.x;
        y = pc.transform.position.y;



        x = Mathf.Clamp(x, -Utils.GAME_WIDTH / 2, Utils.GAME_WIDTH / 2);
        y = Mathf.Clamp(y, -Utils.GAME_HEIGHT / 2, Utils.GAME_HEIGHT / 2);
        pc.transform.position = new Vector3(x, y);

    }

    public void UpdateScale()
    {
        scale = 1.5f * Mathf.Sqrt(size);
    }
}