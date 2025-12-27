using UnityEngine;
using System.Collections;

public class CoroutineHelper : MonoBehaviour
{
    // 외부에서 직접 접근할 수 없는 private static 변수
    private static CoroutineHelper _instance;

    // 어디서든 접근 가능한 public static 프로퍼티
    public static CoroutineHelper Instance
    {
        get
        {
            // _instance가 null이면 (아직 생성되지 않았으면)
            if (_instance == null)
            {
                // 씬에서 "CoroutineHelper"라는 이름의 오브젝트를 찾습니다.
                _instance = FindObjectOfType<CoroutineHelper>();

                // 그래도 없으면 새로 만듭니다.
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("CoroutineHelper");
                    _instance = singletonObject.AddComponent<CoroutineHelper>();
                }
            }
            return _instance;
        }
    }

    // 씬을 전환해도 파괴되지 않게 합니다.
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            // DontDestroyOnLoad(gameObject); // 필요하다면 이 주석을 해제하세요.
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
