using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    protected Animator animator;

    [Header("로딩 씬")]
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
        Debug.Log("충돌");
        PlayerInteractionDoor(collision);

    }

    protected virtual void PlayerInteractionDoor(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("도어");
            animator.Play("DoorOpenAni");
            SceneManager.LoadScene(loadingScene.Name);
            collision.gameObject.transform.position = movePos;
        }
    }


}
