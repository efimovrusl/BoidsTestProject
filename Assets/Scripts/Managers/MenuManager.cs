using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Managers
{
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private VisualTreeAsset rowTemplate;

    private VisualElement _rootVisualElement;
    private TextField _frontFlockInput;
    private TextField _backFlockInput;
    private Button _runSimulationButton;
    private ScrollView _table;

    private void Start()
    {
        _rootVisualElement = uiDocument.rootVisualElement;

        _frontFlockInput = _rootVisualElement.Q<TextField>( "FrontFlockInput" );
        _backFlockInput = _rootVisualElement.Q<TextField>( "BackFlockInput" );
        _runSimulationButton = _rootVisualElement.Q<Button>( "RunSimulationButton" );
        _table = _rootVisualElement.Q<ScrollView>();

        _runSimulationButton.clicked += () =>
        {
            if ( TryGetInput( out var frontFlockSize, out var backFlockSize ) )
            {
                var stats = new SaveManager.SimulationStats()
                {
                    FrontFlockSize = frontFlockSize,
                    BackFlockSize = backFlockSize,
                };
                HideMenu();
                gameManager.RunSimulation( frontFlockSize, backFlockSize, 10, frameRate =>
                {
                    ShowMenu();
                    stats.FrameRate = frameRate;
                    SaveManager.AddSimulationStats( stats );
                    UpdateStatsTable();
                } );
            }
            // else TODO: run some invalid-input-animation on button
        };

        UpdateStatsTable();
    }

    private bool TryGetInput( out int frontFlockSize, out int backFlockSize )
    {
        if ( int.TryParse( $"{_frontFlockInput.value}", out frontFlockSize ) &&
             int.TryParse( $"{_backFlockInput.value}", out backFlockSize ) ) return true;
        frontFlockSize = backFlockSize = 0;
        return false;
    }

    private void UpdateStatsTable()
    {
        _table.contentContainer.Clear();
        var statsList = SaveManager.GetAllSimulationStats();
        statsList.Sort( SaveManager.SimulationStats.Comparer );
        foreach ( var stats in statsList )
        {
            VisualElement row = rowTemplate.Instantiate().Q();
            FillStatsRowWithValues( row, stats );
            _table.contentContainer.Add( row );
        }
    }

    private static void FillStatsRowWithValues( VisualElement row, SaveManager.SimulationStats stats )
    {
        row.Q<Label>( "Cell1" ).text = $"{stats.FrontFlockSize}";
        row.Q<Label>( "Cell2" ).text = $"{stats.BackFlockSize}";
        row.Q<Label>( "Cell3" ).text = $"{stats.FrameRate:0.##}";
    }

    private void ShowMenu()
    {
        _rootVisualElement.style.display = DisplayStyle.Flex;
    }

    private void HideMenu()
    {
        _rootVisualElement.style.display = DisplayStyle.None;
    }
}
}