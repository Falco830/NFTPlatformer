using MoralisUnity.Kits.AuthenticationKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewSwitchController : MonoBehaviour
{
  [SerializeField]
  private GameObject gameUiObject;

  [SerializeField]
  private GameObject authenticationKitObject;

  [SerializeField]
  private GameObject characterSelectorObject;

  [SerializeField]
  private GameObject mintNFTObject;

  [SerializeField]
  private AuthenticationKit authKit;
  // Start is called before the first frame update
  void Start()
    {
      authKit = authenticationKitObject.GetComponent<AuthenticationKit>();
      if (authKit.State.ToString().Equals("Connected"))
      {
        OnConnected();
      }
      else
      {
        authenticationKitObject.SetActive(true);
        gameUiObject.SetActive(false);
      }

    }

  public void MintAnNFT()
  {
    authenticationKitObject.SetActive(false);
    characterSelectorObject.SetActive(false);
    mintNFTObject.SetActive(true);
  }
  public void CharacterSelector()
  {
    authenticationKitObject.SetActive(false);
    characterSelectorObject.SetActive(true);
    mintNFTObject.SetActive(false);
  }

  public void OnConnected()
  {
    authenticationKitObject.SetActive(false);
    gameUiObject.SetActive(true);
    characterSelectorObject.SetActive(true);   
    mintNFTObject.SetActive(false);
  }

  public void Quit()
  {
    authKit.Disconnect();

    authenticationKitObject.SetActive(true);
    gameUiObject.SetActive(false);
  }

}
