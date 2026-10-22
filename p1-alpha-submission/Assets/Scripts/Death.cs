using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Health))]
public class PlayerDeath : MonoBehaviour
{
    GameManager gameManager;

    void Awake()
    {
        GetComponent<Health>().OnDeath += Die;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void Die()
    {
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        gameManager.StopMusic();
        gameManager.StopSFX();
        gameManager.setSFXVolume(0.7f);
        gameManager.PlaySFX(gameManager.playerDeath);

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}