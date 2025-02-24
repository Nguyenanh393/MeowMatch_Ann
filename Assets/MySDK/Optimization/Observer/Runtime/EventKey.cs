namespace Observer.Runtime
{
    public enum EventKey
    {
        NONE,
        ON_START_GAME,
        ON_END_GAME,
        ON_WIN_GAME,
        
        ON_ITEM_COLLECTED,
        ON_ITEM_REQUIRE_CHANGED,

        // LifeSystem
        ON_LIFE_SYSTEM_CHANGED_STATE,
        ON_LIFE_SYSTEM_UPDATED,
    }
}
