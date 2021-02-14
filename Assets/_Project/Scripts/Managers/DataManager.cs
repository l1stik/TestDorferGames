using UnityEngine;

namespace _Project.Scripts.Managers
{
    public static class DataManager
    {
        public static int GetCurrencyCount()
        {
            const string CURRENCY_COUNT = "CurrencyCount";
            
            return PlayerPrefs.GetInt(CURRENCY_COUNT);
        }
        
        public static void SetCurrencyCount(int count)
        {
            const string CURRENCY_COUNT = "CurrencyCount";

            var currentCount = GetCurrencyCount();
            currentCount += count;
            
            PlayerPrefs.SetInt(CURRENCY_COUNT, currentCount);
        }
        
        public static int GetRecord()
        {
            const string RECORD_LVL = "RecordLvL";
            
            return PlayerPrefs.GetInt(RECORD_LVL);
        }
        
        public static void SetRecord(int count)
        {
            const string RECORD_LVL = "RecordLvL";
            
            if (GetRecord() < count)
            {
                PlayerPrefs.SetInt(RECORD_LVL, count);
            }
        }
    }
}