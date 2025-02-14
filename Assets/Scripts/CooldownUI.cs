using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownUI : MonoBehaviour
{
    public UnityEngine.UI.Image fill;
    public float maxCooldown = 5f;
    public float currentCooldown = 5f;
    public int count;

    // ���� Cool Down �ִϸ��̼� ����
    // �ִ� �ð�(maxCooldown)�� �����Ͽ� �ð� ��ü ����
    public void SetMaxCooldown(in float value)
    {
        maxCooldown = value;
        UpdateFiilAmount();
    }

    public void SetCurrentCooldown(in float value)
    {
        currentCooldown = value;
        UpdateFiilAmount();
    }

    private void UpdateFiilAmount()
    {
        fill.fillAmount = currentCooldown / maxCooldown;
    }

    // Test
    private void Update()
    {
        SetCurrentCooldown(currentCooldown + Time.deltaTime);

        // Loop
        if (currentCooldown > maxCooldown)
        {
            if (count > 0)
            {
                currentCooldown = 0;
                count++;
            }
            
        }
            
    }
}