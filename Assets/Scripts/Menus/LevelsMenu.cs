using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsMenu : MonoBehaviour
{
    public static int SelectedStage;
    public static int SelectedLevel = 1;

    public void Start()
    {
        SelectStage(1);
    }
    
    public void SelectStage(int stage)
    {
        SelectedStage = stage;
        
        // Set the background colour to match the stage theme
        var bgElement = GameObject.Find("Background");
        var bgImage = bgElement.GetComponent<Image>();
        var bgColour = ResolveStageColour(stage);
        bgImage.color = bgColour;
    }

    public void SelectLevel(int level)
    {
        SelectedLevel = level;
        
        var sceneName = ResolveStageName(SelectedStage);
        SceneManager.LoadScene(sceneName);
    }
    
    private string ResolveStageName(int stageIndex)
    {
        return stageIndex switch {
            2 => "Desert",
            3 => "Ice",
            4 => "Volcano",
            _ => "Plains",
        };
    }

    private Color ResolveStageColour(int stageIndex)
    {
        return stageIndex switch
        {
            2 => new Color(255f/255f, 245f/255f, 130f/255f),
            3 => new Color(176f/255f, 255f/255f, 241f/255f),
            4 => new Color(138f/255f, 30f/255f, 0f/255f),
            _ => new Color(50f/255f, 168f/255f, 82f/255f),
        };
    }
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Start");
    }
}
