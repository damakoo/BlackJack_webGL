using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class PracticeSet
{
    public static List<List<int>> MyCardsPracticeList = new List<List<int>>();
    public static List<List<int>> YourCardsPracticeList = new List<List<int>>();
    public static List<int> FieldCardsPracticeList = new List<int>();

    public static int TrialAll;
    public static int NumberofCards = 5;


    public static int NumberofSet = 10;
    static int FieldCards = 0;

    static List<int> MyCards;
    static List<int> YourCards;
    public static void UpdateParameter()
    {
        for (int i = 0; i < NumberofSet; i++)
        {
                DecidingCards(Random.Range(0,NumberofCards));
                FieldCardsPracticeList.Add(FieldCards);
                MyCardsPracticeList.Add(MyCards);
                YourCardsPracticeList.Add(YourCards);
        }       
    }

    static void DecidingCards(int _j)
    {
        DecideCards(_j);
        while (CheckmorethanfourCards())
        {
            DecideCards(_j);
        }
    }

    static void DecideCards(int _j)
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
    static bool CheckmorethanfourCards()
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
    static bool ValidityCheck(int _targetSum, int card, List<int> _MyCard)
    {
        bool Result = false;
        if (_targetSum <= card) Result = true;
        if (_targetSum - card > 13) Result = true;
        foreach (var eachcard in _MyCard) if (eachcard == card) Result = true;
        return Result;
    }
    static bool ValidityCheck_remaining(int _targetSum, int mycard, int yourcard, List<int> _MyCard, List<int> _YourCard)
    {
        bool Result = false;
        if (mycard + yourcard == _targetSum) Result = true;
        //foreach (var eachcard in _MyCard) if (eachcard == mycard) Result = true;
        foreach (var eachcard in _MyCard) if (yourcard + eachcard == _targetSum) Result = true;
        return Result;
    }
    static void ShuffleCards()
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
