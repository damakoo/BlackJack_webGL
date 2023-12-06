using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class PracticeSet
{
    public static List<List<int>> MyCardsPracticeList = new List<List<int>>();
    public static List<List<int>> YourCardsPracticeList = new List<List<int>>();
    public static List<int> FieldCardsPracticeList = new List<int>();

    public static int TrialAll;
    public static int NumberofCards;
    public static void UpdateParameter(string _filepath)
    {

        // CSVファイルを全ての行で読み込む
        string[] lines = File.ReadAllLines(_filepath);

        TrialAll = lines.Length - 1;
        NumberofCards = (lines[0].Split(',').Length - 1) / 2;
        
        for (int i = 0; i < TrialAll; i++)
        {
            string[] columns = lines[i + 1].Split(',');
            FieldCardsPracticeList.Add(int.Parse(columns[0]));
            List<int> MyCardsList0 = new List<int>();
            List<int> YourCardsList0 = new List<int>();
            for (int j = 0; j < NumberofCards; j++)
            {
                MyCardsList0.Add(int.Parse(columns[j+1]));
                YourCardsList0.Add(int.Parse(columns[j+1+NumberofCards]));
            }
            MyCardsPracticeList.Add(MyCardsList0);
            YourCardsPracticeList.Add(YourCardsList0);
        }
    }
}
