using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlackJackManager : MonoBehaviour
{
    [SerializeField] CardsList _cardslist;
    [SerializeField] int TimeLimit;
    [SerializeField] int ShowMyCardsTime = 10;
    [SerializeField] GameObject FinishUI;
    //[SerializeField] BlackJackRecorder _blackJackRecorder;
    [SerializeField] TextMeshProUGUI MyScoreUI;
    [SerializeField] TextMeshProUGUI YourScoreUI;

    PracticeSet _PracticeSet;

    public enum HostorClient
    {
        Host,
        Client
    }
    public HostorClient _hostorclient;
    private enum HowShowCard
    {
        KeyBoard,
        Time
    }
    [SerializeField] HowShowCard _HowShowCard;
    int nowTrial = 0;
    float nowTime = 0;
    private int Score = 0;
    public bool hasPracticeSet { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        FinishUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPracticeSet)
        {

            if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.BeforeStart)
            {
                if (Input.GetKeyDown(KeyCode.Space)) MoveToWaitForNextTrial();
            }
            else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.WaitForNextTrial)
            {
                //if (Input.GetKeyDown(KeyCode.Space)) MoveToShowMyCards();
                nowTime += Time.deltaTime;
                if (nowTime > ShowMyCardsTime)
                {
                    nowTime = 0;
                    MoveToShowMyCards();
                }
            }
            else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.ShowMyCards)
            {
                if (_HowShowCard == HowShowCard.KeyBoard)
                {
                    if (Input.GetKeyDown(KeyCode.Space)) MoveToSelectCards();
                }
                else if (_HowShowCard == HowShowCard.Time)
                {
                    nowTime += Time.deltaTime;
                    if (nowTime > ShowMyCardsTime)
                    {
                        nowTime = 0;
                        MoveToSelectCards();
                    }
                }

            }
            else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.SelectCards)
            {
                nowTime += Time.deltaTime;
                BlackJacking();
                if (nowTime > TimeLimit) MoveToShowResult();
            }
            else if (_PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.ShowResult)
            {
                //if (Input.GetKeyDown(KeyCode.Space)) MoveToWaitForNextTrial();
                nowTime += Time.deltaTime;
                if (nowTime > ShowMyCardsTime)
                {
                    nowTime = 0;
                    MoveToWaitForNextTrial();
                }
            }
        }
    }
    public void SetPracticeSet(PracticeSet _practiceset)
    {
        _PracticeSet = _practiceset;
        _cardslist.SetPracticeSet(_practiceset);
        hasPracticeSet = true;
    }


    public void UpdateParameter()
    {
        _PracticeSet.UpdateParameter();
    }
    public void InitializeCard()
    {
        _cardslist.InitializeCards();
    }
    void BlackJacking()
    {
        // �}�E�X�{�^�����N���b�N���ꂽ���m�F
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            // ���C�L���X�g���g�p���ăI�u�W�F�N�g�����o
            if (hit && hit.collider.gameObject.CompareTag("Card"))
            {
                if (hit.collider.gameObject.TryGetComponent<CardState>(out CardState thisCard))
                {
                    if (thisCard.MyCard)
                    {
                        _PracticeSet.MySelectedCard.UnClicked();
                        _PracticeSet.MySelectedCard = thisCard;
                        _PracticeSet.MySelectedCard.Clicked();
                    }
                    else
                    {
                        _PracticeSet.YourSelectedCard.UnClicked();
                        _PracticeSet.YourSelectedCard = thisCard;
                        _PracticeSet.YourSelectedCard.Clicked();
                    }
                }
            }
        }
    }

    void MoveToShowMyCards()
    {
        _cardslist.MyCardsOpen();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowMyCards;
    }
    void MoveToSelectCards()
    {
        _cardslist.AllOpen();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.SelectCards;

    }
    void MoveToShowResult()
    {
        _PracticeSet.MySelectedCard.Clicked();
        _PracticeSet.YourSelectedCard.Clicked();
        foreach (var card in _cardslist.MyCardsList_opponent)
        {
            if (card.Number == _PracticeSet.YourSelectedCard.Number) card.Clicked();
        }
        foreach (var card in _cardslist.YourCardsList_opponent)
        {
            if (card.Number == _PracticeSet.MySelectedCard.Number) card.Clicked();
        }
        Score = CalculateResult();
        //_blackJackRecorder.RecordResult(_PracticeSet.MySelectedCard.Number, _PracticeSet.YourSelectedCard.Number,Score);
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.ShowResult;
        MyScoreUI.text = Score.ToString();
        YourScoreUI.text = Score.ToString();
        nowTime = 0;
        nowTrial += 1;
        if(nowTrial == _PracticeSet.TrialAll)
        {
            _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.Finished;
            FinishUI.SetActive(true);
            //_blackJackRecorder.WriteResult();
        }
    }
    void MoveToWaitForNextTrial()
    {
        _cardslist.AllClose();
        _PracticeSet.BlackJackState = PracticeSet.BlackJackStateList.WaitForNextTrial;
        _cardslist.SetCards(nowTrial);
        MyScoreUI.text = "";
        YourScoreUI.text = "";
        _PracticeSet.MySelectedCard = _cardslist.MyCardsList[0];
        _PracticeSet.YourSelectedCard = _cardslist.YourCardsList[0];
    }
    private int CalculateResult()
    {
        return (_PracticeSet.MySelectedCard.Number + _PracticeSet.YourSelectedCard.Number + _PracticeSet.FieldCardsPracticeList[nowTrial] > 21)?0: _PracticeSet.MySelectedCard.Number + _PracticeSet.YourSelectedCard.Number + _PracticeSet.FieldCardsPracticeList[nowTrial];
    }

}
