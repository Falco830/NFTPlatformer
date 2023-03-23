using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DropItem : MonoBehaviour
{

  public GameObject itemToDrop;
  public int raisePosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (this.gameObject.GetComponent<Enemy>() && this.gameObject.GetComponent<Enemy>().health <= 0)
      {
        DroppingItem();
      }
    }

  public void DroppingItem()
  {
    Instantiate(itemToDrop, this.transform.position + (Vector3.up * raisePosition), this.transform.rotation);
  }

  private void OnDestroy()
  {
    if (this.gameObject.GetComponent<Enemy>() && this.gameObject.GetComponent<Enemy>().health <= 0)
    {
      DroppingItem();
    }
      //Instantiate(itemToDrop, this.transform.position + (Vector3.up * raisePosition), this.transform.rotation);
  }

}
