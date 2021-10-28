using System;
using System.Threading;
using UnityEngine;

public static class IdManager
{
    public static string AssignId(string currentID)
    {
        if (string.IsNullOrWhiteSpace(currentID))
        {
            Thread.Sleep(15); //to avoid assigning duplicate id
            string date = DateTime.Now.ToString("yyMMddHHmmssff");
            return date;
        }
        else
        {
            Debug.LogWarning("Prefab has assigned id: " + currentID);
            return null;
        }
    }

    public static void DisplayDuplicatedIdError(string object1Name, string object2Name, string id)
    {
        Debug.LogError(String.Concat(object1Name, " and ", object2Name, " have duplicated ID: " + id));
    }
}