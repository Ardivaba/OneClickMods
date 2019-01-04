using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class LuaMethods
{
    [LuaMethod]
    public static void print(object logObject)
    {
        Logger.Log(logObject);
    }

    [LuaMethod]
    public static Card[] GetFriendlyHand()
    {
        return GameState.Get().GetFriendlySidePlayer().GetHandZone().GetCards().ToArray();
    }

    [LuaMethod]
    public static Card[] GetEnemyHand()
    {
        return GameState.Get().GetOpposingSidePlayer().GetHandZone().GetCards().ToArray();
    }

    [LuaMethod]
    public static Card[] GetFriendlyDeck()
    {
        return GameState.Get().GetFriendlySidePlayer().GetDeckZone().GetCards().ToArray();
    }

    [LuaMethod]
    public static Card[] GetEnemyDeck()
    {
        return GameState.Get().GetOpposingSidePlayer().GetDeckZone().GetCards().ToArray();
    }

    [LuaMethod]
    public static Card[] GetFriendlyGraveyard()
    {
        return GameState.Get().GetFriendlySidePlayer().GetGraveyardZone().GetCards().ToArray();
    }

    [LuaMethod]
    public static Card[] GetEnemyGraveyard()
    {
        return GameState.Get().GetOpposingSidePlayer().GetGraveyardZone().GetCards().ToArray();
    }

    [LuaMethod]
    public static Player GetFriendlyPlayer()
    {
        return GameState.Get().GetFriendlySidePlayer();
    }

    [LuaMethod]
    public static Player GetEnemyPlayer()
    {
        return GameState.Get().GetOpposingSidePlayer();
    }

    [LuaMethod]
    public static Card[] GetFriendlyBattlefield()
    {
        return GameState.Get().GetFriendlySidePlayer().GetBattlefieldZone().GetCards().ToArray();
    }

    [LuaMethod]
    public static Card[] GetEnemyBattlefield()
    {
        return GameState.Get().GetOpposingSidePlayer().GetBattlefieldZone().GetCards().ToArray();
    }

    [LuaMethod]
    public static int GetAttackPower(Player player)
    {
        int attackPower = 0;

        var cards = GetFriendlyBattlefield();
        foreach (var card in cards)
        {
            if (card.GetEntity().CanAttack())
            {
                attackPower += card.GetEntity().GetRealTimeAttack();
            }
        }

        return attackPower;
    }

    [LuaMethod]
    public static int GetFriendlyAttackPower(Player player)
    {
        return GetAttackPower(GetFriendlyPlayer());
    }

    [LuaMethod]
    public static int GetEnemyAttackPower(Player player)
    {
        return GetAttackPower(GetEnemyPlayer());
    }

    [LuaMethod]
    public static Vector2 WorldToScreen(Vector3 position)
    {
        return Camera.main.WorldToScreenPoint(position);
    }
}
