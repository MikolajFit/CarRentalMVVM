﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using CarRental.UI.Mappers;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;

namespace CarRental.UI.ViewModels.AdminViewModels
{
    public class CarsManagementViewModel : CustomViewModelBase
    {
        private readonly ICarService _carService;
        private readonly ICarViewModelMapper _carViewModelMapper;
        private readonly IRentalAreaService _rentalAreaService;
        private ObservableCollection<CarViewModel> _carsCollection = new ObservableCollection<CarViewModel>();
        private bool _isCarListEnabled;
        private string _saveErrorContent;
        private CarViewModel _selectedCar;
        private RentalAreaDTO _selectedRentalArea;

        public CarsManagementViewModel(ICarService carService, IRentalAreaService rentalAreaService,
            ICarViewModelMapper carViewModelMapper)
        {
            IsCarListEnabled = true;
            _carService = carService;
            _rentalAreaService = rentalAreaService;
            _carViewModelMapper = carViewModelMapper;
            UpdateRentalAreaCombobox = new RelayCommand(UpdateRentalArea);
            AddNewCarCommand = new RelayCommand(AddNewCar);
            SaveCarCommand = new RelayCommand(SaveCar, IsCarValid);
            PopulateCarsListView();
            PopulateRentalAreasCombobox();
        }

        public ObservableCollection<CarViewModel> CarsCollection
        {
            get => _carsCollection;
            set { Set(() => CarsCollection, ref _carsCollection, value); }
        }

        public ObservableCollection<RentalAreaDTO> RentalAreas { get; set; }

        public CarViewModel SelectedCar
        {
            get => _selectedCar;
            set { Set(() => SelectedCar, ref _selectedCar, value); }
        }

        public RentalAreaDTO SelectedRentalArea
        {
            get => _selectedRentalArea;
            set { Set(() => SelectedRentalArea, ref _selectedRentalArea, value); }
        }

        public string SaveErrorContent
        {
            get => _saveErrorContent;
            set { Set(() => SaveErrorContent, ref _saveErrorContent, value); }
        }

        public RelayCommand UpdateRentalAreaCombobox { get; }
        public RelayCommand AddNewCarCommand { get; }
        public RelayCommand SaveCarCommand { get; }

        public bool IsCarListEnabled
        {
            get => _isCarListEnabled;
            set { Set(() => IsCarListEnabled, ref _isCarListEnabled, value); }
        }

        private void PopulateRentalAreasCombobox()
        {
            var rentalAreas = _rentalAreaService.GetAllRentalAreas();
            RentalAreas = new ObservableCollection<RentalAreaDTO>(rentalAreas);
        }

        private void PopulateCarsListView()
        {
            var cars = _carService.GetAllCars();
            CarsCollection.Clear();
            foreach (var carViewModel in cars.Select(car => _carViewModelMapper.Map(car)))
                CarsCollection.Add(carViewModel);
        }

        private bool IsCarValid()
        {
            return SelectedCar != null && SelectedCar.IsValid && SelectedRentalArea != null;
        }

        private void SaveCar()
        {
            try
            {
                if (SelectedCar.Id == Guid.Empty)
                {
                    SelectedCar.Id = Guid.NewGuid();
                    var dto = _carViewModelMapper.Map(SelectedRentalArea, SelectedCar);
                    _carService.CreateCar(dto);
                }
                else
                {
                    var dto = _carViewModelMapper.Map(SelectedRentalArea, SelectedCar, true);
                    _carService.UpdateCar(dto);
                }

                PopulateCarsListView();
                IsCarListEnabled = true;
                SaveErrorContent = null;
            }
            catch (Exception e)
            {
                SaveErrorContent = e.Message;
            }
        }

        private void AddNewCar()
        {
            SelectedCar = new CarViewModel();
            IsCarListEnabled = false;
        }

        private void UpdateRentalArea()
        {
            SelectedRentalArea =
                RentalAreas.FirstOrDefault(r => SelectedCar != null && r.Id == SelectedCar.RentalAreaId);
        }
    }
}