using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonDoor : Door
{
    [Header("¿Ãµø«“ æ¿")]
    [SerializeField] SceneReference scene;

    public override void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected override void PlayerInteractionDoor(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Loading.LoadScene(scene);
            SceneManager.LoadScene(loadingScene.Name);
        }
    }
}
