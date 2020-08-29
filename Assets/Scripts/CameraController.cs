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
    public SpriteRenderer bg;
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
        if (follow)
            bg.drawMode = SpriteDrawMode.Tiled;
        else
            bg.drawMode = SpriteDrawMode.Simple;

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

            if(Input.GetKeyDown(KeyCode.H))
            {
                if (followingPlayer.mode == PlayerController.Mode.Human)
                    followingPlayer.mode = PlayerController.Mode.AI;
                else
                    followingPlayer.mode = PlayerController.Mode.Human;
            }
            target = followingPlayer.transform.position + new Vector3(0, 0, -10);
            targetScale = Mathf.Sqrt(followingPlayer.player.scale);
        }
        else
        {
            if(followingPlayer != null)
            {
                followingPlayer.mode = PlayerController.Mode.AI;
                followingPlayer = null;
            }
            target = new Vector3(0, 0, -10);
            targetScale = 22f;
        }
        transform.position = Vector3.Lerp(transform.position, target, smoothSpeed);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetScale, smoothSpeed);
    }
}
