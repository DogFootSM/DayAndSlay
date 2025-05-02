using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private Animator animator;

    [Header("·Îµù ¾À")]
    [SerializeField] private SceneReference loadingScene;

    [SerializeField] private Transform movePosTrans;
    private Vector2 movePos;

    void Start()
    {
        animator = GetComponent<Animator>();
        movePos = movePosTrans.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerInteractionDoor(collision);

    }

    void PlayerInteractionDoor(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.Play("DoorOpenAni");
            SceneManager.LoadSceneAsync(loadingScene.Name, LoadSceneMode.Additive);
            collision.gameObject.transform.position = movePos;
        }
    }

    


}
