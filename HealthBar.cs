using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviourPun
{
    public Slider slider;

    public void Health(float health) 
    {
        slider.value = health;
    }
}
