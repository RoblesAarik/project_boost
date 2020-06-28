using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 240f;
    [SerializeField] float mainThrust = 5f;
    
    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
        Thruster();
        Rotate();
    }

    void Thruster() {
        if (Input.GetKey(KeyCode.Space)) {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if(!audioSource.isPlaying) {

            audioSource.Play();

            } else {
                audioSource.Stop();
            }
        }
    }

    void Rotate() {

        rigidBody.freezeRotation = true;  // Manual control of rotation
        
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationThisFrame);

        } else if (Input.GetKey(KeyCode.D)) {

            transform.Rotate(-Vector3.forward * rotationThisFrame);

        }

       rigidBody.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision) {
        switch (collision.gameObject.tag) {
            case "Friendly":
                print("OK");
            break;
            
            case "Fuel":
                print("Fuel");
            break;

            default: 
                print("Dead");
            break;
        }
    }
    
}
