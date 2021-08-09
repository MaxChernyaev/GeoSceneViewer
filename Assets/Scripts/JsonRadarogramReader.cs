using System;
using UnityEngine;

[Serializable]
public class JsonRadarogramReader : MonoBehaviour
{
    [Serializable]
    public class images
    {
        [SerializeField]
        public int[] jpg0 = new int[2];
        [SerializeField]
        public int[] jpg1 = new int[2];
        [SerializeField]
        public int[] jpg2 = new int[2];
        [SerializeField]
        public int[] jpg3 = new int[2];
        public int[] jpg4 = new int[2];
        public int[] jpg5 = new int[2];
        public int[] jpg6 = new int[2];
        public int[] jpg7 = new int[2];
        public int[] jpg8 = new int[2];
        public int[] jpg9 = new int[2];
        public int[] jpg10 = new int[2];
    }

    [Serializable]
    public class CommonData
    {
        [SerializeField]
        public images images;

        [SerializeField]
        public double time;
        
        [SerializeField]
        public string id;
    }

}
