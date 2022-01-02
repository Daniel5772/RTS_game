using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTS.Units
{
    [CreateAssetMenu(fileName = "New Unit", menuName = "New Unit/Basic")]

    public class BasicUnit : ScriptableObject
    {
        public enum unitType
        {
            Worker,
            Warrior,
            Healer
        };

        [Space(15)]
        [Header("Unit Settings")]

        public unitType type;
        public new string name;
        public GameObject playerPrefab;
        public GameObject enemyPrefab;

        [Space(15)]
        [Header("Unit Base Stats")]
        [Space(40)]

        public int cost;
        public int attack;
        public int atkRange;
        public int health;
        public int armor;
    }

}

