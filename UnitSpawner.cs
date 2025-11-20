using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject TheUnitGameObjects;

    public float SpawnAmount = 5f;
    public float i = 0f;

    private float UnitXPos;
    private float UnitYPos;
    private float UnitZPos;
    private float UnitZPosAdder;

    private bool HasSpawned = false;

    public int CentralNamingInt;
    public string TheLogPath;
    public bool IsLucky = false;
    public string AllUnitString;
    public string StartingString;
    public bool YearJustEnded = false;
    public bool SimulationJustEnded = false;
    public float TotalHourlyWageOfAllUnits;
    private int NumberOfUnit = 0;
    public float AverageOfTotalHourlyWageOfAllUnits = 0;
    public string AllCentralString;
    private float PreviousLuckFactor;
    private float PreviousLuckFactor2;
    private string MostLuckyUnitName;
    private string MostUnLuckyUnitName;
    private float MostLuckyLifeTimeWages = 0;
    private float MostUnLuckyLifeTimeWages = 0;
    private float PreviousTotalSuccessfulPromotion = 0;
    private string MostSuccessfullyPromotedUnitName;
    private float MostSuccessfullyPromotedUnitNumber;
    private float PreviousLifeTimeHoursWorked = 0;
    private float MostHardWorkingLifeTimeWages = 0;
    private string MostHardWorkingName;
    private float PreviousRichestUnitLifeTimeWages;
    private string MostRichestUnitName;
    private float PreviousPoorestUnitLifeTimeWages = 5000000;
    private string MostPoorestUnitName;
    private float PreviousLifeTimeHoursWorked2 = 0;
    private float MostLazyLifeTimeWages = 0;
    private string MostLazyUnitName;
    private int NumberOfUnitSimulationEnded = 0;
    private float TotalSuccessfulPromotionNumber;
    private float AverageSuccessfulPromotionNumber;
    private float MedianLifeTimeWagesEarned = 0;
    private float TotalLuckFactor;
    private float AverageLuckFactor;

    void Start()
    {
       CreateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (HasSpawned == false)
        {
            for (i = 1; i <= SpawnAmount; i++)
            {
                SpawnUnit();
            }
            HasSpawned = true;
        }

        /*
        if (YearJustEnded) 
        {
            AddAllUnitCentralString();
            YearJustEnded = false;
        }
        */

        if (SimulationJustEnded)
        {
            AddAllUnitCentralString();
            SimulationJustEnded = false;
        }

    }

    public void SpawnUnit()
    {
        Instantiate(TheUnitGameObjects, GetNextPos(), Quaternion.identity);
    }

    Vector3 GetNextPos()
    {
        UnitZPosAdder += 20;

        UnitXPos = TheUnitGameObjects.transform.position.x;
        UnitYPos = TheUnitGameObjects.transform.position.y;
        UnitZPos = (TheUnitGameObjects.transform.position.z + UnitZPosAdder);

        Vector3 newPos = new Vector3(UnitXPos, UnitYPos, UnitZPos);
        return newPos;
    }

    private void CreateText()
    {
        
        // Create a timestamp for the file name
        string PathTimestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");



        //Create File if doesnt exist Writeall rewrites everything
        if (IsLucky)
        {
            // Path of the File inside "HardWorkTextFolder" under Assets
            string fileName = "Lucky_" + PathTimestamp + ".txt";
            TheLogPath = Path.Combine(Application.dataPath, "HardWorkTextFolder", fileName);

            StartingString += "Lucky Log \n";
        }
        else
        {
            // Path of the File inside "HardWorkTextFolder" under Assets
            string fileName = "Dave_" + PathTimestamp + ".txt";
            TheLogPath = Path.Combine(Application.dataPath, "HardWorkTextFolder", fileName);

            StartingString += "HardWork Log \n";
        }


        //New Content of the file (Just A Variable)
        string NewContent = "Login date:" + System.DateTime.Now + "\n\n";

        //Add text to it
        StartingString += NewContent;
    }

    private GameObject[] HardworkerGameObjects;
    public void AddAllUnitCentralString()
    {
        AllCentralString = StartingString;
        AllUnitString = "";
        NumberOfUnit = 0;
        TotalHourlyWageOfAllUnits = 0;
        PreviousPoorestUnitLifeTimeWages = 5000000;
        PreviousLifeTimeHoursWorked = 0;
        PreviousLifeTimeHoursWorked2 = 147840;
        NumberOfUnitSimulationEnded = 0;
        PreviousLuckFactor2 = 7;
        List<float> LifeTimeWagesEarnedList = new List<float>();

        // Find all GameObjects with the tag "hardworker"
        if (IsLucky)
        {
            HardworkerGameObjects = GameObject.FindGameObjectsWithTag("lucky");
        }
        else
        {
            HardworkerGameObjects = GameObject.FindGameObjectsWithTag("hardworker");
        }

        // Loop through each GameObject found
        foreach (GameObject AHardWorkerGameObject in HardworkerGameObjects)
        {
            // Try to get the Hardworker script on this GameObject
            HardWorker HardWorkerScript = AHardWorkerGameObject.GetComponent<HardWorker>();

            if (HardWorkerScript != null)
            {
                AllUnitString += HardWorkerScript.UnitCentralString;
                TotalHourlyWageOfAllUnits += HardWorkerScript.LifeTimeWageEarned;

                if (IsLucky)
                {
                    if(HardWorkerScript.LuckFactor > PreviousLuckFactor)
                    {
                        PreviousLuckFactor = HardWorkerScript.LuckFactor;
                        MostLuckyUnitName = HardWorkerScript.UnitName;
                        MostLuckyLifeTimeWages = HardWorkerScript.LifeTimeWageEarned;
                    }

                    if (HardWorkerScript.LuckFactor < PreviousLuckFactor2)
                    {
                        PreviousLuckFactor2 = HardWorkerScript.LuckFactor;
                        MostUnLuckyUnitName = HardWorkerScript.UnitName;
                        MostUnLuckyLifeTimeWages = HardWorkerScript.LifeTimeWageEarned;
                    }
                    TotalLuckFactor += HardWorkerScript.LuckFactor;
                }

                if(HardWorkerScript.SuccessfulPromotionNumber > PreviousTotalSuccessfulPromotion)
                {
                    PreviousTotalSuccessfulPromotion = HardWorkerScript.SuccessfulPromotionNumber;
                    MostSuccessfullyPromotedUnitName = HardWorkerScript.UnitName;
                    MostSuccessfullyPromotedUnitNumber = HardWorkerScript.SuccessfulPromotionNumber;
                }

                if(HardWorkerScript.LifeTimeHoursWorked > PreviousLifeTimeHoursWorked)
                {
                    PreviousLifeTimeHoursWorked = HardWorkerScript.LifeTimeHoursWorked;
                    MostHardWorkingName = HardWorkerScript.UnitName;
                    MostHardWorkingLifeTimeWages = HardWorkerScript.LifeTimeWageEarned;
                }

                if(HardWorkerScript.LifeTimeWageEarned > PreviousRichestUnitLifeTimeWages)
                {
                    PreviousRichestUnitLifeTimeWages = HardWorkerScript.LifeTimeWageEarned;
                    MostRichestUnitName = HardWorkerScript.UnitName;
                }

                if(HardWorkerScript.LifeTimeWageEarned < PreviousPoorestUnitLifeTimeWages)
                {
                    PreviousPoorestUnitLifeTimeWages = HardWorkerScript.LifeTimeWageEarned;
                    MostPoorestUnitName = HardWorkerScript.UnitName;
                }

                if(HardWorkerScript.LifeTimeHoursWorked < PreviousLifeTimeHoursWorked2)
                {
                    PreviousLifeTimeHoursWorked2 = HardWorkerScript.LifeTimeHoursWorked;
                    MostLazyUnitName = HardWorkerScript.UnitName;
                    MostLazyLifeTimeWages = HardWorkerScript.LifeTimeWageEarned;
                }

                if(HardWorkerScript.IsSimulationRunning == false)
                {
                    NumberOfUnitSimulationEnded++;
                }

                NumberOfUnit++;
                TotalSuccessfulPromotionNumber += HardWorkerScript.SuccessfulPromotionNumber;
                LifeTimeWagesEarnedList.Add(HardWorkerScript.LifeTimeWageEarned);
                //Debug.Log(AHardWorkerGameObject.name + " CentralInt = " + HardWorkerScript.UnitCentralString);
            }

            


        }

        // Sort the list
        LifeTimeWagesEarnedList.Sort();

        int count = LifeTimeWagesEarnedList.Count;
        if (count % 2 == 1)
        {
            // Odd count -> middle value
            MedianLifeTimeWagesEarned = LifeTimeWagesEarnedList[count / 2];
        }
        else
        {
            // Even count -> average of two middle values
            float middle1 = LifeTimeWagesEarnedList[(count / 2) - 1];
            float middle2 = LifeTimeWagesEarnedList[count / 2];
            MedianLifeTimeWagesEarned = (middle1 + middle2) / 2f;
        }

        AverageSuccessfulPromotionNumber = (TotalSuccessfulPromotionNumber / NumberOfUnit);
        AverageLuckFactor = (TotalLuckFactor / NumberOfUnit);

        if (NumberOfUnit > 0) 
        {
            AverageOfTotalHourlyWageOfAllUnits = (TotalHourlyWageOfAllUnits / NumberOfUnit);

        }
        string HeadLineContent = "HEADLINE" + "\n";
        string AverageOfTotalHourlyWageOfAllUnitsContent = "Average Lifetime Earnings: $" + AverageOfTotalHourlyWageOfAllUnits.ToString("N2") + "\n";
        string MedianLifeTimeWagesEarnedContent = "Median Lifetime Earnings: $" + MedianLifeTimeWagesEarned.ToString("N2") + "\n";
        string NumberOfUnitContent = "Number of Workers: " + NumberOfUnit.ToString() + "\n";
        string MostLuckyUnitNameContent = "Most Lucky: " + MostLuckyUnitName + " with " + PreviousLuckFactor + "X Luck" + " earned $" + MostLuckyLifeTimeWages.ToString("N2") + "\n";
        string MostUnLuckyUnitNameContent = "Most UnLucky: " + MostUnLuckyUnitName + " with " + PreviousLuckFactor2 + "X Luck" + " earned $" + MostUnLuckyLifeTimeWages.ToString("N2") + "\n";
        string MostSuccessfullyPromotedUnitNameContent = "Most Promoted: " + MostSuccessfullyPromotedUnitName + " promoted " + MostSuccessfullyPromotedUnitNumber + "\n";
        string AverageLuckFactorContent = "Average Luck Factor: " + AverageLuckFactor.ToString("N2") + "X" + " out of 7X" + "\n";
        string AverageSuccessfulPromotionNumberContent = "Average Successful Promotion: " + AverageSuccessfulPromotionNumber.ToString("F0") + " out of 20" + "\n";
        string MostHardWorkingNameContent = "Most HardWorking: " + MostHardWorkingName + " earned $" + MostHardWorkingLifeTimeWages.ToString("N2") + "\n";
        string MostLazyUnitNameContent = "Laziest: " + MostLazyUnitName + " earned $" + MostLazyLifeTimeWages.ToString("N2") + "\n";
        string MostRichestUnitNameContent = "Richest: " + MostRichestUnitName + " earned $" + PreviousRichestUnitLifeTimeWages.ToString("N2") + "\n";
        string MostPoorestUnitNameContent = "Poorest: " + MostPoorestUnitName + " earned $" + PreviousPoorestUnitLifeTimeWages.ToString("N2") + "\n\n";

        AllCentralString += HeadLineContent;
        AllCentralString += AverageOfTotalHourlyWageOfAllUnitsContent;
        AllCentralString += MedianLifeTimeWagesEarnedContent;
        AllCentralString += NumberOfUnitContent;
        if (IsLucky)
        {
            AllCentralString += MostLuckyUnitNameContent;
            AllCentralString += MostUnLuckyUnitNameContent;
            AllCentralString += AverageLuckFactorContent;
        }
        AllCentralString += MostSuccessfullyPromotedUnitNameContent;
        AllCentralString += AverageSuccessfulPromotionNumberContent;
        AllCentralString += MostHardWorkingNameContent;
        AllCentralString += MostLazyUnitNameContent;
        AllCentralString += MostRichestUnitNameContent;
        AllCentralString += MostPoorestUnitNameContent;
        AllCentralString += AllUnitString;

        
            WriteToFileAllData();
        
    }

    private void WriteToFileAllData()
    {
        File.WriteAllText(TheLogPath, AllCentralString);
    }
}
