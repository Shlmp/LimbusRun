using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayModeTests
{
    [UnityTest]
    public IEnumerator idleSpriteTest()
    {
        SceneManager.LoadScene("DKScene");
        yield return new WaitForSeconds(1f);

        PlayerController idleSprite = GameObject.FindAnyObjectByType<PlayerController>();

        GameObject _result = idleSprite.idleSprite;

        yield return new WaitForSeconds(1f);

        Assert.IsNotNull(_result);
    }

    [UnityTest]
    public IEnumerator ObstacleSpawnTest()
    {
        SceneManager.LoadScene("DKScene");
        yield return new WaitForSeconds(1f);

        ObstacleSpawner spawnPoint = GameObject.FindAnyObjectByType<ObstacleSpawner>();

        Transform _result = spawnPoint.spawnPoint;

        yield return new WaitForSeconds(1f);

        Assert.IsNotNull(_result);
    }
}
