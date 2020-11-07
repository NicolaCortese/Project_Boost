using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float RotationalThrust = 200f;
    [SerializeField] float MainThrust = 1000f;
    Rigidbody rigidBody;
    AudioSource audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Thrust();

    }
    void OnCollisionEnter(Collision collision)
        {
            switch (collision.gameObject.tag)
            {
                case "friendly":
                    print("smooth");
                    //do nothing
                    break;
                case "baddy":
                    print("kaboom!");
                    break;


            }
                
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

