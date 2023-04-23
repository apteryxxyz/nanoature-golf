using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public GameObject[] levels;

    private void Start()
    {
        // Ensure that every level is disabled
        foreach (var level in levels)
            level.SetActive(false);
        
        // Start the level that was selected in the levels menu
        StartLevel(LevelsMenu.SelectedLevel);
    }

    // Starts the specified level
    private void StartLevel(int levelNumber)
    {
        var levelIndex = levelNumber - 1;
        if (levelIndex < 0 || levelIndex >= levels.Length) levelIndex = 0;
        LevelsMenu.SelectedLevel = levelIndex + 1;
        levels[levelIndex].SetActive(true);
    }

    private void EndLevel()
    {
        var currentLevel = GetActiveLevel();
        if (currentLevel) currentLevel.SetActive(false);
    }
    
    // Restarts the current level
    public void RestartLevel()
    {
        EndLevel();
        StartLevel(LevelsMenu.SelectedLevel);
    }

    // Loads the next level, or the next stage if there are no more levels
    public void NextLevel()
    {
        var levelNumber = LevelsMenu.SelectedLevel + 1;
        var levelIndex = levelNumber - 1;

        // If the next level is out of bounds, go to the next stage
        if (levelIndex >= levels.Length)
        {
            LevelsMenu.SelectedLevel = 1;
            LevelsMenu.SelectedStage++;
            SceneManager.LoadScene("Levels");
        }
        else
        {
            EndLevel();
            StartLevel(levelNumber);
        }
        
    }
    
    // Returns the active level, or null if no level is active
    private GameObject GetActiveLevel()
    {
        foreach (var level in levels)
            if (level.activeSelf) return level;
        return null;
    }
    
    // Returns to the levels menu
    public void GoToLevelsMenu()
    {
        foreach (var level in levels)
            level.SetActive(false);
        SceneManager.LoadScene("Levels");
    }
    
    public void TogglePause()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }
}
