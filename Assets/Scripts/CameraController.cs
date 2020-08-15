using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController followingPlayer;
    GameManager gm;
    Camera cam;
    [SerializeField]
    float smoothSpeed = 0.05f;
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if(followingPlayer == null)
        {
            followingPlayer = gm.players[0];
        }
    }

    void LateUpdate()
    {
        Vector3 target = followingPlayer.transform.position + new Vector3(0, 0, -10);
        transform.position = Vector3.Lerp(transform.position, target, smoothSpeed);

        float targetScale = Mathf.Sqrt(followingPlayer.player.scale);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetScale, smoothSpeed);
    }
}
