using Random = UnityEngine.Random;

public static class RandomUtility
{
    /// <param name="chance">Chance entre 0 e 1.</param>
    public static bool Chance(float chance) => Random.value < chance;

    /// <summary> Retorna um sinal -1 ou 1. </summary>
    public static int Sign() => Chance(.5f) ? 1 : -1;
}