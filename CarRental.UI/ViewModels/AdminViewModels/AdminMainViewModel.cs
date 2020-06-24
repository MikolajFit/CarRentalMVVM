using CarRental.UI.Services;

namespace CarRental.UI.ViewModels.AdminViewModels
{
    public class AdminMainViewModel : MainViewModel
    {
        public AdminMainViewModel(IMessengerService messengerService,
            DriversManagementViewModel driversManagementViewModel, CarsManagementViewModel carsCarsManagementViewModel,
            RentalAreaManagementViewModel rentalRentalAreaManagementViewModel,
            RentalsManagementViewModel rentalsManagementViewModel) : base(messengerService)
        {
            DriversManagementViewModel = driversManagementViewModel;
            CarsManagementViewModel = carsCarsManagementViewModel;
            RentalAreaManagementViewModel = rentalRentalAreaManagementViewModel;
            RentalsManagementViewModel = rentalsManagementViewModel;
        }

        public CarsManagementViewModel CarsManagementViewModel { get; }
        public RentalAreaManagementViewModel RentalAreaManagementViewModel { get; }
        public RentalsManagementViewModel RentalsManagementViewModel { get; }
        public DriversManagementViewModel DriversManagementViewModel { get; }
    }
}