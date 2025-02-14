using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AutoClickButton : MonoBehaviour
{
    public Button myButton; // Inspector���� ������ ��ư ����

    void Start()
    {
        // PlayerPrefs���� ���� �ð� �������� (�⺻���� 30��)
        float delay = PlayerPrefs.GetFloat("ButtonClickDelay", 30f);

        // �ڷ�ƾ ����
        StartCoroutine(ClickButtonAfterDelay(delay));
    }

    IEnumerator ClickButtonAfterDelay(float delay)
    {
        // ������ �ð���ŭ ���
        yield return new WaitForSeconds(delay);
        // ��ư Ŭ��
        myButton.onClick.Invoke();
    }
}

