using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GongsFistController : MonoBehaviour
{

    GameObject gong;
    //bool awake = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

      if (gong.GetComponent<GongController>().awake)
      {
        //gong.transform.
      }

    }
}
