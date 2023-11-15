namespace InterOrbital.Utils
{
    public enum ItemType
    {
        Consumable, Build, Bullet, Upgrade, None
    }

    public enum ConsumableType
    {
        Elytrum, Health, Recollector
    }

    public enum UpgradeType
    {
        Elytrum, Health
    }

    public enum FillMode
    {
        None,
        All,
        Random
    }

    public enum SizeChest
    {
        Small,
        Big
    }

    public enum Orientation
    {
        N,
        E,
        W,
        S,
        NW,
        NE,
        SW,
        SE
    }

    public enum TypeTableCraft
    {
        Craft,
        TemporalFundition,
        Fundition, 
        Bullet
    }

    public enum TypeMission
    {
        Recolection,
        Hunt,
        Craft
    }

    public enum DifficultyArea
    {
        Easy,
        Medium,
        Hard
    }
}