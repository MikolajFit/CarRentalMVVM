using System;
using System.Collections.ObjectModel;
using System.Linq;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.UI.Mappers;
using CarRental.UI.ViewModels.ObservableObjects;
using GalaSoft.MvvmLight.CommandWpf;

namespace CarRental.UI.ViewModels.AdminViewModels
{
    public class RentalAreaManagementViewModel : CustomViewModelBase
    {
        private readonly IRentalAreaService _rentalAreaService;
        private readonly IRentalAreaViewModelMapper _rentalAreaViewModelMapper;
        private ObservableCollection<RentalAreaViewModel> _rentalAreas;
        private PositionViewModel _selectedPosition;

        private RentalAreaViewModel _selectedRentalArea;
        private string _saveErrorContent;
        private bool _isPositionListEnabled;
        private bool _isPositionTextEnabled;

        public RentalAreaManagementViewModel(IRentalAreaService rentalAreaService,
            IRentalAreaViewModelMapper rentalAreaViewModelMapper)
        {
            _rentalAreaService = rentalAreaService;
            _rentalAreaViewModelMapper = rentalAreaViewModelMapper;
            RentalAreas = new ObservableCollection<RentalAreaViewModel>();
            IsPositionListEnabled = true;
            RefreshRentalAreaList();
            SelectedRentalArea = RentalAreas.FirstOrDefault();
            AddPositionCommand = new RelayCommand(AddPosition,CanAddPosition);
            SavePositionCommand = new RelayCommand(SavePosition,CanSavePosition);
            AddRentalAreaCommand = new RelayCommand(AddRentalArea);
            SaveRentalAreaCommand = new RelayCommand(SaveRentalArea, CanSaveRentalArea);
        }

        private bool CanSaveRentalArea()
        {
            return SelectedRentalArea!=null && SelectedRentalArea.IsValid;
        }

        private void SaveRentalArea()
        {
            try
            {
                if (SelectedRentalArea.Id == Guid.Empty)
                {
                    SelectedRentalArea.Id = Guid.NewGuid();
                    var dto = _rentalAreaViewModelMapper.Map(SelectedRentalArea);
                    _rentalAreaService.CreateRentalArea(dto);
                }
                else
                {
                    var dto = _rentalAreaViewModelMapper.Map(SelectedRentalArea);
                    _rentalAreaService.UpdateRentalArea(dto);
                }

                RefreshRentalAreaList();
                SaveErrorContent = null;
                SelectedRentalArea = null;
            }
            catch (Exception e)
            {
                SaveErrorContent = e.Message;
                if (RentalAreas.First(a => a.Id == SelectedRentalArea.Id) == null)
                    if (SelectedRentalArea != null)
                        SelectedRentalArea.Id = Guid.Empty;
            }
        }

        private void AddRentalArea()
        {
            SelectedRentalArea = null;
            SelectedRentalArea = new RentalAreaViewModel();
        }

        private bool CanSavePosition()
        {
            return SelectedPosition!= null && SelectedPosition.IsValid && SelectedRentalArea!=null;
        }

        private void SavePosition()
        {
            SelectedRentalArea.Area.Add(SelectedPosition);
            SelectedPosition = null;
            IsPositionListEnabled = true;
        }

        private void AddPosition()
        {
            SelectedPosition = new PositionViewModel();
            IsPositionListEnabled = false;
        }

        private bool CanAddPosition()
        {
            return SelectedRentalArea != null;
        }

        public ObservableCollection<RentalAreaViewModel> RentalAreas
        {
            get => _rentalAreas;
            set { Set(() => RentalAreas, ref _rentalAreas, value); }
        }

        public RentalAreaViewModel SelectedRentalArea
        {
            get => _selectedRentalArea;
            set { Set(() => SelectedRentalArea, ref _selectedRentalArea, value); }
        }

        public PositionViewModel SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                Set(() => SelectedPosition, ref _selectedPosition, value);
                IsPositionTextEnabled = value != null;
            }
        }

        public RelayCommand AddPositionCommand { get;  }
        public RelayCommand SavePositionCommand { get;  }
        public RelayCommand SaveRentalAreaCommand { get;  }
        public RelayCommand AddRentalAreaCommand { get;  }

        public string SaveErrorContent
        {
            get => _saveErrorContent;
            set { Set(() => SaveErrorContent, ref _saveErrorContent, value); }
        }

        public bool IsPositionListEnabled
        {
            get => _isPositionListEnabled;
            set { Set(() => IsPositionListEnabled, ref _isPositionListEnabled, value); }
        }

        public bool IsPositionTextEnabled
        {
            get => _isPositionTextEnabled;
            set { Set(() => IsPositionTextEnabled, ref _isPositionTextEnabled, value); }
        }

        private void RefreshRentalAreaList()
        {
            var rentalAreas = _rentalAreaService.GetAllRentalAreas();
            if (rentalAreas == null) return;
            RentalAreas.Clear();
            foreach (var rentalArea in rentalAreas.Select(
                rentalAreaDto => _rentalAreaViewModelMapper.Map(rentalAreaDto)))
            {
                RentalAreas.Add(rentalArea);
            }
        }
    }
}