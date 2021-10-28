using UnityEngine;

public static class GameManager
{
    public delegate void PauseGame(bool pause);

    public static event PauseGame GamePaused;

    static bool isGamePaused;

    public static bool IsGamePaused { get => isGamePaused; }

    public static void Pause(bool pause)
    {
        if (pause)
        {
            Cursor.visible = true;
            GamePaused?.Invoke(true);
            Time.timeScale = 0f;
            isGamePaused = true;
            return;
        }
        Cursor.visible = false;
        isGamePaused = false;
        GamePaused?.Invoke(false);
        Time.timeScale = 1f;
    }
}