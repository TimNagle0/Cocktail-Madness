using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ExperimentManager : MonoBehaviour
{
    //BOF STUFF
    // Imports of JS functions defined in the .jslib in Plugins folder 
    [DllImport("__Internal")]
    public static extern void RedirectBOF();
    [DllImport("__Internal")]
    public static extern void RequestConfigFromJS();

    [SerializeField] private CustomerManager customerManager;
    [SerializeField] private Shaker shaker;
    [SerializeField] private OrderManager orderManager;
    [SerializeField] private IngredientList ingredientList;
    [SerializeField] private LevelManager levelManager;

    #region Variables
    private string startTimeExperiment = "";
    [SerializeField] string currentLevel;




    #region Time intervals
    private float timeSinceLastIngredient = 0;
    private float timeSinceLastMissclick = 0;
    private float timeSinceLastTrashcan = 0;
    private float timeSinceLastIncorrect = 0;
    private float timeSinceLastMissed = 0;
    private float timeSinceStopShaking = 0;
    private float timeSinceStartShaking = 0;
    private float timeSinceLostLife = 0;
    private float timeSincePerfect = 0;
    private float timeSinceCorrect= 0;
    #endregion

    #endregion

    #region events
    private void SubscribeToEvents()
    {
        ingredientList.useIngredient += UseIngredient;
        ingredientList.deselectIngredient += MissclickIngredient;
        shaker.trashShaker += DeleteIngredients;
        shaker.startShaking += StartShaking;
        shaker.stopShaking += StopShaking;
        orderManager.incorrectOrder += IncorrectOrder;
        customerManager.missedOrder += MissedOrder;
        orderManager.incorrectOrder += LifeLost;
        orderManager.perfectOrder += PerfectServe;
        orderManager.correctOrder += CorrectServe;
        orderManager.shakeTime += ShakeTime;
        levelManager.finishTutorial += PrepareSummaryDataPoint;
    }

    private float GetInterval(ref float timeSinceLast)
    {
        float interval = Time.timeSinceLevelLoad - timeSinceLast;
        timeSinceLast = Time.timeSinceLevelLoad;
        return interval;
    }

    #region Preparing
    private void UseIngredient(GameObject ingredient)
    {
        PrepareContinuousDataPoint("Ingredient", ingredient.name, GetInterval(ref timeSinceLastIngredient));
    }
    private void StartShaking()
    {
        timeSinceStartShaking = Time.timeSinceLevelLoad;
        PrepareContinuousDataPoint("Shaking", "Start", GetInterval(ref timeSinceStopShaking));
    }

    private void StopShaking()
    {
        timeSinceStopShaking = Time.timeSinceLevelLoad;
        PrepareContinuousDataPoint("Shaking", "Stop", GetInterval(ref timeSinceStartShaking));
    }

    #endregion

    #region accuracy
    private void MissclickIngredient(GameObject ingredient)
    {
        PrepareContinuousDataPoint("Accuracy", "Missclick", GetInterval(ref timeSinceLastMissclick));
    }

    private void DeleteIngredients()
    {
        PrepareContinuousDataPoint("Accuracy", "Trashed", GetInterval(ref timeSinceLastTrashcan));
    }

    private void IncorrectOrder()
    {
        PrepareContinuousDataPoint("Accuracy", "Incorrect", GetInterval(ref timeSinceLastIncorrect));
    }

    private void MissedOrder()
    {
        PrepareContinuousDataPoint("Accuracy", "Missed", GetInterval(ref timeSinceLastMissed));
    }
    #endregion
  
    #region Serving
    private void LifeLost()
    {
        PrepareContinuousDataPoint("Lives", "Lose Life", GetInterval(ref timeSinceLostLife));
    }

    private void PerfectServe()
    {
        PrepareContinuousDataPoint("Serving", "Perfect", GetInterval(ref timeSincePerfect));
    }
    private void CorrectServe()
    {
        PrepareContinuousDataPoint("Serving", "Correct", GetInterval(ref timeSinceCorrect));
    }
    private void ShakeTime(float time)
    {
        PrepareContinuousDataPoint("Serving", "Shaketime", time);
    }
    #endregion


    #endregion


    private void Start()
    {
        SubscribeToEvents();   
    }
    #region Data Classes
    public class ContinuousData
    {
        public string level { get; set; }
        public string timeStamp { get; set; }
        public string category { get; set; }
        public string action { get; set; }
        public float interval { get; set; }
        public ContinuousData(string currentLevel)
        {
            level = currentLevel;
            timeStamp = System.DateTime.Now.ToString();
        }
    }

    public class SummaryData
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string level { get; set; }
        public int totalTime { get; set; }
        public int totalCustomers { get; set; }
        public int perfectServes { get; set; }
        public int correctServes { get; set; }
        public int incorrectServes { get; set; }
        public int totalScore { get; set; }

        public SummaryData(string start, string currentLevel)
        {
            startTime = start;
            level = currentLevel;
            endTime = System.DateTime.Now.ToString();
        }
    }
    #endregion


    #region Datapoint methods
    public void PrepareContinuousDataPoint(string category, string action, float interval)
    {

        ContinuousData dataPoint = new ContinuousData(currentLevel);
        dataPoint.category = category;
        dataPoint.action = action;
        dataPoint.interval = interval;

        SendFilesContinuous(dataPoint);
    }

    public void PrepareSummaryDataPoint(bool isLast)
    {
        SummaryData dataPoint = new SummaryData(startTimeExperiment, currentLevel);
        dataPoint.totalTime = (int)Time.timeSinceLevelLoad;
        dataPoint.totalCustomers = PlayerStats.GetTotalServings();
        dataPoint.perfectServes = PlayerStats.perfectServings;
        dataPoint.correctServes = PlayerStats.correctServings;
        dataPoint.incorrectServes = PlayerStats.incorrectServings;
        dataPoint.totalScore = PlayerStats.GetTotalScore();

        SendFilesSummary(dataPoint, isLast);
    }

    public void SendFilesContinuous(ContinuousData dataPoint)
    {
        WWWForm form;
        form = AddFieldsContinuous(dataPoint);
        StartCoroutine(SendFiles(form, false, false));

    }

    public void SendFilesSummary(SummaryData dataPoint, bool isLast)
    {

        WWWForm form;
        form = AddFieldsSummary(dataPoint);
        StartCoroutine(SendFiles(form, true, isLast));

        if (isLast)
        {
            Invoke("StopExperiment", 0.5f);
        }
        else
        {
            Invoke("LoadNextScene", 0.5f);
        }

    }
    private WWWForm AddFieldsContinuous(ContinuousData data)
    {
        WWWForm form = new WWWForm();
        form.AddField("formType", "Continuous");
        form.AddField("level", data.level);
        form.AddField("timeStamp", data.timeStamp);
        form.AddField("category", data.category);
        form.AddField("action", data.action);
        form.AddField("interval", data.interval.ToString());
        return form;
    }

    private WWWForm AddFieldsSummary(SummaryData data)
    {
        WWWForm form = new WWWForm();
        form.AddField("formType", "Summary");
        form.AddField("startTime", data.startTime);
        form.AddField("endTime", data.endTime);
        form.AddField("level", data.level);
        form.AddField("totalTime", data.totalTime);
        form.AddField("totalCustomers", data.totalCustomers);
        form.AddField("perfectServes", data.perfectServes);
        form.AddField("correctServes", data.correctServes);
        form.AddField("incorrectServes", data.incorrectServes);
        form.AddField("totalScore", data.totalScore);

        return form;
    }

    #endregion

    public IEnumerator SendFiles(WWWForm form, bool loadNext, bool isFinalForm)
    {
        // Instead of the URL we can use # to get to the same route as the game was delivered on
        // Alternatively specify the URL of the server with port and route
        // var url = "http://127.0.0.1:5000/game"
        var url = "#";

        UnityWebRequest UWRPost = UnityWebRequest.Post(url, form);
        yield return UWRPost.SendWebRequest();

        if (UWRPost.isNetworkError || UWRPost.isHttpError)
        {
            Debug.Log(UWRPost.error);
        }
        UWRPost.Dispose();
        if (isFinalForm)
        {
            Invoke("StopExperiment", 1f);
        }
        else if (loadNext)
        {
            Invoke("LoadNextScene", 0.5f);
        }
    }

    public void StopExperiment()
    {
        RedirectBOF();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    
}
