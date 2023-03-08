using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabable
{
    /// <summary>
    /// Grabing, is called when an object with IGrabable is grabbed
    /// </summary>
    void Grab();
}
