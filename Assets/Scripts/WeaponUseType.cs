using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace DonBigo
{
    [Flags]
    public enum WeaponUseType : uint
    {
        Offensive = 1 << 0,
        Defensive = 1 << 1,
        Ranged    = 1 << 2,
        Melee     = 1 << 3,
        Lethal    = 1 << 4,
        Trap      = 1 << 5
    }

    public static class WeaponUseTypeUtility
    {
        public static WeaponUseType[] AllTypes { get; } = (WeaponUseType[])Enum.GetValues(typeof(WeaponUseType));
        
        public static int Score(this WeaponUseType type, Func<WeaponUseType, int> scoreFunc)
        {
            return AllTypes.Where(t => type.HasFlag(t)).Sum(scoreFunc);
        }

        

        public static Tile GetAttackTile(this Item weapon, Entity attacker, ITileGiver target)
        {
            Tile VerifyTile(Tile tile)
            {
                return weapon.CanBeUsed(attacker, tile) ? tile : null;
            }
            var type = weapon.Type.WeaponType;

            //Para ranged ou melee, só vai direto no alvo.
            if (type.HasFlag(WeaponUseType.Ranged) || type.HasFlag(WeaponUseType.Melee))
            {
                return VerifyTile(target.Tile);
            }

            //Para trap, arma entre si e o alvo. Se não der, arma de baixo de si (pra depois poder evitar).
            if (type.HasFlag(WeaponUseType.Trap))
            {
                GameGrid grid = attacker.Tile.ParentGrid;
                Vector2Int dir = (target.Tile.Pos - attacker.Tile.Pos).Sign();
                Vector2Int bestPos = attacker.Tile.Pos + dir;
                if (bestPos == target.Tile.Pos) return attacker.Tile; // Colocar trap diretamente abaixo do alvo nao adianta
                
                Tile bestTile = grid.InBounds(bestPos) ? grid[bestPos] : null;
                return VerifyTile(bestTile) ?? VerifyTile(attacker.Tile); 
            }

            return null;
        }
    }
}