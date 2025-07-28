using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EditModeTests
{
    [Test]
    public void PlayerMaxLivesTest()
    {
        PlayerController myPlayer = new PlayerController();

        int _result = myPlayer.maxLives;

        Assert.GreaterOrEqual(3, _result);
    }

    [Test]
    public void GroundLayerTest()
    {
        PlayerController myPlayer = new PlayerController();

        LayerMask _result = myPlayer.groundLayer;

        Assert.NotNull(_result);
    }

    [Test]
    public void ShootCooldownTest()
    {
        PlayerController myProjectile = new PlayerController();

        float _result = myProjectile.shootCooldown;

        Assert.AreEqual(10, _result);
    }
}
