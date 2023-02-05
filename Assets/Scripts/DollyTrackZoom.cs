using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class TrackedDollyWithZoom : CinemachineTrackedDolly
{
    /// <summary>How much Z position should affect the ortho size. If it's one, Z position of 1 will make 1 ortho size</summary>
    [Tooltip("How much Z position should affect the ortho size. If it's one, Z position of 1 will make 1 ortho size")]
    public float m_ZPosToOrthoSizeMult = -1f;
    public bool m_EnableZPosOrtho = false;
    
    public override void MutateCameraState(ref CameraState curState, float deltaTime)
    {
        base.MutateCameraState(ref curState, deltaTime);
        if (m_EnableZPosOrtho)
            curState.Lens.OrthographicSize = curState.RawPosition.z * m_ZPosToOrthoSizeMult;
    }
}
