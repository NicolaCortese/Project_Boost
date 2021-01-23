using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 MovementVector;

    [SerializeField] float period;

    float MovementFactor;

    Vector3 StartingPos;


    // Start is called before the first frame update
    void Start()
    {
        StartingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = MovementVector * MovementFactor;
        transform.position = StartingPos + offset;
        if (period <= Mathf.Epsilon)
        {
            return;
        }
        float cycles = Time.time / period;
        const float tau = 2 * Mathf.PI;

        float rawSinwave = Mathf.Sin(tau * cycles);
        MovementFactor = rawSinwave / 2f + 0.5f;
        




    }
}
