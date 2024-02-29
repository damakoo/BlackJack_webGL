using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.Burst.CompilerServices;

public class DecideHostorClient : MonoBehaviour
{
    public bool HostReady { get; set; } = false;
    public bool ClientReady { get; set; } = false;
    [SerializeField] BlackJackManager _BlackJackManager;
    [SerializeField] GameObject WaitforAnother;
    bool _DecideHostorClient = false;
    public bool isConnecting = false;
    public PracticeSet _practiceSet { get; set; }
    private void Start()
    {
        _BlackJackManager._hostorclient = BlackJackManager.HostorClient.Host;
    }

    private void Update()
    {
        if(_practiceSet != null)
        {
            _BlackJackManager.SetPracticeSet(_practiceSet);
            if (_BlackJackManager._hostorclient == BlackJackManager.HostorClient.Host)
            {
                _BlackJackManager.UpdateParameter();
                _BlackJackManager.PhotonGameStartUI();
            }
            WaitforAnother.SetActive(false);
            this.gameObject.SetActive(false);

        }
    }
}