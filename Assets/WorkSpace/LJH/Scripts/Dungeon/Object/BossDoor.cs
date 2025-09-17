using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossDoor : InteractableObj
{
    [Header("·Îµù ¾À")]
    [SerializeField] private SceneReference loadingScene;
    [Header("ÀÌµ¿ÇÒ ¾À")]
    [SerializeField] private SceneReference scene;
    
    [SerializeField] private GameObject dungeonExitPopUp;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void Start()
    {
        yesButton.onClick.AddListener(YesButton);
        noButton.onClick.AddListener(NoButton);
    }
    public override void Interaction()
    {
        dungeonExitPopUp.SetActive(true);
    }

    private void YesButton()
    {
        Loading.LoadScene(scene);
        SceneManager.LoadScene(loadingScene.Name);
    }

    private void NoButton()
    {
        
    }
}
