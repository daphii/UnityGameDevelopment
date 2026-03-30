using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class BallSO : ScriptableObject
{
    readonly public static string prefix = "BallSO_";
    public string Name
    {
        get
        {
            string strippedName = name.Replace(prefix, "");
            return strippedName;
        }
    }

    // Runs when the script is loaded or a value is changed in the inspector
    private void OnValidate()
    {
        if (physicsMaterial != null)
        {
            physicsMaterial.bounciness = ActualBounciness;
        }
    }

    [Title("Ball")]
    [InfoBox("@\"Ball Name: \" + Name")]
    [SerializeField, Required]
    public GameObject Prefab;

    [Title("Stats")]
    [InfoBox("@CheckStatTotals()")]
    [SerializeField, Range(0, 5), Tooltip("The maximum power the ball can be shot at.")]
    int maxPowerRating = 1;
    [SerializeField, Range(0, 5), Tooltip("The ammount a player can influence the ball after the shot")]
    int influenceRating = 1;
    [SerializeField, Range(0, 5), Tooltip("The bounciness rating of the ball")]
    int bouncinessRating = 1;
    int MaxStats = 7;
    string CheckStatTotals()
    {
        string output = "";
        float totalStats = maxPowerRating + influenceRating + bouncinessRating;

        if (totalStats > MaxStats)
        {
            output = $"<color=red>Warning! Total stats exceed the maximum allowed ({MaxStats}).</color>";
        }
        else if (totalStats < MaxStats)
        {
            output = $"<color=yellow>Not all stats allocated: {totalStats}/{MaxStats}.</color>";
        }
        else
        {
            output = $"<color=green>All stats allocated correctly: {totalStats}/{MaxStats}.</color>";
        }

        output += $"\n Power: <b>{maxPowerRating}</b> ({ActualMaxPower})";
        output += $"\n Influence: <b>{influenceRating}</b> ({ActualInfluence})";
        output += $"\n Bounciness: <b>{bouncinessRating}</b> ({ActualBounciness})";

        return output;
    }

    [Title("Materials")]
    [SerializeField, InlineEditor, Required]
    PhysicsMaterial physicsMaterial;
    [SerializeField, Space, InlineEditor, Required]
    Material material;


    public PhysicsMaterial PhysicsMaterial => physicsMaterial;
    public Material Material => material;


    const int powerRatingConversion = 10;
    const int powerBaseValue = 25;

    const float influenceRatingConversion = 0.4f;
    const float influenceBaseValue = 0.25f;

    const float bouncinessRatingConversion = 0.15f;
    const float bouncinessBaseValue = 0.1f;

    /// <summary>
    /// Gets the actual maximum power value after applying conversion and base adjustments.
    /// </summary>
    public int ActualMaxPower => (maxPowerRating * powerRatingConversion) + powerBaseValue;

    /// <summary>
    /// Gets the actual influence value after applying conversion and base adjustments.
    /// </summary>
    public float ActualInfluence => (influenceRating * influenceRatingConversion) + influenceBaseValue;

    /// <summary>
    /// Gets the actual bounciness value after applying conversion and base adjustments.
    /// </summary>
    public float ActualBounciness => (bouncinessRating * bouncinessRatingConversion) + bouncinessBaseValue;
}

#if UNITY_EDITOR
public class BallSOCustomEditor
{
    [MenuItem("Assets/Create/Custom Objects/Ball")]
    public static void CreateBallSO()
    {
        string path = "Assets";
        if (Selection.activeObject != null)
        {
            path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (System.IO.Path.GetExtension(path) != "")
            {
                path = System.IO.Path.GetDirectoryName(path);
            }
        }

        string prefix = BallSO.prefix;
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{path}/{prefix}New Ball.asset");

        BallSO asset = ScriptableObject.CreateInstance<BallSO>();

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
#endif


/* Ball Stat Distribution Concept
 * Basic Idea: Each ball has 4 points to add to the base stats. these stats are ratings, and have a conversion value to the actual stat.
 * 
 * Example: Base Stats
 * Max Power: 1
 * Influence: 1
 * Bounciness: 1
 * 
 * Example 1:
 * Max Power: 3
 * Influence: 2
 * Bounciness: 2
 * 
 * Example 2:
 * Max Power: 1
 * Influence: 4
 * Bounciness: 2
*/