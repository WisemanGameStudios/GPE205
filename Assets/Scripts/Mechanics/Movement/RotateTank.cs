using System;
using UnityEngine;

public class RotateTank : MonoBehaviour
{
   // Variable 
   public float rotateSpeed;
   
   // Rotation Function
   public void Rotate(float rotateSpeed)
   {
      // Rotate tank along the Y-Axis
      transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
   }

   public void Update()
   {
      if (Input.GetKey(KeyCode.A))
      {
         Rotate(-rotateSpeed);
      }

      if (Input.GetKey(KeyCode.D))
      {
         Rotate(rotateSpeed);
      }
   }
}
