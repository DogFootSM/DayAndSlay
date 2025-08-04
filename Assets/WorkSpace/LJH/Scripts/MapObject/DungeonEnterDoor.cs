using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class DungeonEnterDoor : MonoBehaviour
{
    [Inject(Id = "LoadingScene")] [Header("·Îµù ¾À")]
    private SceneReference loadingScene;

    [Inject(Id = "DungeonScene")] [Header("ÀÌµ¿ÇÒ ¾À")]
    private SceneReference scene;

    [SerializeField] private GameObject stagePopUp;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            stagePopUp.SetActive(!stagePopUp.activeSelf);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            stagePopUp.SetActive(!stagePopUp.activeSelf);
        }
    }

    public void PlayerInteractionDoor()
    {
        Loading.LoadScene(scene);
        SceneManager.LoadScene(loadingScene.Name);
    }
}