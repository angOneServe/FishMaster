using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using XLua;
public static class XLuaConfig {

    [Hotfix]
    public static List<Type> by_property
    {
        get
        {
            return (from type in Assembly.Load("Assembly-CSharp").GetTypes()
                    where type.Namespace == "Model" || type.Namespace == "Manager" || type.Namespace == "GameAttribute" || type.Namespace == "GameEffect"
                    select type).ToList();
        }
    }
}
