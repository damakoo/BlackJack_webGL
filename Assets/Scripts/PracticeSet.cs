using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;


public class PracticeSet:MonoBehaviour
{
    BlackJackManager _BlackJackManager;
    private PhotonView _PhotonView;
    public CardState MySelectedCard { get; set; }
    public CardState YourSelectedCard { get; set; }
    public void SetMySelectedCard(CardState card)
    {
        MySelectedCard = card;
        _PhotonView.RPC("UpdateMySelectedCardOnAllClients", RpcTarget.Others, card.Number,card.MyCard);
    }
    [PunRPC]
    void UpdateMySelectedCardOnAllClients(int _Number, bool _MyCard)
    {
        // ここでカードデータを再構築
        CardState newCardState = new CardState { Number = _Number, MyCard = _MyCard };
    }
    public void SetYourSelectedCard(CardState card)
    {
        YourSelectedCard = card;
        _PhotonView.RPC("UpdateMySelectedCardOnAllClients", RpcTarget.Others, card.Number, card.MyCard);
    }
    [PunRPC]
    void UpdateYourSelectedCardOnAllClients(int _Number, bool _MyCard)
    {
        // ここでカードデータを再構築
        CardState newCardState = new CardState { Number = _Number, MyCard = _MyCard };
    }
    public List<List<int>> MyCardsPracticeList /*{ get; set; }*/ = new List<List<int>>();
    public List<List<int>> YourCardsPracticeList { get; set; } = new List<List<int>>();
    public List<int> FieldCardsPracticeList { get; set; } = new List<int>();
    public void SetMyCardsPracticeList(List<List<int>> _MyCardsPracticeList)
    {
        MyCardsPracticeList = _MyCardsPracticeList;
        _PhotonView.RPC("UpdateMyCardsPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_MyCardsPracticeList));
    }
    [PunRPC]
    void UpdateMyCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        MyCardsPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetYourCardsPracticeList(List<List<int>> _YourCardsPracticeList)
    {
        YourCardsPracticeList = _YourCardsPracticeList;
        _PhotonView.RPC("UpdateYourCardsPracticeListOnAllClients", RpcTarget.Others, SerializeCardList(_YourCardsPracticeList));
    }
    [PunRPC]
    void UpdateYourCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        YourCardsPracticeList = DeserializeCardList(serializeCards);
    }
    public void SetFieldCardsList(List<int> _FieldCardsPracticeList)
    {
        FieldCardsPracticeList = _FieldCardsPracticeList;
        _PhotonView.RPC("UpdateFieldCardsPracticeListOnAllClients", RpcTarget.Others, SerializeFieldCard(_FieldCardsPracticeList));
    }
    [PunRPC]
    void UpdateFieldCardsPracticeListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        FieldCardsPracticeList = DeserializeFieldCard(serializeCards);
    }

    private string SerializeCardList(List<List<int>> cards)
    {
        return JsonUtility.ToJson(new SerializationWrapper<List<List<int>>>(cards));
    }

    private List<List<int>> DeserializeCardList(string serializedCards)
    {
        return JsonUtility.FromJson<SerializationWrapper<List<List<int>>>>(serializedCards).data;
    }

    private string SerializeFieldCard(List<int> cards)
    {
        return JsonUtility.ToJson(new SerializationWrapper<List<int>>(cards));
    }

    private List<int> DeserializeFieldCard(string serializedCards)
    {
        return JsonUtility.FromJson<SerializationWrapper<List<int>>>(serializedCards).data;
    }

    [System.Serializable]
    private class SerializationWrapper<T>
    {
        public T data;

        public SerializationWrapper(T data)
        {
            this.data = data;
        }
    }

    public enum BlackJackStateList
    {
        BeforeStart,
        WaitForNextTrial,
        ShowMyCards,
        SelectCards,
        ShowResult,
        Finished,
    }
    public BlackJackStateList BlackJackState = BlackJackStateList.BeforeStart;

    public void SetBlackJackState(BlackJackStateList _BlackJackState)
    {
        BlackJackState = _BlackJackState;
        _PhotonView.RPC("UpdateBlackJackStateListOnAllClients", RpcTarget.Others, SerializeBlackJackState(_BlackJackState));
    }
    [PunRPC]
    void UpdateBlackJackStateListOnAllClients(string serializeCards)
    {
        // ここでカードデータを再構築
        BlackJackState = DeserializeBlackJackState(serializeCards);
    }

    private string SerializeBlackJackState(BlackJackStateList _BlackJackState)
    {
        return JsonUtility.ToJson(new SerializationWrapper<BlackJackStateList>(_BlackJackState));
    }

    private BlackJackStateList DeserializeBlackJackState(string serializedCards)
    {
        return JsonUtility.FromJson<SerializationWrapper<BlackJackStateList>>(serializedCards).data;
    }

    public int TrialAll;
    public int NumberofCards = 5;


    public int NumberofSet = 10;
    int FieldCards = 0;

    List<int> MyCards;
    List<int> YourCards;
    private void Start()
    {
        _PhotonView = GetComponent<PhotonView>();
        _BlackJackManager = GameObject.FindWithTag("Manager").GetComponent<BlackJackManager>();
    }
    public void UpdateParameter()
    {
        for (int i = 0; i < NumberofSet; i++)
        {
            DecidingCards(Random.Range(0,NumberofCards));
            FieldCardsPracticeList.Add(FieldCards);
            MyCardsPracticeList.Add(MyCards);
            YourCardsPracticeList.Add(YourCards);
        }
        SetMyCardsPracticeList(MyCardsPracticeList);
        SetYourCardsPracticeList(YourCardsPracticeList);
        SetFieldCardsList(FieldCardsPracticeList);
        _BlackJackManager.InitializeCard();
    }

    void DecidingCards(int _j)
    {
        DecideCards(_j);
        while (CheckmorethanfourCards())
        {
            DecideCards(_j);
        }
    }

    void DecideCards(int _j)
    {
        MyCards = new List<int>();
        YourCards = new List<int>();
        FieldCards = UnityEngine.Random.Range(1, 14);
        int _targetSum = 21 - FieldCards;
        if (_j > 0)
        {
            for (int i = 0; i < _j; i++)
            {
                int card = UnityEngine.Random.Range(1, 14);
                while (ValidityCheck(_targetSum, card, MyCards))
                {
                    card = UnityEngine.Random.Range(1, 14);
                }
                MyCards.Add(card);
                YourCards.Add(_targetSum - card);
            }
        }
        if (_j < NumberofCards)
        {
            for (int i = 0; i < NumberofCards - _j; i++)
            {
                int mycard = UnityEngine.Random.Range(1, 14);
                int yourcard = UnityEngine.Random.Range(1, 14);
                while (ValidityCheck_remaining(_targetSum, mycard, yourcard, MyCards, YourCards))
                {
                    mycard = UnityEngine.Random.Range(1, 14);
                    yourcard = UnityEngine.Random.Range(1, 14);
                }
                MyCards.Add(mycard);
                YourCards.Add(yourcard);
            }
        }
        ShuffleCards();
    }
    bool CheckmorethanfourCards()
    {
        bool Result = false;
        for (int k = 1; k < 14; k++)
        {
            int number = 0;
            if (FieldCards == k) number++;
            foreach (var i in MyCards) if (i == k) number++;
            foreach (var i in YourCards) if (i == k) number++;
            if (number > 4) Result = true;
        }
        return Result;
    }
    bool ValidityCheck(int _targetSum, int card, List<int> _MyCard)
    {
        bool Result = false;
        if (_targetSum <= card) Result = true;
        if (_targetSum - card > 13) Result = true;
        foreach (var eachcard in _MyCard) if (eachcard == card) Result = true;
        return Result;
    }
    bool ValidityCheck_remaining(int _targetSum, int mycard, int yourcard, List<int> _MyCard, List<int> _YourCard)
    {
        bool Result = false;
        if (mycard + yourcard == _targetSum) Result = true;
        //foreach (var eachcard in _MyCard) if (eachcard == mycard) Result = true;
        foreach (var eachcard in _MyCard) if (yourcard + eachcard == _targetSum) Result = true;
        return Result;
    }
    void ShuffleCards()
    {
        for (int i = 0; i < MyCards.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, MyCards.Count);
            int temp = MyCards[i];
            MyCards[i] = MyCards[randomIndex];
            MyCards[randomIndex] = temp;
        }
        for (int i = 0; i < YourCards.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, YourCards.Count);
            int temp = YourCards[i];
            YourCards[i] = YourCards[randomIndex];
            YourCards[randomIndex] = temp;
        }
    }
}
