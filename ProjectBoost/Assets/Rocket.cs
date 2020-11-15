using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
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
    private void Thrust(){
      if(Input.GetKey(KeyCode.Space)){
              print("Space Pressed");
              rigidBody.AddRelativeForce(Vector3.up);

              if(!rocketThrusting.isPlaying){

                  rocketThrusting.Play();
              }
          }else {
            rocketThrusting.Stop();
          }
  }

  private void Rotate(){

      rigidBody.freezeRotation = true;//take manual control of rotation

    if(Input.GetKey(KeyCode.A)){

      transform.Rotate(Vector3.forward);

    } else if(Input.GetKey(KeyCode.D)){

      transform.Rotate(Vector3.back);
    }
      rigidBody.freezeRotation = false; // resume physics control of rotation
      
  }
} //end of class
