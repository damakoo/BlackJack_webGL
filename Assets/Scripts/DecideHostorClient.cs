using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DecideHostorClient : MonoBehaviour
{
    [SerializeField] BlackJackManager _BlackJackManager;
    [SerializeField] GameObject WaitforAnother;
    bool tryConnetcion = false;
    public bool isConnecting = false;
    public PracticeSet _practiceSet { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            if (hit)
            {

                if (hit.collider.gameObject.name == "Host")
                {
                    _BlackJackManager._hostorclient = BlackJackManager.HostorClient.Host;
                    tryConnetcion = true;
                }
                else if (hit.collider.gameObject.name == "Client")
                {
                    _BlackJackManager._hostorclient = BlackJackManager.HostorClient.Client;
                    tryConnetcion = true;
                }
            }

        }
        if (tryConnetcion)
        {
            if (isConnecting)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
                {
                    _BlackJackManager.SetPracticeSet(_practiceSet);
                    if (_BlackJackManager._hostorclient == BlackJackManager.HostorClient.Host) _BlackJackManager.UpdateParameter();
                    this.gameObject.SetActive(false);
                }
                else
                {
                    WaitforAnother.SetActive(true);
                }
            }
            else
            {
                WaitforAnother.SetActive(true);
            }
        }
    }
}
