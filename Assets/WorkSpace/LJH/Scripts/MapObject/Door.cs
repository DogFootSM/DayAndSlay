using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Door : InteractableObj, IInteractionStoreScene
{
    private Animator animator;

    [Inject(Id = "LoadingScene")]
    [Header("로딩 씬")]
    private SceneReference loadingScene;

    [SerializeField] private Transform movePosTrans;
    private Vector2 movePos;
    [SerializeField] private GameObject player;

    [Inject(Id = "PopUp")]
    GameObject popUp;

    void Start()
    {
        animator = GetComponent<Animator>();
        movePos = movePosTrans.position;
    }

    public override void Interaction()
    {
        Debug.Log("문열림 실행");
        animator.Play("DoorOpenAni");
        SceneManager.LoadSceneAsync(loadingScene.Name, LoadSceneMode.Additive);
        player.transform.position = movePos;

    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        popUp.GetComponent<PopUp>().objName = "문";
        popUp.SetActive(!popUp.gameObject.activeSelf);
    }




}
