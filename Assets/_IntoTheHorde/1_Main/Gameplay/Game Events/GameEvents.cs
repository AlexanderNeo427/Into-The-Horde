namespace IntoTheHorde
{
    public enum GameEvent
    {
        OnActorDeath,

        OnGunFired, OnGunScope, OnGunUnscope,

        OnUseThrowable,

        OnHealthChanged,
        OnHealingBegin, OnHealingEnd, OnHealingSuccess,

        OnInventoryChanged,

        OnDoorOpened, OnDoorClosed,

        OnRestartGame,
        OnEnterMainMenu,

        OnPauseGame, OnResumeGame,
        OnGameBegin, OnGameEnd,
    }
}
