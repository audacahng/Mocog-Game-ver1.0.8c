using UnityEngine;
using System.Collections;

/// <summary>
/// 省得每次都需要GetComponent并且缓存transition manager来使用transition功能了！
/// 而且可以直接使用ITransition接口！何其方便！
/// 直接继承我，一切尽在掌握~
/// </summary>
[RequireComponent(typeof(TransitionManager))]
public class TransitionableMonoBehavior : MonoBehaviour, ITransition
{
    #region Fields
    private TransitionManager _transitionManager;
    #endregion

    #region Properties
    /// <summary>
    /// 获得TransitionManager Component
    /// </summary>
    public TransitionManager transitionManager
    {
        get {
            if (_transitionManager == null)
            {
                _transitionManager = GetComponent<TransitionManager>();
                if (_transitionManager == null) Debug.LogError("No TransitionManager in " + name);
            }
            return _transitionManager; 
        }
    }
    #endregion

    #region Functions

    #endregion

    #region ITransition 成员

    public void TransitionIn()
    {
        transitionManager.TransitionIn();
    }

    public void TransitionIn(bool instant)
    {
        transitionManager.TransitionIn(instant);
    }

    public void TransitionOut()
    {
		transitionManager.TransitionOut();
    }

    public void TransitionOut(bool instant)
    {
        transitionManager.TransitionOut(instant);
    }

    #endregion
}
