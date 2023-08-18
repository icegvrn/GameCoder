using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] List<GameObject> pauseElements;

    private void Start()
    {
        if (pauseElements is null) { pauseElements = new List<GameObject>(); }
       
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        EnablePauseScreen(isPaused);
    }

    void EnablePauseScreen(bool paused)
    {
        foreach (GameObject obj in pauseElements) { obj.SetActive(paused); }
    }

}