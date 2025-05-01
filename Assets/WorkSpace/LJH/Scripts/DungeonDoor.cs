using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonDoor : Door
{
    [Header("이동할 씬")]
    [SerializeField] SceneReference scene;

    public override void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected override void PlayerInteractionDoor(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("던젼도어");
            Loading.LoadScene(scene);
            SceneManager.LoadScene(loadingScene.Name);
        }
    }
}
