using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackJackRecorder : MonoBehaviour
{
    [SerializeField] CSVWriter _CSVWriter;
    public List<int> MyNumberList { get; set; } = new List<int>();
    public List<int> YourNumberList { get; set; } = new List<int>();
    public List<int> ScoreList { get; set; } = new List<int>();
    private List<List<int>> MyCardsPracticeList => PracticeSet.MyCardsPracticeList;
    private List<List<int>> YourCardsPracticeList => PracticeSet.YourCardsPracticeList;
    private List<int> FieldCardsPracticeList => PracticeSet.FieldCardsPracticeList;
    private int TrialAll => PracticeSet.TrialAll;

    public void RecordResult(int mynumber, int yournumber, int score)
    {
        MyNumberList.Add(mynumber);
        YourNumberList.Add(yournumber);
        ScoreList.Add(score);
    }

    public void WriteResult()
    {
        string Content = "";
        Content += "FieldNumber";
        for (int i = 0; i < MyCardsPracticeList[0].Count; i++) Content += ",MyCards" + (i + 1).ToString();
        for (int i = 0; i < YourCardsPracticeList[0].Count; i++) Content += ",YourCards" + (i + 1).ToString();
        Content += ",MyNumber,YourNumber,Score\n";
        for(int i = 0;i < TrialAll; i++)
        {
            Content += FieldCardsPracticeList[i].ToString();
            for (int j = 0; j < MyCardsPracticeList[i].Count; j++) Content += "," + MyCardsPracticeList[i][j].ToString();
            for (int j = 0; j < YourCardsPracticeList[i].Count; j++) Content += "," + YourCardsPracticeList[i][j].ToString();
            Content += "," + MyNumberList[i].ToString() + "," + YourNumberList[i].ToString() + "," + ScoreList[i].ToString() + "\n";
        }
        _CSVWriter.WriteCSV(Content);
    }
}
