using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    private Rigidbody rigidBody;
    private AudioSource rocketThrusting;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rocketThrusting = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
      Thrust();
      Rotate();
    }

    private void OnCollisionEnter(Collision collision){
      switch(collision.gameObject.tag){
        case "Friendly": 
          break;
        case "Fuel": print("Fuel");
          break;
        default: print("sup bish");
          break;
      }
    }
    private void Thrust(){
      if(Input.GetKey(KeyCode.Space)){
             
              rigidBody.AddRelativeForce(Vector3.up * mainThrust);

              if(!rocketThrusting.isPlaying){

                  rocketThrusting.Play();
              }
          }else {
            rocketThrusting.Stop();
          }
  }

    private void Rotate(){

    rigidBody.freezeRotation = true;//take manual control of rotation
     float rotationThisFrame = Time.deltaTime * rcsThrust;

    if(Input.GetKey(KeyCode.A)){

     
      transform.Rotate(Vector3.forward * rotationThisFrame);

    } else if(Input.GetKey(KeyCode.D)){
      
      transform.Rotate(Vector3.back * rotationThisFrame);
    }
      rigidBody.freezeRotation = false; // resume physics control of rotation
      
  }
} //end of class
