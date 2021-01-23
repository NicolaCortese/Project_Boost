using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float RotationalThrust = 200f;
    [SerializeField] float MainThrust = 1000f;

    [SerializeField] float levelDelay = 2f;

    [SerializeField] AudioClip MainEngine;
    [SerializeField] AudioClip Kaboom;
    [SerializeField] AudioClip Winner;
    
    [SerializeField] ParticleSystem MainEngineParticle;
    [SerializeField] ParticleSystem KaboomParticle;
    [SerializeField] ParticleSystem WinnerParticle;

    Rigidbody rigidBody;
    AudioSource audioSource;
    bool Transient = false;

    bool CollisionsEnabler = true;

    



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Transient) 
        {
            Rotate();
            Thrust();
        }
        if (Debug.isDebugBuild)
        {
            Debugkeys();
        }
    }

    private void Debugkeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CollisionsEnabler = !CollisionsEnabler;

        }
    }

    void OnCollisionEnter(Collision collision)
        {
        if (Transient|| !CollisionsEnabler )
        {
            return;
        }
        
        switch (collision.gameObject.tag)
            {
            case "friendly":
                //do nada
                break;
            case "finish":
                Transient = true;
                audioSource.Stop();
                audioSource.PlayOneShot(Winner);
                WinnerParticle.Play();
                
                Invoke("LoadNextScene", levelDelay); // change timing
                break;
            default:
                Transient = true;
                audioSource.Stop();
                audioSource.PlayOneShot(Kaboom);
                KaboomParticle.Play();
                Invoke("Respawn", levelDelay);

                break;


            }
                
        }
    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        int finalScene = SceneManager.sceneCountInBuildSettings-1;
        print(finalScene);
        if (currentSceneIndex == finalScene)
        {
            SceneManager.LoadScene(0);

        }
        else
            SceneManager.LoadScene(nextSceneIndex);

    }

    private void Respawn()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        int finalScene = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex == finalScene)
        {
            SceneManager.LoadScene(0);

        }
        else
        SceneManager.LoadScene(currentSceneIndex);

    }

    private void Rotate()
    {
        rigidBody.angularVelocity = Vector3.zero; // remove rotation due to physics

        float rotationframespeed = Time.deltaTime * RotationalThrust;
            
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.forward * rotationframespeed);
            else if (Input.GetKey(KeyCode.D))
                transform.Rotate(-Vector3.forward * rotationframespeed);
                    
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float MainThrustframespeed = Time.deltaTime * MainThrust;
            rigidBody.AddRelativeForce(Vector3.up * MainThrustframespeed*Time.deltaTime);

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(MainEngine);
                MainEngineParticle.Play();
            }
            
        }
        else
        { 
            audioSource.Stop();
        MainEngineParticle.Stop();
        }
    }

}

