using MoralisUnity.Kits.AuthenticationKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewSwitchController : MonoBehaviour
{
  [SerializeField]
  private GameObject gameUiObject;

  [SerializeField]
  private GameObject authenticationKitObject;

  private AuthenticationKit authKit;
  // Start is called before the first frame update
  void Start()
    {
    authKit = authenticationKitObject.GetComponent<AuthenticationKit>();

    authenticationKitObject.SetActive(true);
    gameUiObject.SetActive(false);
    }

  public void OnConnected()
  {
    authenticationKitObject.SetActive(false);
    gameUiObject.SetActive(true);
  }

  public void Quit()
  {
    authKit.Disconnect();

    authenticationKitObject.SetActive(true);
    gameUiObject.SetActive(false);
  }

}
