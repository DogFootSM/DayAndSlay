using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    public bool Save(SqlManager sqlManager);
}
