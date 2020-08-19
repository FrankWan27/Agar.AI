using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    bool follow = true;
    public PlayerController followingPlayer;
    GameManager gm;
    NNetVisualizer nnetVis;
    Camera cam;
    [SerializeField]
    float smoothSpeed = 0.2f;
    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        cam = GetComponent<Camera>();
        nnetVis = FindObjectOfType<NNetVisualizer>();
    }

    public void ToggleCamera()
    {
        follow = !follow;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            followingPlayer = gm.GetPrevPlayer(followingPlayer);
            nnetVis.SetPlayer(followingPlayer);
        }
        if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            followingPlayer = gm.GetNextPlayer(followingPlayer);
            nnetVis.SetPlayer(followingPlayer);
        }
    }


    void LateUpdate()
    {
        Vector3 target;
        float targetScale;
        if (follow)
        {
            if (followingPlayer == null)
            {
                followingPlayer = gm.GetBestPlayer();
                nnetVis.SetPlayer(followingPlayer);
            }

            if (followingPlayer == null)
                return;

            target = followingPlayer.transform.position + new Vector3(0, 0, -10);
            targetScale = Mathf.Sqrt(followingPlayer.player.scale);
        }
        else
        {
            followingPlayer = null;
            target = new Vector3(0, 0, -10);
            targetScale = 22f;
        }
        transform.position = Vector3.Lerp(transform.position, target, smoothSpeed);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetScale, smoothSpeed);
    }
}
