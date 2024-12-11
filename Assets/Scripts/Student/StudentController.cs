using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class StudentController : MonoBehaviour
{
    private DatabaseReference dbReference;

    public bool isNeedToFindTeacher;

   private float timerToSerch = 10f;
   private bool isFirstSearch = true;

   private string currentTeacher;
   private bool isOnline = true;
   private void Start()
   {
       dbReference = FirebaseDatabase.DefaultInstance.RootReference;
       TeacherFind("Alex Kupris");
   }

   public bool TeacherFind(string teacher)
   {
       currentTeacher = teacher;
       StartCoroutine(GetData());
       Debug.Log(isOnline);
       return isOnline;
   }

   private void Update()
   {
       if(!isNeedToFindTeacher)
           return;

       timerToSerch -= Time.deltaTime;
       if (timerToSerch <= 0)
       {
         //  timerToSerch = 10;
         //  StartCoroutine(GetData());
       }
   }

   private void CheckOnlineTeache(Dictionary<string, object> teachersData)
   {
       
           foreach (var name in teachersData.Values)
           {
               Dictionary<string, object> values = name as Dictionary<string, object>;
               string currentName = values["Name"].ToString();

               if (currentName == currentTeacher)
               {
                   isOnline = (bool)values["Online"];

                   if (isOnline)
                   {
                       Debug.Log($"Teache {currentTeacher} is Online");
                       //Navigate to teacher
                   }
                   else
                   {
                       Debug.Log($"Teache {currentTeacher} is Offline");
                       if (isFirstSearch)
                       {
                           //Show Message to wait teacher;
                       }
                   }
               }
           }
   }

   IEnumerator GetData()
   {
      var task = dbReference.Child("Teachers").GetValueAsync();
      while (!task.IsCompleted)
      {
          yield return null;
      }
      
      DataSnapshot snapshot = task.Result;
      Dictionary<string, object> dict = snapshot.Value as Dictionary<string, object>;
      CheckOnlineTeache(dict);
   }
 
}
