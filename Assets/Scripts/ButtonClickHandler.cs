/*using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    public Button myButton; // ��ư ������Ʈ
    public Animator avatarAnimator; // �ƹ�Ÿ�� �ִϸ�����

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
        // ������ �����͸� ������ ���� UnityWebRequest ���
        UnityWebRequest www = UnityWebRequest.Get("http://yourserver.com/api/button");
        yield return www.SendWebRequest();

        // �����κ��� ������ ���� ��
        if (www.result == UnityWebRequest.Result.Success)
        {
            // ���� �����Ϳ� ���� �ִϸ��̼� ����
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
                    Debug.Log("�� �� ���� ����: " + www.downloadHandler.text);
                    break;
            }
        }
        else
        {
            Debug.Log("���� ��� ����: " + www.error);
        }
    }

    void StartTrigger(string triggerName)
    {
        // �ִϸ����Ϳ��� �����ϰ��� �ϴ� �ִϸ��̼� Ʈ���� ����
        avatarAnimator.SetTrigger(triggerName);
    }
}
*/