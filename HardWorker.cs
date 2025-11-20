using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using System.IO;

public class HardWorker : MonoBehaviour
{
    public float DaveCurrentHourlyWage;
    public float DaveCurrentMonthlyWage;
    public float DaveCurrentYearlyWage;
    public float MinStartingHourlyWage = 7.25f;
    public float MaxStartingHourlyWage = 32f;
    public int MinWorkingHoursADay = 8;
    public int MaxWorkingHoursADay = 12;
    public int TheWorkingHoursADay;
    public int TheWorkingDaysAWeek = 7;
    private float StartingHourlyWage;
    private float StartingMonthlyWage;
    private float StartingYearlyWage;
    public int Month = 1;
    public float LifeTimeWageEarned;
    public int CurrentYear = 1;

    public float CountDown;
    public float StartCountDownNumber;
    public bool IsWorking = false;
    public bool IsSimulationRunning = true;
   /*  public float MinWageIncrementPercentage = 3;
    public float MaxWageIncrementPercentage = 4;
    public float TheWageIncrementPercentage;
    public float TheDaveCurrentMonthlyWageIncrement;
   */
    private int PromotionYearCounter = 0;
    public float MinPromotionChancePercentage = 5;
    public float MaxPromotionChancePercentage = 15;
    public float ThePromotionChancePercentage;
    private int PromotionNumberProbablity;
    public int AssessedPromotionNumber;
    public int SuccessfulPromotionNumber;
    public float MinPromotionWageIncrementPercentage = 10;
    public float MaxPromotionWageIncrementPercentage = 30;
    public float ThePromotionWageIncrementPercentage;
    public float TheDaveCurrentHourlyPromotionWageIncrement;
    private string TheLogPath;
    private bool JustAssessedPromotion = false;
    private float PreviousDaveCurrentHourlyWage;
    public float LuckFactor;
    public bool IsLucky = false;
    public float LifeTimeHoursWorked;
    private float PercentageOfLifeTimeHoursWorked;
    private float MaxLifeTimeWorkingHours = 147840; //Waking Hours. Xhrs x 7days x 4weeks x 12months x 40years
    public UnitSpawner TheUnitSpawner;
    public string UnitDatasString;
    public string UnitStartingDatasString;
    public string UnitCentralString;
    private string NewUnitName;
    public string UnitSpawnerName;
    private bool JustGotSuccessfullyPromoted = false;
    public string UnitName;
    private float AverageLifeTimeHourlyWage;
    public string StaticStartingDatasString;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AccessTextFile();

