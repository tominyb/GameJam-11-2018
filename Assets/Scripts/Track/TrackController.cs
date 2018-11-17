using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TrackController : MonoBehaviour
{
    public static TrackController Instance;

    [System.Serializable]
    public class TrackSelection
    {
        public KeyCode InputKey;
        public Image SelectionImage;
        public TrackMeshGenerator MeshGenerator;
    }

    [SerializeField] private TrackDrawer m_drawer;
    [SerializeField] private List<TrackSelection> m_tracks;
    private TrackSelection m_selected;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_selected = m_tracks[0];
        UpdateTrackSelections();
    }

    private void Update()
    {
        foreach (var track in m_tracks)
        {
            if (!Input.GetKeyDown(track.InputKey) || track == m_selected)
                continue;

            m_selected = track;
            m_drawer.SetMeshGenerator(m_selected.MeshGenerator);
            UpdateTrackSelections();
        }
    }

    private void UpdateTrackSelections()
    {
        foreach (var track in m_tracks)
        {
            var grayScale = track == m_selected ? 1 : 0.5f;
            track.SelectionImage.color  = new Color(grayScale, grayScale, grayScale);
            track.MeshGenerator.enabled = track == m_selected;
        }
    }

    public void ResetAll()
    {
        foreach (var track in m_tracks)
            track.MeshGenerator.Reset();
    }
}
