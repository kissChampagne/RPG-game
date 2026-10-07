public class MonsterConfig
{
    public static MonsterData Get(int id)
    {
        switch (id)
        {
            case 1:
                return new MonsterData { name = "哥布林", health = 80 };
            case 2:
                return new MonsterData { name = "精英怪", health = 150 };
            case 3:
                return new MonsterData { name = "Boss", health = 300 };
            default:
                return new MonsterData { name = "怪物", health = 200 };
        }
    }
}
public class MonsterData
{
    public string name;
    public int health;
}