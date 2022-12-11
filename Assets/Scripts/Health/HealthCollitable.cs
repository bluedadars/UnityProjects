using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollitable : MonoBehaviour
{
    [SerializeField] private float healthValue;

    [SerializeField] private AudioClip pickeupSound;



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(pickeupSound);
            collision.GetComponent<Health>().AddHealthValue(healthValue);
            gameObject.SetActive(false);
        }
    }
}
