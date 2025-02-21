using System;
using UnityEngine;

public class RotateTank : MonoBehaviour
{
   // Variable 
   //public float rotateSpeed;
   
   // Rotation Function
   public void Rotate(float speed)
   {
      // Rotate tank along the Y-Axis
      transform.Rotate(0, speed * Time.deltaTime, 0);
   }

   public void Update()
   {
      if (Input.GetKey(KeyCode.A))
      {
         Rotate(-50f);
      }

      if (Input.GetKey(KeyCode.D))
      {
         Rotate(50f);
      }
   }
}
