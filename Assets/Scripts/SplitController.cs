using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitController : MonoBehaviour
{
    public float x;
    public float y;
    public float size;
    public float scale;
    public float angle;
    float velocity = 1.5f;
    float lifetime = 10f;
    PlayerController parent;
    public Vector3 targetPosition;
    float smoothSpeed = 0.1f;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initalize(float size, float angle, PlayerController parent)
    {
        this.x = parent.player.x;
        this.y = parent.player.y;
        transform.position = new Vector3(x, y);
        this.size = size;
        this.angle = angle;
        this.parent = parent;
        velocity *= Mathf.Sqrt(parent.player.scale);

        GetComponent<SpriteRenderer>().color = parent.GetComponent<SpriteRenderer>().color;

        scale = 1.5f * Mathf.Sqrt(size);
        transform.localScale = new Vector3(scale, scale);

        targetPosition = new Vector3(x + Mathf.Cos(angle) * velocity, y +  Mathf.Sin(angle) * velocity);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            collision.gameObject.GetComponent<Food>().Death();
            parent.EatFood();
        }
        else if (collision.gameObject.tag == "Player")
        {
            PlayerController other = collision.gameObject.GetComponent<PlayerController>();
            if (other == parent)
                return;
            if (size >= other.player.size * 1.25f)
            {
                //Absorb(other);
                parent.player.AddSize(other.player.size);
                other.GameOver();

            }
            else if(other.player.size >= size * 1.25f)
            {
                other.player.AddSize(size);
                Destroy(gameObject);
            }

        }
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        x = transform.position.x;
        y = transform.position.y;


        x = Mathf.Clamp(x, -Utils.GAME_WIDTH / 2, Utils.GAME_WIDTH / 2);
        y = Mathf.Clamp(y, -Utils.GAME_HEIGHT / 2, Utils.GAME_HEIGHT / 2);
        transform.position = new Vector3(x, y);


        if (lifetime <= 0f)
        {
            parent.player.AddSize(size);
            Destroy(gameObject);
        }
    }
}
