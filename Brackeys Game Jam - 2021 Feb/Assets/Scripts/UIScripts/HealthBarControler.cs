using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarControler : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image Health;
    private Vector2 relativePos;
    // Start is called before the first frame update

    private void Start()
    {
        relativePos = transform.parent.parent.position - transform.parent.position;
    }
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
       
        Health.color = gradient.Evaluate(1f);
    }

    private void Update()
    {
        transform.parent.eulerAngles = new Vector3(0, 0, 0);
        transform.parent.position = (Vector2)transform.parent.parent.position - relativePos;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        Health.color = gradient.Evaluate(slider.normalizedValue);
    }
}
