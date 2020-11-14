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
    enum State { Alive, Dying, Transcending };
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {// todo remove sound on death
        if (state == State.Alive) 
        {
            Rotate();
            Thrust();
        }

    }
    void OnCollisionEnter(Collision collision)
        {
        if (state != State.Alive)
        {
            
            return;
        }
            switch (collision.gameObject.tag)
            {
            case "friendly":
                //do nada
                break;
            case "finish":
                state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(Winner);
                WinnerParticle.Play();
                
                Invoke("LoadNextScene", levelDelay); // change timing
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(Kaboom);
                KaboomParticle.Play();
                Invoke("LoadFirstScene", levelDelay);

                break;


            }
                
        }
    private void LoadNextScene()
    {
        
        
        
        SceneManager.LoadScene(1);

    }

    private void LoadFirstScene()
    {
        
        SceneManager.LoadScene(0);

    }

    private void Rotate()
    {
        {
            
            float rotationframespeed = Time.deltaTime * RotationalThrust;

            rigidBody.freezeRotation = true;
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.forward * rotationframespeed);
            else if (Input.GetKey(KeyCode.D))
                transform.Rotate(-Vector3.forward * rotationframespeed);
            rigidBody.freezeRotation = false;
        }
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

