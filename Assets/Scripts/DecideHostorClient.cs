using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideHostorClient : MonoBehaviour
{
    [SerializeField] BlackJackManager _BlackJackManager;

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
                    Destroy(this);
                }
                else if (hit.collider.gameObject.name == "Client")
                {
                    _BlackJackManager._hostorclient = BlackJackManager.HostorClient.Client;
                    Destroy(this);
                }
            }
        }
    }
}
