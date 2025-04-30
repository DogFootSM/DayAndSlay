using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    Animator animator;
    [SerializeField] SceneReference scene;
    [SerializeField] SceneReference loadingScene;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Ãæµ¹!!!");
            animator.Play("DoorOpenAni");
            Loading.LoadScene(scene);
            SceneManager.LoadScene(loadingScene.Name);
        }
    }
}
