using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    protected Animator animator;

    [Header("·Îµù ¾À")]
    [SerializeField] protected SceneReference loadingScene;

    [SerializeField] private Transform movePosTrans;
    private Vector2 movePos;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        movePos = movePosTrans.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerInteractionDoor(collision);

    }

    protected virtual void PlayerInteractionDoor(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.Play("DoorOpenAni");
            
            collision.gameObject.transform.position = movePos;
        }
    }

    


}
