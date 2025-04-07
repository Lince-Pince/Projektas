using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.TestTools.Logging;

public class GameManagerTests
{
    private GameObject gmObject;
    private GameManager gameManager;
    private GameObject gameOverUI;

    [SetUp]
    public void Setup()
    {
        // Create a dummy GameOverUI object and ensure it is inactive
        gameOverUI = new GameObject("GameOverUI");
        gameOverUI.SetActive(false);

        // Create the GameManager object and assign the gameOverUI
        gmObject = new GameObject("GameManager");
        gameManager = gmObject.AddComponent<GameManager>();
        gameManager.gameOverUI = gameOverUI;

        // Call Start() manually to initialize parameters
        gameManager.SendMessage("Start");

        // Ensure PlayerStats has the initial value
        PlayerStats.Lives = 3;
        GameManager.GameIsOver = false;
    }

    [TearDown]
    public void Teardown()
    {
        // In PlayMode tests, use Destroy
        Object.Destroy(gmObject);
        Object.Destroy(gameOverUI);
    }

    // 1. Test that Start() correctly sets GameIsOver to false.
    [UnityTest]
    public IEnumerator Test_Start_SetsGameIsOverToFalse()
    {
        yield return null; // Wait one frame
        Assert.IsFalse(GameManager.GameIsOver, "After Start(), GameIsOver should be false.");
    }

    // 2. Test that the game ends when PlayerStats.Lives becomes 0.
    [UnityTest]
    public IEnumerator Test_EndGameTriggered_WhenLivesAreZero()
    {
        PlayerStats.Lives = 0;
        yield return null; // Wait one frame for Update() to be called
        gameManager.SendMessage("Update");
        yield return null;
        Assert.IsTrue(GameManager.GameIsOver, "GameIsOver should be true when Lives are 0.");
        Assert.IsTrue(gameOverUI.activeSelf, "GameOverUI should be active when the game ends.");
    }

    // 3. Test that the game ends when PlayerStats.Lives is negative.
    [UnityTest]
    public IEnumerator Test_EndGameTriggered_WhenLivesAreNegative()
    {
        PlayerStats.Lives = -1;
        yield return null;
        gameManager.SendMessage("Update");
        yield return null;
        Assert.IsTrue(GameManager.GameIsOver, "GameIsOver should be true when Lives are negative.");
        Assert.IsTrue(gameOverUI.activeSelf, "GameOverUI should be active when Lives are negative.");
    }

    // 4. Test that manually calling EndGame() (simulating 'e' key press) ends the game.
    [UnityTest]
    public IEnumerator Test_EndGameTriggered_WhenEKeyPressed()
    {
        // Since simulating Input is not trivial in tests, call EndGame() directly.
        gameManager.SendMessage("EndGame");
        yield return null;
        Assert.IsTrue(GameIsOverHelper(), "GameIsOver should be true after EndGame() is called.");
        Assert.IsTrue(gameOverUI.activeSelf, "GameOverUI should be active after EndGame() is called.");
    }

    // Helper method to access the static GameIsOver property.
    private bool GameIsOverHelper()
    {
        return GameManager.GameIsOver;
    }

    // 5. Test that Update() does not change the state after the game is over.
    [UnityTest]
    public IEnumerator Test_Update_DoesNotChangeState_AfterGameIsOver()
    {
        gameManager.SendMessage("EndGame");
        yield return null;
        bool initialUIState = gameOverUI.activeSelf;
        gameManager.SendMessage("Update");
        yield return null;
        Assert.IsTrue(GameManager.GameIsOver, "After EndGame(), GameIsOver should remain true.");
        Assert.AreEqual(initialUIState, gameOverUI.activeSelf, "The state of GameOverUI should not change after EndGame().");
    }

    // 6. Test that GameOverUI remains active after multiple Update() calls.
    [UnityTest]
    public IEnumerator Test_GameOverUI_RemainsActive_AfterMultipleUpdates()
    {
        gameManager.SendMessage("EndGame");
        yield return null;
        for (int i = 0; i < 3; i++)
        {
            gameManager.SendMessage("Update");
            yield return null;
        }
        Assert.IsTrue(gameOverUI.activeSelf, "GameOverUI should remain active after multiple Update() calls.");
    }

    // 7. Test that the game does not end when PlayerStats.Lives is above zero.
    [UnityTest]
    public IEnumerator Test_GameDoesNotEnd_WhenLivesAreAboveZero()
    {
        PlayerStats.Lives = 5;
        yield return null;
        gameManager.SendMessage("Update");
        yield return null;
        Assert.IsFalse(GameManager.GameIsOver, "GameIsOver should not be true when Lives are above zero.");
        Assert.IsFalse(gameOverUI.activeSelf, "GameOverUI should not be active when Lives are above zero.");
    }

    // 8. Test that calling EndGame() multiple times leaves the state unchanged.
    [UnityTest]
    public IEnumerator Test_MultipleEndGameCalls()
    {
        gameManager.SendMessage("EndGame");
        yield return null;
        bool stateAfterFirstCall = GameManager.GameIsOver;
        bool uiActiveAfterFirstCall = gameOverUI.activeSelf;
        // Call EndGame() a second time
        gameManager.SendMessage("EndGame");
        yield return null;
        Assert.IsTrue(GameManager.GameIsOver, "GameIsOver should remain true after multiple EndGame() calls.");
        Assert.AreEqual(uiActiveAfterFirstCall, gameOverUI.activeSelf, "The state of GameOverUI should remain the same after multiple EndGame() calls.");
    }

    // 9. Test that after the game is over, even if PlayerStats.Lives changes, the game state remains over.
    [UnityTest]
    public IEnumerator Test_UpdateDoesNotResetGameStateAfterGameIsOver()
    {
        gameManager.SendMessage("EndGame");
        yield return null;
        // Simulate increasing Lives after game over
        PlayerStats.Lives = 5;
        gameManager.SendMessage("Update");
        yield return null;
        Assert.IsTrue(GameManager.GameIsOver, "GameIsOver should remain true even if Lives increase after the game ends.");
    }
}
