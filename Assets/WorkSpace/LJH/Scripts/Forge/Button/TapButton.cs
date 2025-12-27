using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class TapButton : MonoBehaviour
{

    [SerializeField] private List<TapButton> tapButtons;
    
    /// <summary>
    /// 해당 버튼이 웨펀인지 서브웨펀인지 알려주는 메서드
    /// </summary>
    /// <returns></returns>
    public int WhoAmI()
    {
        if (tapButtons.Count == 2)
        {
            return tapButtons.IndexOf(this);
        }
        
        return tapButtons.IndexOf(this) + 2;
        
    }


}
