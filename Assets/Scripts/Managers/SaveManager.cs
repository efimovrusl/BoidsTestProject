using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

namespace Managers
{
public static class SaveManager
{
    public static List<SimulationStats> GetAllSimulationStats()
    {
        var statsList = new List<SimulationStats>();
        var jsonString = new JSONString( PlayerPrefsManager.AllStatsJsonString );
        var array = JSON.Parse( jsonString ).AsArray;
        if ( array == null ) array = new JSONArray();
        foreach ( JSONObject jsonObject in array )
        {
            SimulationStats stats = new SimulationStats
            {
                FrontFlockSize = jsonObject["FrontFlockSize"],
                BackFlockSize = jsonObject["BackFlockSize"],
                FrameRate = jsonObject["FrameRate"]
            };
            statsList.Add( stats );
        }

        return statsList;
    }

    public static void AddSimulationStats( SimulationStats stats )
    {
        var jsonObject = new JSONObject();
        jsonObject.Add( "FrontFlockSize", stats.FrontFlockSize );
        jsonObject.Add( "BackFlockSize", stats.BackFlockSize );
        jsonObject.Add( "FrameRate", stats.FrameRate );
        var jsonString = new JSONString( PlayerPrefsManager.AllStatsJsonString );
        var array = JSON.Parse( jsonString ).AsArray;
        if ( array == null ) array = new JSONArray();
        array.Add( jsonObject );
        var newJsonString = new JSONString( array.ToString() );
        PlayerPrefsManager.AllStatsJsonString = newJsonString;
    }


    public struct SimulationStats
    {
        public int FrontFlockSize;
        public int BackFlockSize;
        public float FrameRate;

        public static int Comparer( SimulationStats s1, SimulationStats s2 )
        {
            var s1SquaresSum = MathF.Pow( s1.FrontFlockSize, 2 ) + MathF.Pow( s1.BackFlockSize, 2 );
            var s2SquaresSum = MathF.Pow( s2.FrontFlockSize, 2 ) + MathF.Pow( s2.BackFlockSize, 2 );
            return s1SquaresSum > s2SquaresSum ? -1 :
                s1SquaresSum < s2SquaresSum ? 1 :
                Math.Abs( s1SquaresSum - s2SquaresSum ) < 0.00001 ? s1.FrameRate > s2.FrameRate ? -1 : 1 : 1;
        }
    }


    private static class PlayerPrefsManager
    {
        private const string AllStatsJsonStringKey = "allStatsJsonString";

        public static string AllStatsJsonString
        {
            get
            {
                if ( !PlayerPrefs.HasKey( AllStatsJsonStringKey ) ) AllStatsJsonString = "";
                return PlayerPrefs.GetString( AllStatsJsonStringKey );
            }
            set
            {
                PlayerPrefs.SetString( AllStatsJsonStringKey, value );
                PlayerPrefs.Save();
            }
        }
    }
}
}