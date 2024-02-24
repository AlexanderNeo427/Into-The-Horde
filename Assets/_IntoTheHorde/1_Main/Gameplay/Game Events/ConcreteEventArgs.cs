using System;
using UnityEngine;

namespace IntoTheHorde
{
    public class ActorDeathEventArgs : EventArgs
    {
        public GameActor Actor { get; private set; }

        public ActorDeathEventArgs(GameActor _Actor) { Actor = _Actor; }
    }

    public class GunFiredEventArgs : EventArgs
    {
        public GameActor Shooter         { get; private set; }
        public float     GunRange        { get; private set; }
        public float     BulletDamage    { get; private set; }
        public float     MinGunSpread    { get; private set; }
        public float     MaxGunSpread    { get; private set; }
        public float     SpreadDecayRate { get; private set; }

        public GunFiredEventArgs(GameActor shooter, 
                                 float     gunRange, 
                                 float     bulletDamage, 
                                 float     minGunSpread, 
                                 float     maxGunSpread, 
                                 float     spreadDecayRate)
        {
            Shooter         = shooter;
            GunRange        = gunRange;
            BulletDamage    = bulletDamage;
            MinGunSpread    = minGunSpread;
            MaxGunSpread    = maxGunSpread;
            SpreadDecayRate = spreadDecayRate;
        }
    }

    public class GunScopeEventArgs : EventArgs
    {
        public float  ScopedFOV   { get; private set; }
        public Sprite ScopeSprite { get; private set; }

        public GunScopeEventArgs(float scopedFOV, Sprite scopeSprite) 
        {
            ScopedFOV   = scopedFOV;
            ScopeSprite = scopeSprite;
        }
    }

    public class GunUnscopeEventArgs : EventArgs
    {
        public GunUnscopeEventArgs() {}
    }

    public class ThrowableUsedEventArgs : EventArgs
    {
        public GameActor             Thrower         { get; private set; }
        public BaseThrowableBehavior ThrowablePrefab { get; private set; }

        public ThrowableUsedEventArgs(GameActor thrower, BaseThrowableBehavior throwablePrefab)
        {
            Thrower         = thrower;
            ThrowablePrefab = throwablePrefab;
        }
    }

    public class HealthChangedEventArgs : EventArgs
    {
        public Health AffectedHealth;
        public float  Delta;

        public HealthChangedEventArgs() {}

        public void Set(Health health, float healthDelta)
        {
            AffectedHealth = health;
            Delta          = healthDelta;
        }
    }

    public class HealingBeginEventArgs : EventArgs
    {
        public GameActor Actor       { get; private set; }
        public float     HealingTime { get; private set; }

        public HealingBeginEventArgs(GameActor actor, float healingTime)
        {
            Actor       = actor;
            HealingTime = healingTime;
        }
    }

    public class HealingEndEventArgs : EventArgs
    {
        public GameActor Actor { get; private set; }

        public HealingEndEventArgs(GameActor actor) { Actor = actor; }
    }

    public class HealingSuccessEventArgs : EventArgs
    {
        public GameActor Actor { get; private set; }

        public HealingSuccessEventArgs(GameActor actor) { Actor = actor; }
    }

    public class InventoryChangedEventArgs : EventArgs
    {
        public InventoryChangedEventArgs() {}
    }

    public class DoorOpenedEventArgs : EventArgs
    {
        public DoorInteraction Door { get; private set; }

        public DoorOpenedEventArgs(DoorInteraction door) { Door = door; }
    }

    public class DoorClosedEventArgs : EventArgs
    {
        public DoorInteraction Door { get; private set; }

        public DoorClosedEventArgs(DoorInteraction door) { Door = door; }
    }

    public class RestartGameEventArgs : EventArgs
    {
        public RestartGameEventArgs() {}
    }

    public class EnterMainMenuEventArgs : EventArgs
    {
        public EnterMainMenuEventArgs() {}
    }

    public class PauseGameEventArgs : EventArgs 
    {
        public PauseGameEventArgs() {} 
    }

    public class UnpauseGameEventArgs : EventArgs
    {
        public UnpauseGameEventArgs() {}
    }
}
