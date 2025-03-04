using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionController : MonoBehaviour
{
    // Let's you change the color of an object upon collision
    public bool changeColor;
    public Color myColor;
    
    // States of GameObjects to destroy them upon collision
    public bool destroyEnemy;
    public bool destroyCollectibles;
        
    // Allows you to add an audio file that's played on collision
    public AudioClip collisionAudio;
    private AudioSource audioSource;
    public float pushPower;
    
    // Score UI setup
    public TMPro.TMP_Text scoreUI;
    public int increaseScore = 1;
    public int decreaseScore = 1; 
    
    private int score = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    // Update is called once per frame
    void Update()
    {
        // Update score UI text if it exists
        if (scoreUI != null)
        {
            scoreUI.text = score.ToString();
        }
    }
    
    // This is called when colliders touch
    void OnCollisionEnter(Collision other)
    {
        if (changeColor == true)
        {
            // Change color of the object upon collision
            gameObject.GetComponent<Renderer>().material.color = myColor;
        }
        
        // Play collision sound if the audio source is available and not already playing
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(collisionAudio, 0.5F);
        }
        
        // Destroy enemy or collectible based on the flags
        if ((destroyEnemy && other.gameObject.tag == "Enemy") || (destroyCollectibles && other.gameObject.tag == "Collectible"))
        {
            Destroy(other.gameObject);
        }
        
        // Increase the score when colliding with a "Collectible"
        if (scoreUI != null && other.gameObject.tag == "Collectible")
        {
            score += increaseScore;
        }
        
        // Decrease the score when colliding with an "Enemy"
        if (scoreUI != null && other.gameObject.tag == "Enemy")
        {
            score -= decreaseScore;
        }
    }
    
    // This function is used with character controllers
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        
        // If no Rigidbody or if it's Kinematic, do nothing
        if (body == null || body.isKinematic)
        {
            return;
        }
        
        // Don't push ground or platform GameObjects below character
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction (only on x and z axes)
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        
        // Apply push direction and power to the object's velocity
        if (hit.gameObject.tag == "Object")
        {
            body.velocity = pushDir * pushPower;
        }

        // Play collision sound if the audio source is available and not already playing
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(collisionAudio, 0.5F);
        }
        
        // Destroy enemy or collectible based on the flags
        if (destroyEnemy == true && hit.gameObject.tag == "Enemy" || destroyCollectibles == true && hit.gameObject.tag == "Collectible")
        {
            Destroy(hit.gameObject);
        }
        
        // Increase the score when hitting a "Collectible"
        if (scoreUI != null && hit.gameObject.tag == "Collectible")
        {
            score += increaseScore;
        }
        
        // Decrease the score when hitting an "Enemy"
        if (scoreUI != null && hit.gameObject.tag == "Enemy")
        {
            score -= decreaseScore;
        }
    }
}