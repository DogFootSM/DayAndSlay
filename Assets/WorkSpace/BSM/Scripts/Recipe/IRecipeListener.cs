using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecipeListener
{
    public void RecipeNotify((int, int) category);
}
