using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DonBigo
{
    public class CharacterManager : MonoBehaviour
    {
        GameObject donbigo;
        [SerializeField] private Sprite[] donbigoSprite;
        void Start()
        {
            donbigo = new GameObject("Player", typeof(SpriteRenderer), typeof(Bigodon));
            SpriteRenderer DBRenderer = donbigo.GetComponent<SpriteRenderer>();
            DBRenderer.sprite = donbigoSprite[7];
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}