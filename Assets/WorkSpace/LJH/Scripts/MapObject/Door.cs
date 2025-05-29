using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Door : MonoBehaviour, IInteractionStore
{
    private Animator animator;

    [Inject(Id = "LoadingScene")]
    [Header("·Îµù ¾À")]
    private SceneReference loadingScene;

    [SerializeField] private Transform movePosTrans;
    private Vector2 movePos;
    private GameObject player;

    void Start()
    {
        animator = GetComponent<Animator>();
        movePos = movePosTrans.position;
    }

    public void Interaction()
    {
        animator.Play("DoorOpenAni");
        SceneManager.LoadSceneAsync(loadingScene.Name, LoadSceneMode.Additive);
        player.transform.position = movePos;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        player = collision.gameObject;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        player = null;
    }




}
