using UnityEngine;
using DG.Tweening;

public class GhostTrail : MonoBehaviour
{
    public float ghostDelay;
    public float destroytime = 0.5f;
    public GameObject ghost;
    public bool makeGhost = false;
    private Movement move;
    private float ghostDelaySeconds;
    private void Start()
    {
        ghostDelaySeconds = ghostDelay;
        move = GetComponent<Movement>();
    }

    private void Update()
    {
        if (makeGhost)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;

            }
            else
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                if (move.side == -1)
                    currentGhost.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, destroytime);
            }
        }
    }



}
