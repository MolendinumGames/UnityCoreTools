using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools;

public class GlobalPoolManager : Singleton<GlobalPoolManager>
{
    protected override bool Persistent => false;
}