        StartingWage();
        Work();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (IsSimulationRunning)
        {
            if (CountDown > 0 && IsWorking)
            {
                CountDown -= Time.deltaTime * 1f;
            }
            else
            {
                EndWork();
            }

            if (Month > 12)
            {
                // Year End
                PromotionYearCounter += 1;

                if (PromotionYearCounter == 2)
                {
                    AssessPromotion();
                    PromotionYearCounter = 0;
                }

                StoreYearData();

                Month = 1;
                CurrentYear += 1;
                
                //WageIncrement();

            }

            if (CurrentYear == 41)
            {
                EndSimulation();
            }



            DaveCurrentMonthlyWage = Mathf.Round(((DaveCurrentHourlyWage * TheWorkingHoursADay) * TheWorkingDaysAWeek) * 4 * 100f) / 100f;
            DaveCurrentYearlyWage = (DaveCurrentMonthlyWage * 12);
            AverageLifeTimeHourlyWage = (LifeTimeWageEarned / LifeTimeHoursWorked);

        }
        
        
        
    }


    private void StartingWage () 
    { 
        DaveCurrentHourlyWage = Mathf.Round(Random.Range(MinStartingHourlyWage, MaxStartingHourlyWage) * 100f) / 100f;
        PreviousDaveCurrentHourlyWage = DaveCurrentHourlyWage;

        TheWorkingHoursADay = Random.Range(MinWorkingHoursADay, (MaxWorkingHoursADay + 1));

        DaveCurrentMonthlyWage = (((DaveCurrentHourlyWage * TheWorkingHoursADay) * TheWorkingDaysAWeek) * 4);

        if (IsLucky)
        {
            LuckFactor = Mathf.Round(Random.Range(2f, 7f) * 100f) / 100f;
        }

        StartingHourlyWage = DaveCurrentHourlyWage;
        StartingMonthlyWage = DaveCurrentMonthlyWage;
        StartingYearlyWage = (DaveCurrentMonthlyWage * 12);

        StoreStartingWages();
    }

    private void Work() 
    {
        // Play work Animation
        CountDown = StartCountDownNumber;
        IsWorking = true;
    }

    private void EndWork()
    {
        IsWorking = false;
        LifeTimeWageEarned += DaveCurrentMonthlyWage;
        Month += 1;

        Work();

    }


    private void EndSimulation() 
    {
        IsSimulationRunning = false;
        TheUnitSpawner.SimulationJustEnded = true;
    }

   /* private void WageIncrement()
    {
        TheWageIncrementPercentage = Random.Range(MinWageIncrementPercentage, MaxWageIncrementPercentage);

        TheDaveCurrentMonthlyWageIncrement = ((TheWageIncrementPercentage / 100) * DaveCurrentMonthlyWage);

        DaveCurrentMonthlyWage += Mathf.RoundToInt(TheDaveCurrentMonthlyWageIncrement);
    }
   */

    private void AssessPromotion()
    {
        ThePromotionChancePercentage = Mathf.Round(Random.Range(MinPromotionChancePercentage, MaxPromotionChancePercentage) * 100f) / 100f;

        if (IsLucky)
        {
            ThePromotionChancePercentage *= LuckFactor;
            ThePromotionChancePercentage = Mathf.Round(ThePromotionChancePercentage * 100f) / 100f;
        }

        PromotionNumberProbablity = (Random.Range(1, 100));

        if(PromotionNumberProbablity <= ThePromotionChancePercentage)
        {
            // Is Within Range
            Promote();
        }
        AssessedPromotionNumber += 1;
        JustAssessedPromotion = true;
    }

    private void Promote()
    {
        PreviousDaveCurrentHourlyWage = DaveCurrentHourlyWage;

        ThePromotionWageIncrementPercentage = Mathf.Round(Random.Range(MinPromotionWageIncrementPercentage, MaxPromotionWageIncrementPercentage) * 100f) / 100f;


        TheDaveCurrentHourlyPromotionWageIncrement = Mathf.Round(((ThePromotionWageIncrementPercentage / 100f) * DaveCurrentHourlyWage) * 100f) / 100f;


        DaveCurrentHourlyWage += TheDaveCurrentHourlyPromotionWageIncrement;

        SuccessfulPromotionNumber += 1;

        StorePromotionData();
        JustGotSuccessfullyPromoted = true;
    }

    
    private void AccessTextFile()
    {
        TheUnitSpawner = GameObject.Find(UnitSpawnerName).GetComponent<UnitSpawner>();
        TheUnitSpawner.CentralNamingInt += 1;
        TheLogPath = TheUnitSpawner.TheLogPath;

        

        if (IsLucky)
        {
            UnitName = "Lucky " + TheUnitSpawner.CentralNamingInt;
            NewUnitName = UnitName + " __________________________________________________________________________________________________" + "\n\n";
        }
        else
        {
            UnitName = "Hardworker " + TheUnitSpawner.CentralNamingInt;
            NewUnitName = UnitName + " __________________________________________________________________________________________________" + "\n\n";
        }

        

        //Add text to it
        StaticStartingDatasString += NewUnitName;
    }

    

    private void StoreYearData()
    {
        string CurrentYearContent = "Year: " + CurrentYear.ToString() + "\n";
        string DaveCurrentHourlyWageContent = "Hourly Wage: $" + DaveCurrentHourlyWage.ToString("N2") + "\n";
        string DaveCurrentMonthlyWageContent = "Monthly Wage: $" + DaveCurrentMonthlyWage.ToString("N2") + "\n";
        string DaveCurrentYearlyWageContent = "Yearly Wage: $" + DaveCurrentYearlyWage.ToString("N2") + "\n";
        string AssessedPromotionNumberContent = "Assessed Promotions: " + AssessedPromotionNumber.ToString() + "\n";
        string SuccessfulPromotionNumberContent = "Successful Promotions: " + SuccessfulPromotionNumber.ToString() + "\n";
        string PromotionNumberProbablityContent = "Promotion Number Probablity: " + PromotionNumberProbablity.ToString() + "\n";
        string ThePromotionChancePercentageContent = "Promotion Chance Range: " + "0 to "+ ThePromotionChancePercentage.ToString() + "\n";
        string LifeTimeEarnedContent = "LifeTime Wages Earned: $" + LifeTimeWageEarned.ToString("N2") + "\n";


         UnitDatasString += CurrentYearContent;
        
        if(DaveCurrentHourlyWage > PreviousDaveCurrentHourlyWage)
        {
            string PreviousDaveCurrentHourlyWageContent = "Hourly Wage: " + PreviousDaveCurrentHourlyWage.ToString("N2") + "\n";
            UnitDatasString += PreviousDaveCurrentHourlyWageContent;

            PreviousDaveCurrentHourlyWage = DaveCurrentHourlyWage;
        }
        else 
        {
            UnitDatasString += DaveCurrentHourlyWageContent;
        }
        UnitDatasString += DaveCurrentMonthlyWageContent;
        UnitDatasString += DaveCurrentYearlyWageContent;
        UnitDatasString += AssessedPromotionNumberContent;
        UnitDatasString += SuccessfulPromotionNumberContent;

        if (JustAssessedPromotion)
        {
            UnitDatasString += PromotionNumberProbablityContent;
            UnitDatasString += ThePromotionChancePercentageContent;

            JustAssessedPromotion = false;
        }
        
        UnitDatasString += LifeTimeEarnedContent;


        UnitDatasString += "\n";

        if (JustGotSuccessfullyPromoted) 
        {
            WritePromotionData();
            JustGotSuccessfullyPromoted = false;
        }

        string AverageLifeTimeHourlyWageContent = "Average LifeTime Hourly Wage: $" + AverageLifeTimeHourlyWage.ToString("N2") + "\n\n";
        string StartingToCurrentDaveCurrentHourlyWageContent = "LifeTime Hourly Wages Increased From: $" + StartingHourlyWage.ToString("N2") + " to $" + DaveCurrentHourlyWage.ToString("N2") + "\n";
        string DetailsContent = "\n\n" + "DETAILS" + "\n";

        UnitStartingDatasString = StaticStartingDatasString;
        UnitStartingDatasString += SuccessfulPromotionNumberContent;
        UnitStartingDatasString += StartingToCurrentDaveCurrentHourlyWageContent;
        UnitStartingDatasString += LifeTimeEarnedContent;
        UnitStartingDatasString += AverageLifeTimeHourlyWageContent;
        UnitStartingDatasString += DetailsContent;

        UnitCentralString = (UnitStartingDatasString + UnitDatasString);

        TheUnitSpawner.YearJustEnded = true;

    }



    private void StoreStartingWages()
    {
        int WorkingHoursAWeek = (TheWorkingHoursADay * TheWorkingDaysAWeek);
        int WorkingHoursAMonth = (WorkingHoursAWeek * 4);
        int WorkingHoursAYear = (WorkingHoursAMonth * 12);
        LifeTimeHoursWorked = (WorkingHoursAYear * 40);
        PercentageOfLifeTimeHoursWorked = Mathf.Round(((LifeTimeHoursWorked / 215040) * 100f) * 100f) / 100f;

        string StartingDataContent = "STARTING DATA" + "\n";
        string WorkingHoursADayContent = "Working Hours A Day: " + TheWorkingHoursADay.ToString() + "hrs" + "\n";
        string WorkingHoursAWeekContent = "Working Hours A Week: " + WorkingHoursAWeek.ToString() + "hrs" + "\n";
        string WorkingHoursAMonthContent = "Working Hours A Month: " + WorkingHoursAMonth.ToString() + "hrs" + "\n";
        string WorkingHoursAYearContent = "Working Hours A Year: " + WorkingHoursAYear.ToString() + "hrs" + "\n";
        string WorkingDaysAWeekyContent = "Working Days A Week: " + TheWorkingDaysAWeek.ToString() + "days" + "\n";
        string StartingHourlyWageContent = "Starting Hourly Wage: $" + StartingHourlyWage.ToString("N2") + "\n";
        string StartingMonthlyWageContent = "Starting Monthly Wage: $" + StartingMonthlyWage.ToString("N2") + "\n";
        string StartingYearlyWageContent = "Starting Yearly Wage: $" + StartingYearlyWage.ToString("N2") + "\n";
        string LifeTimeHoursWorkedContent = "Life Time Hours Worked: " + LifeTimeHoursWorked.ToString() + " out of " + MaxLifeTimeWorkingHours + "\n";
        string PercentageOfLifeTimeHoursWorkedContent = "Percentage Of Waking Hours Worked: " + PercentageOfLifeTimeHoursWorked.ToString() + "%" + "\n";

        string LifeTimeEarnedContent = "LifeTime Wages Earned: $" + LifeTimeWageEarned.ToString("N2") + "\n";
        string AverageLifeTimeHourlyWageContent = "Average LifeTime Hourly Wage: $" + AverageLifeTimeHourlyWage.ToString("N2") + "\n";



        StaticStartingDatasString += StartingDataContent;
        StaticStartingDatasString += WorkingHoursADayContent;
        StaticStartingDatasString += WorkingHoursAWeekContent;
        StaticStartingDatasString += WorkingHoursAMonthContent;
        StaticStartingDatasString += WorkingHoursAYearContent;
        StaticStartingDatasString += WorkingDaysAWeekyContent;
        StaticStartingDatasString += StartingHourlyWageContent;
        StaticStartingDatasString += StartingMonthlyWageContent;
        StaticStartingDatasString += StartingYearlyWageContent;
        StaticStartingDatasString += LifeTimeHoursWorkedContent;
        StaticStartingDatasString += PercentageOfLifeTimeHoursWorkedContent;

        

        if (IsLucky)
        {
            string LuckFactorContent = "Luck Factor: " + LuckFactor.ToString() + "X" + "\n";
            StaticStartingDatasString += LuckFactorContent;
        }

        //StaticStartingDatasString += "\n\n";

        UnitStartingDatasString = StaticStartingDatasString;

        UnitCentralString = (UnitStartingDatasString + UnitDatasString);

    }

    private string SuccessfulPromotionHeadLineContent;
    private string CurrentPromotionYearContent;
    private string PromotionWageIncrementPercentageContent;
    private string PreviousToCurrentDaveCurrentHourlyWageContent;

    private void StorePromotionData() 
    {
        SuccessfulPromotionHeadLineContent = "\n" + "PROMOTION " + SuccessfulPromotionNumber.ToString() + "\n";
        CurrentPromotionYearContent = "Promoted in end of Year: " + CurrentYear.ToString() + "\n";
        PromotionWageIncrementPercentageContent = "Promotion Wage Increment Percentage: " + ThePromotionWageIncrementPercentage.ToString() + "%" + "\n";
        PreviousToCurrentDaveCurrentHourlyWageContent = "Hourly Wage Increase: $" + PreviousDaveCurrentHourlyWage.ToString("N2") + " to $" + DaveCurrentHourlyWage.ToString("N2") + "\n";
        
    }


    private void WritePromotionData()
    {
        
        UnitDatasString += SuccessfulPromotionHeadLineContent;
        UnitDatasString += CurrentPromotionYearContent;
        UnitDatasString += PromotionWageIncrementPercentageContent;
        UnitDatasString += PreviousToCurrentDaveCurrentHourlyWageContent;

        UnitDatasString += "\n\n";
    }



    // NOT IN USE
    /*
    private void CreateTextDepreciated()
    {
        TheUnitSpawner = GameObject.FindFirstObjectByType<UnitSpawner>().GetComponent<UnitSpawner>();
        TheUnitSpawner.CentralNamingInt += 1;
        // Create a timestamp for the file name
        string PathTimestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");



        //Create File if doesnt exist Writeall rewrites everything
        if (IsLucky)
        {
            // Path of the File inside "HardWorkTextFolder" under Assets
            string fileName = "Lucky_" + PathTimestamp + ".txt";
            TheLogPath = Path.Combine(Application.dataPath, "HardWorkTextFolder", fileName);

            File.WriteAllText(TheLogPath, "Lucky Log \n");
        }
        else
        {
            // Path of the File inside "HardWorkTextFolder" under Assets
            string fileName = "Dave_" + PathTimestamp + ".txt";
            TheLogPath = Path.Combine(Application.dataPath, "HardWorkTextFolder", fileName);

            File.WriteAllText(TheLogPath, "HardWork Log \n");
        }


        //New Content of the file (Just A Variable)
        string NewContent = "Login date:" + System.DateTime.Now + "\n\n";

        //Add text to it
        UnitCentralString += NewContent;
        File.AppendAllText(TheLogPath, "LifeTimeHoursWorkedContent");
    }
    */

}
