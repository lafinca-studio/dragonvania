using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] AudioClip coinSFX;
    [SerializeField] int coinValue = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(coinSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddToScore(coinValue);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
