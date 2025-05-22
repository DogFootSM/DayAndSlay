using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Door : MonoBehaviour, IInteractionStore
{
    private Animator animator;

    [Inject(Id = "LoadingScene")]
    [Header("로딩 씬")]
    private SceneReference loadingScene;
    
    [SerializeField] private Transform movePosTrans;
    private Vector2 movePos;

    void Start()
    {
        animator = GetComponent<Animator>();
        movePos = movePosTrans.position;
    }

    public void Interaction()
    {
        //Todo : 문 사용을 여기에 달아줘야함
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
