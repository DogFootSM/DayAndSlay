using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public static SceneReference nextScene;
    [SerializeField] Image gage;

    private void Start()
    {
        StartCoroutine(LoadingGageScene());
    }

    public static void LoadScene(SceneReference scene)
    {
        nextScene = scene;
    }

    //Comment : round를 따와서 라운드에 해당하는 바 컨트롤
    IEnumerator LoadingGageScene()
    {
        yield return null;
        if (nextScene != null)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(nextScene.Name);
            op.allowSceneActivation = false;

            while (!op.isDone)
            {
                yield return new WaitForSeconds(0.1f);
                gage.fillAmount += 0.05f;

                if (gage.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
        else
        {
            while (gage.fillAmount < 1.0f)
            {
                yield return new WaitForSeconds(0.1f);
                gage.fillAmount += 0.05f;

                if (gage.fillAmount == 1.0f)
                {
                    SceneManager.UnloadSceneAsync("LoadingScene");
                    yield break;
                }
            }
        }
    }
}