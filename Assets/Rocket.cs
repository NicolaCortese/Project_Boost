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
                    print("smooth");
                    //do nothing
                    break;
            case "finish":
                state = State.Transcending;
                
                Invoke("LoadNextScene",2f); // change timing
                break;
            default:
                state = State.Dying;
                print("kaboom");
                Invoke("LoadFirstScene", 2f);

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
            rigidBody.AddRelativeForce(Vector3.up * MainThrustframespeed);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
            audioSource.Stop();
    }

}

