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
    [SerializeField] BlackJackRecorder _blackJackRecorder;
    [SerializeField] TextMeshProUGUI MyScoreUI;
    [SerializeField] TextMeshProUGUI YourScoreUI;
    private enum BlackJackState
    {
        WaitForNextTrial,
        ShowMyCards,
        SelectCards,
        ShowResult,
        Finished,
    }
    private enum HowShowCard
    {
        KeyBoard,
        Time
    }
    [SerializeField] HowShowCard _HowShowCard;
    BlackJackState _nowstate = BlackJackState.WaitForNextTrial;
    int nowTrial = 0;
    float nowTime = 0;
    private CardState MySelectedCard;
    private CardState YourSelectedCard;
    private int Score = 0;
    // Start is called before the first frame update
    void Start()
    {
        FinishUI.SetActive(false);
        PracticeSet.UpdateParameter();
        _cardslist.InitializeCards();
        MoveToWaitForNextTrial();
    }

    // Update is called once per frame
    void Update()
    {

        if (_nowstate == BlackJackState.WaitForNextTrial)
        {
            if (Input.GetKeyDown(KeyCode.Space)) MoveToShowMyCards();
        }
        else if (_nowstate == BlackJackState.ShowMyCards)
        {
            if(_HowShowCard == HowShowCard.KeyBoard)
            {
                if (Input.GetKeyDown(KeyCode.Space)) MoveToSelectCards();
            }
            else if(_HowShowCard == HowShowCard.Time)
            {
                nowTime += Time.deltaTime;
                if (nowTime > ShowMyCardsTime)
                {
                    nowTime = 0;
                    MoveToSelectCards();
                }
            }

        }
        else if (_nowstate == BlackJackState.SelectCards)
        {
            nowTime += Time.deltaTime;
            BlackJacking();
            if (nowTime > TimeLimit) MoveToShowResult();
        }
        else if (_nowstate == BlackJackState.ShowResult)
        {
            if (Input.GetKeyDown(KeyCode.Space)) MoveToWaitForNextTrial();
        }
    }
    

    void BlackJacking()
    {
        // マウスボタンがクリックされたか確認
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            // レイキャストを使用してオブジェクトを検出
            if (hit && hit.collider.gameObject.CompareTag("Card"))
            {
                if (hit.collider.gameObject.TryGetComponent<CardState>(out CardState thisCard))
                {
                    if (thisCard.MyCard)
                    {
                        MySelectedCard.UnClicked();
                        MySelectedCard = thisCard;
                        MySelectedCard.Clicked();
                    }
                    else
                    {
                        YourSelectedCard.UnClicked();
                        YourSelectedCard = thisCard;
                        YourSelectedCard.Clicked();
                    }
                }
            }
        }
    }

    void MoveToShowMyCards()
    {
        _cardslist.MyCardsOpen();
        _nowstate = BlackJackState.ShowMyCards;
    }
    void MoveToSelectCards()
    {
        _cardslist.AllOpen();
        _nowstate = BlackJackState.SelectCards;

    }
    void MoveToShowResult()
    {
        MySelectedCard.Clicked();
        YourSelectedCard.Clicked();
        foreach (var card in _cardslist.MyCardsList_opponent)
        {
            if (card.Number == YourSelectedCard.Number) card.Clicked();
        }
        foreach (var card in _cardslist.YourCardsList_opponent)
        {
            if (card.Number == MySelectedCard.Number) card.Clicked();
        }
        Score = CalculateResult();
        _blackJackRecorder.RecordResult(MySelectedCard.Number,YourSelectedCard.Number,Score);
        _nowstate = BlackJackState.ShowResult;
        MyScoreUI.text = Score.ToString();
        YourScoreUI.text = Score.ToString();
        nowTime = 0;
        nowTrial += 1;
        if(nowTrial == PracticeSet.TrialAll)
        {
            _nowstate = BlackJackState.Finished;
            FinishUI.SetActive(true);
            _blackJackRecorder.WriteResult();
        }
    }
    void MoveToWaitForNextTrial()
    {
        _cardslist.AllClose();
        _nowstate = BlackJackState.WaitForNextTrial;
        _cardslist.SetCards(nowTrial);
        MyScoreUI.text = "";
        YourScoreUI.text = "";
        MySelectedCard = _cardslist.MyCardsList[0];
        YourSelectedCard = _cardslist.YourCardsList[0];
    }
    private int CalculateResult()
    {
        return (MySelectedCard.Number + YourSelectedCard.Number + PracticeSet.FieldCardsPracticeList[nowTrial] > 21)?0:MySelectedCard.Number + YourSelectedCard.Number + PracticeSet.FieldCardsPracticeList[nowTrial];
    }
}
