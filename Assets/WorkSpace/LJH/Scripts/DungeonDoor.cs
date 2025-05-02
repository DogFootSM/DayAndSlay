using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonDoor : MonoBehaviour
{

    [Header("·Îµù ¾À")]
    [SerializeField] protected SceneReference loadingScene;
    [Header("ÀÌµ¿ÇÒ ¾À")]
    [SerializeField] SceneReference scene;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerInteractionDoor(collision);

    }

    void PlayerInteractionDoor(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Loading.LoadScene(scene);
            SceneManager.LoadScene(loadingScene.Name);
        }
    }
}
