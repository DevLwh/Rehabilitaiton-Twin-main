using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject targetObject; // Ȱ��ȭ/��Ȱ��ȭ�� ������Ʈ

    public void Toggle()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
