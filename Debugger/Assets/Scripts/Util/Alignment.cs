using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * There are three alignments, FRIEND,
 * FOE, and NEUTRAL. The desired behavior
 * for most weapons is only to hurt things
 * that are not of the same alignment or
 * are of alignment NEUTRAL.
 */ 
public enum Alignment
{
    FRIEND,
    FOE,
    NEUTRAL
}