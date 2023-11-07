using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollective : MonoBehaviour
{
    [SerializeField] private float healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerHealthSystem>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}