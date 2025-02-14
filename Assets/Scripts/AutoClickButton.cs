using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoClickButton : MonoBehaviour
{
    public Button myButton; // Inspector에서 설정할 버튼 참조

    void Start()
    {
        // PlayerPrefs에서 지연 시간 가져오기 (기본값은 30초)
        float delay = PlayerPrefs.GetFloat("ButtonClickDelay", 30f);

        // 코루틴 시작
        StartCoroutine(ClickButtonAfterDelay(delay));
    }

    IEnumerator ClickButtonAfterDelay(float delay)
    {
        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(delay);
        // 버튼 클릭
        myButton.onClick.Invoke();
    }
}

