/*using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    public Button myButton; // 버튼 오브젝트
    public Animator avatarAnimator; // 아바타의 애니메이터

    void Start()
    {
        myButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        StartCoroutine(SendRequestToServer());
    }

    IEnumerator SendRequestToServer()
    {
        // 서버로 데이터를 보내기 위해 UnityWebRequest 사용
        UnityWebRequest www = UnityWebRequest.Get("http://yourserver.com/api/button");
        yield return www.SendWebRequest();

        // 서버로부터 응답을 받은 후
        if (www.result == UnityWebRequest.Result.Success)
        {
            // 응답 데이터에 따라 애니메이션 실행
            switch (www.downloadHandler.text)
            {
                case "1":
                    StartTrigger("Start1");
                    break;
                case "2":
                    StartTrigger("Start2");
                    break;
                case "3":
                    StartTrigger("Start3");
                    break;
                case "4":
                    StartTrigger("Start4");
                    break;
                case "5":
                    StartTrigger("Start5");
                    break;
                case "6":
                    StartTrigger("Start6");
                    break;
                default:
                    Debug.Log("알 수 없는 응답: " + www.downloadHandler.text);
                    break;
            }
        }
        else
        {
            Debug.Log("서버 통신 실패: " + www.error);
        }
    }

    void StartTrigger(string triggerName)
    {
        // 애니메이터에서 실행하고자 하는 애니메이션 트리거 설정
        avatarAnimator.SetTrigger(triggerName);
    }
}
*/