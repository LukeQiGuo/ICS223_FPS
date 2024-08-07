using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position);
        }
    }

    public void SetHealth(float healthPercentage)
    {
        fillImage.fillAmount = healthPercentage;
    }
}
