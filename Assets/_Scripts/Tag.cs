using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag : MonoBehaviour {

    public enum ArrowType { Left, Right }
    [SerializeField]
    public ArrowType rootArrow;
}
